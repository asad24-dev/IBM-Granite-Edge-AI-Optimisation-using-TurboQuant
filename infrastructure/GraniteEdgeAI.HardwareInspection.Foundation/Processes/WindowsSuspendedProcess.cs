using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using GraniteEdgeAI.HardwareInspection.Foundation.TrustedTools;

namespace GraniteEdgeAI.HardwareInspection.Foundation.Processes;

internal sealed class WindowsSuspendedProcess : IDisposable
{
    private readonly BoundedCleanupCoordinator _cleanup;

    private WindowsSuspendedProcess(
        Process process,
        SafeProcessHandle processHandle,
        AnonymousPipeServerStream standardOutput,
        AnonymousPipeServerStream standardError,
        TrustedToolOperationEnvironment operationEnvironment)
    {
        Process = process;
        ProcessHandle = processHandle;
        StandardOutput = standardOutput;
        StandardError = standardError;
        OperationEnvironment = operationEnvironment;
        _cleanup = new BoundedCleanupCoordinator(
            Sync(OwnedCleanupStage.StandardOutput, StandardOutput.Dispose),
            Sync(OwnedCleanupStage.StandardError, StandardError.Dispose),
            Sync(OwnedCleanupStage.Session, Process.Dispose),
            Sync(OwnedCleanupStage.ProcessHandle, ProcessHandle.Dispose),
            Sync(OwnedCleanupStage.OperationEnvironment, () =>
            {
                OperationEnvironment.Dispose();
                if (!OperationEnvironment.CleanupSucceeded)
                {
                    throw new InvalidOperationException(
                        "The trusted operation environment cleanup could not be verified.");
                }
            }));
    }

    internal Process Process { get; }

    private SafeProcessHandle ProcessHandle { get; }

    internal Stream StandardOutput { get; }

    internal Stream StandardError { get; }

    private TrustedToolOperationEnvironment OperationEnvironment { get; }

    internal bool CleanupSucceeded { get; private set; }

    internal CleanupOutcome? CleanupOutcome { get; private set; }

    internal static bool TryStart(
        VerifiedTrustedTool tool,
        TrustedToolCommand command,
        WindowsKillOnCloseJob job,
        out WindowsSuspendedProcess? launched) =>
        TryStart(tool, command, job, operationStorageRoot: null, out launched, out bool _);

    internal static bool TryStart(
        VerifiedTrustedTool tool,
        TrustedToolCommand command,
        WindowsKillOnCloseJob job,
        out WindowsSuspendedProcess? launched,
        out bool startCleanupSucceeded) =>
        TryStart(
            tool,
            command,
            job,
            operationStorageRoot: null,
            out launched,
            out startCleanupSucceeded);

    internal static bool TryStart(
        VerifiedTrustedTool tool,
        TrustedToolCommand command,
        WindowsKillOnCloseJob job,
        string? operationStorageRoot,
        out WindowsSuspendedProcess? launched,
        out bool startCleanupSucceeded)
    {
        ArgumentNullException.ThrowIfNull(tool);
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(job);

        TrustedToolOperationEnvironment operationEnvironment;
        try
        {
            operationEnvironment = TrustedToolOperationEnvironment.CreateCurrent(
                includeDotnetRoots: false,
                localApplicationDataOverride: operationStorageRoot);
        }
        catch (InvalidOperationException error)
        {
            launched = null;
            startCleanupSucceeded = !error.Message.Contains(
                "partial cleanup",
                StringComparison.Ordinal);
            return false;
        }

        return TryStartCore(
            tool.ExecutablePath,
            command.Arguments,
            tool.PackageRoot,
            operationEnvironment,
            job,
            out launched,
            out startCleanupSucceeded);
    }

    internal static bool TryStart(
        string executablePath,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        TrustedToolOperationEnvironment operationEnvironment,
        WindowsKillOnCloseJob job,
        out WindowsSuspendedProcess? launched,
        out bool startCleanupSucceeded)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(executablePath);
        ArgumentNullException.ThrowIfNull(arguments);
        ArgumentException.ThrowIfNullOrWhiteSpace(workingDirectory);
        ArgumentNullException.ThrowIfNull(operationEnvironment);
        ArgumentNullException.ThrowIfNull(job);
        return TryStartCore(
            executablePath,
            arguments,
            workingDirectory,
            operationEnvironment,
            job,
            out launched,
            out startCleanupSucceeded);
    }

    internal static bool TryStartWithOutcome(
        string executablePath,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        TrustedToolOperationEnvironment operationEnvironment,
        WindowsKillOnCloseJob job,
        out WindowsSuspendedProcess? launched,
        out CleanupOutcome startCleanupOutcome)
    {
        bool started = TryStartCore(
            executablePath,
            arguments,
            workingDirectory,
            operationEnvironment,
            job,
            out launched,
            out startCleanupOutcome);
        return started;
    }

    private static bool TryStartCore(
        string executablePath,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        TrustedToolOperationEnvironment suppliedEnvironment,
        WindowsKillOnCloseJob job,
        out WindowsSuspendedProcess? launched,
        out bool startCleanupSucceeded)
    {
        bool started = TryStartCore(
            executablePath,
            arguments,
            workingDirectory,
            suppliedEnvironment,
            job,
            out launched,
            out CleanupOutcome outcome);
        startCleanupSucceeded = outcome.Succeeded;
        if (started && !outcome.Succeeded && launched is not null)
        {
            _ = AbortTransferredStart(launched, job);
            launched = null;
            return false;
        }
        return started;
    }

    private static bool TryStartCore(
        string executablePath,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        TrustedToolOperationEnvironment suppliedEnvironment,
        WindowsKillOnCloseJob job,
        out WindowsSuspendedProcess? launched,
        out CleanupOutcome startCleanupOutcome)
    {
        launched = null;
        startCleanupOutcome = new CleanupOutcome([]);
        AnonymousPipeServerStream? standardInput = null;
        AnonymousPipeServerStream? standardOutput = null;
        AnonymousPipeServerStream? standardError = null;
        ProcessInformation processInformation = default;
        bool processCreated = false;
        IntPtr attributeList = IntPtr.Zero;
        IntPtr jobHandleList = IntPtr.Zero;
        IntPtr inheritedHandleList = IntPtr.Zero;
        bool attributeListInitialized = false;
        SafeProcessHandle? processHandle = null;
        Process? process = null;
        TrustedToolOperationEnvironment? operationEnvironment = suppliedEnvironment;
        try
        {
            standardInput = new AnonymousPipeServerStream(
                PipeDirection.Out,
                HandleInheritability.Inheritable);
            standardOutput = new AnonymousPipeServerStream(
                PipeDirection.In,
                HandleInheritability.Inheritable);
            standardError = new AnonymousPipeServerStream(
                PipeDirection.In,
                HandleInheritability.Inheritable);
            IntPtr[] inheritedHandles =
            [
                standardInput.ClientSafePipeHandle.DangerousGetHandle(),
                standardOutput.ClientSafePipeHandle.DangerousGetHandle(),
                standardError.ClientSafePipeHandle.DangerousGetHandle(),
            ];
            if (!TryCreateLaunchAttributeList(
                    inheritedHandles,
                    job.Handle,
                    out attributeList,
                    out inheritedHandleList,
                    out jobHandleList))
            {
                return false;
            }

            attributeListInitialized = true;
            StartupInformationEx startup = new()
            {
                StartupInformation = new StartupInformation
                {
                    Size = Marshal.SizeOf<StartupInformationEx>(),
                    Flags = StartfUseStdHandles,
                    StandardInput = inheritedHandles[0],
                    StandardOutput = inheritedHandles[1],
                    StandardError = inheritedHandles[2],
                },
                AttributeList = attributeList,
            };
            string commandLine = BuildCommandLine(executablePath, arguments);
            if (commandLine.Length >= MaximumCommandLineLength)
            {
                return false;
            }

            using TrustedToolEnvironmentBlock environment =
                TrustedToolEnvironmentBlock.Create(operationEnvironment.Variables);

            processCreated = CreateProcess(
                executablePath,
                (commandLine + '\0').ToCharArray(),
                IntPtr.Zero,
                IntPtr.Zero,
                inheritHandles: true,
                CreateNoWindow | ExtendedStartupInfoPresent |
                    CreateUnicodeEnvironment,
                environment.Pointer,
                workingDirectory,
                ref startup,
                out processInformation);
            standardInput.DisposeLocalCopyOfClientHandle();
            standardOutput.DisposeLocalCopyOfClientHandle();
            standardError.DisposeLocalCopyOfClientHandle();
            if (!processCreated)
            {
                return false;
            }

            if (!job.ContainsProcess(processInformation.Process))
            {
                return false;
            }

            process = Process.GetProcessById(checked((int)processInformation.ProcessId));
            processHandle = new SafeProcessHandle(
                processInformation.Process,
                ownsHandle: true);
            processInformation.Process = IntPtr.Zero;
            launched = new WindowsSuspendedProcess(
                process,
                processHandle,
                standardOutput,
                standardError,
                operationEnvironment);
            operationEnvironment = null;
            processHandle = null;
            process = null;
            standardOutput = null!;
            standardError = null!;
            return true;
        }
        catch (Exception error) when (
            error is IOException or UnauthorizedAccessException or
                InvalidOperationException or ArgumentException or OverflowException or
                System.ComponentModel.Win32Exception)
        {
            return false;
        }
        finally
        {
            startCleanupOutcome = CleanupFailedStart(
                processCreated && operationEnvironment is not null,
                job,
                standardInput,
                standardOutput,
                standardError,
                process,
                attributeListInitialized,
                attributeList,
                jobHandleList,
                inheritedHandleList,
                processInformation,
                processHandle,
                operationEnvironment);
        }
    }

    private static CleanupOutcome CleanupFailedStart(
        bool processCreated,
        WindowsKillOnCloseJob job,
        AnonymousPipeServerStream? standardInput,
        AnonymousPipeServerStream? standardOutput,
        AnonymousPipeServerStream? standardError,
        Process? process,
        bool attributeListInitialized,
        IntPtr attributeList,
        IntPtr jobHandleList,
        IntPtr inheritedHandleList,
        ProcessInformation processInformation,
        SafeProcessHandle? processHandle,
        TrustedToolOperationEnvironment? operationEnvironment)
    {
        BoundedCleanupCoordinator cleanup = new(
            new OwnedCleanupAction(OwnedCleanupStage.ProcessTree, async () =>
            {
                if (processCreated &&
                    (!job.TryTerminate() ||
                     !await job.WaitForEmptyAsync(TimeSpan.FromSeconds(5))
                         .ConfigureAwait(false)))
                {
                    throw new InvalidOperationException(
                        "The protected process tree cleanup could not be verified.");
                }
            }),
            Sync(OwnedCleanupStage.StandardInput, () => standardInput?.Dispose()),
            Sync(OwnedCleanupStage.StandardOutput, () => standardOutput?.Dispose()),
            Sync(OwnedCleanupStage.StandardError, () => standardError?.Dispose()),
            Sync(OwnedCleanupStage.Session, () => process?.Dispose()),
            Sync(OwnedCleanupStage.Closure, () =>
            {
                if (attributeListInitialized)
                {
                    DeleteProcThreadAttributeList(attributeList);
                }
                if (inheritedHandleList != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(inheritedHandleList);
                }
                if (jobHandleList != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(jobHandleList);
                }
                if (attributeList != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(attributeList);
                }
            }),
            Sync(OwnedCleanupStage.ProcessHandle, () =>
            {
                bool handlesClosed = true;
                if (processInformation.Thread != IntPtr.Zero)
                {
                    handlesClosed &= CloseHandle(processInformation.Thread);
                }
                if (processInformation.Process != IntPtr.Zero)
                {
                    handlesClosed &= CloseHandle(processInformation.Process);
                }
                processHandle?.Dispose();
                if (!handlesClosed)
                {
                    throw new System.ComponentModel.Win32Exception();
                }
            }),
            Sync(OwnedCleanupStage.OperationEnvironment, () =>
            {
                operationEnvironment?.Dispose();
                if (operationEnvironment is not null &&
                    !operationEnvironment.CleanupSucceeded)
                {
                    throw new InvalidOperationException(
                        "The trusted operation environment cleanup could not be verified.");
                }
            }));
        return cleanup.ExecuteAsync().GetAwaiter().GetResult();
    }

    private static CleanupOutcome AbortTransferredStart(
        WindowsSuspendedProcess launched,
        WindowsKillOnCloseJob job) =>
        new BoundedCleanupCoordinator(
            new OwnedCleanupAction(OwnedCleanupStage.ProcessTree, async () =>
            {
                if (!job.TryTerminate() ||
                    !await job.WaitForEmptyAsync(TimeSpan.FromSeconds(5))
                        .ConfigureAwait(false))
                {
                    throw new InvalidOperationException(
                        "The transferred process tree cleanup could not be verified.");
                }
            }),
            Sync(OwnedCleanupStage.Session, launched.Dispose))
        .ExecuteAsync().GetAwaiter().GetResult();

    internal bool TryGetExitCode(out int exitCode) =>
        NativeProcessExitCodeAuthority.TryRead(ProcessHandle, out exitCode);

    private static bool TryCreateLaunchAttributeList(
        IntPtr[] handles,
        IntPtr jobHandle,
        out IntPtr attributeList,
        out IntPtr inheritedHandleList,
        out IntPtr jobHandleList)
    {
        attributeList = IntPtr.Zero;
        inheritedHandleList = IntPtr.Zero;
        jobHandleList = IntPtr.Zero;
        nuint attributeListSize = 0;
        _ = InitializeProcThreadAttributeList(
            IntPtr.Zero,
            attributeCount: 2,
            flags: 0,
            ref attributeListSize);
        if (attributeListSize == 0)
        {
            return false;
        }

        attributeList = Marshal.AllocHGlobal(checked((nint)attributeListSize));
        if (!InitializeProcThreadAttributeList(
                attributeList,
                attributeCount: 2,
                flags: 0,
                ref attributeListSize))
        {
            return false;
        }

        try
        {
            int handleListSize = checked(handles.Length * IntPtr.Size);
            inheritedHandleList = Marshal.AllocHGlobal(handleListSize);
            for (int index = 0; index < handles.Length; index++)
            {
                Marshal.WriteIntPtr(
                    inheritedHandleList,
                    checked(index * IntPtr.Size),
                    handles[index]);
            }

            bool handlesAttached = UpdateProcThreadAttribute(
                attributeList,
                flags: 0,
                new IntPtr(ProcThreadAttributeHandleList),
                inheritedHandleList,
                checked((nuint)handleListSize),
                IntPtr.Zero,
                IntPtr.Zero);
            if (handlesAttached)
            {
                jobHandleList = Marshal.AllocHGlobal(IntPtr.Size);
                Marshal.WriteIntPtr(jobHandleList, jobHandle);
                bool jobAttached = UpdateProcThreadAttribute(
                    attributeList,
                    flags: 0,
                    new IntPtr(ProcThreadAttributeJobList),
                    jobHandleList,
                    checked((nuint)IntPtr.Size),
                    IntPtr.Zero,
                    IntPtr.Zero);
                if (jobAttached)
                {
                    return true;
                }
            }
        }
        catch
        {
            DeleteProcThreadAttributeList(attributeList);
            if (inheritedHandleList != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(inheritedHandleList);
            }
            if (jobHandleList != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(jobHandleList);
            }

            attributeList = IntPtr.Zero;
            inheritedHandleList = IntPtr.Zero;
            jobHandleList = IntPtr.Zero;
            throw;
        }

        DeleteProcThreadAttributeList(attributeList);
        if (inheritedHandleList != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(inheritedHandleList);
        }
        if (jobHandleList != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(jobHandleList);
        }
        attributeList = IntPtr.Zero;
        inheritedHandleList = IntPtr.Zero;
        jobHandleList = IntPtr.Zero;
        return false;
    }

    public void Dispose()
    {
        CleanupOutcome = _cleanup.ExecuteAsync().GetAwaiter().GetResult();
        CleanupSucceeded = CleanupOutcome.Succeeded;
    }

    private static OwnedCleanupAction Sync(
        OwnedCleanupStage stage,
        Action action) => new(stage, () =>
        {
            action();
            return ValueTask.CompletedTask;
        });

    private static string BuildCommandLine(
        string executablePath,
        IReadOnlyList<string> arguments)
    {
        var commandLine = new StringBuilder(QuoteArgument(executablePath));
        foreach (string argument in arguments)
        {
            commandLine.Append(' ');
            commandLine.Append(QuoteArgument(argument));
        }

        return commandLine.ToString();
    }

    private static string QuoteArgument(string argument)
    {
        if (argument.Length > 0 &&
            argument.IndexOfAny([' ', '\t', '\n', '\v', '"']) < 0)
        {
            return argument;
        }

        var quoted = new StringBuilder(argument.Length + 2);
        quoted.Append('"');
        int backslashes = 0;
        foreach (char character in argument)
        {
            if (character == '\\')
            {
                backslashes++;
                continue;
            }

            if (character == '"')
            {
                quoted.Append('\\', checked(backslashes * 2 + 1));
                quoted.Append('"');
                backslashes = 0;
                continue;
            }

            quoted.Append('\\', backslashes);
            backslashes = 0;
            quoted.Append(character);
        }

        quoted.Append('\\', checked(backslashes * 2));
        quoted.Append('"');
        return quoted.ToString();
    }

    private const int MaximumCommandLineLength = 32_767;
    private const uint CreateUnicodeEnvironment = 0x00000400;
    private const uint ExtendedStartupInfoPresent = 0x00080000;
    private const uint CreateNoWindow = 0x08000000;
    private const uint StartfUseStdHandles = 0x00000100;
    private const int ProcThreadAttributeHandleList = 0x00020002;
    private const int ProcThreadAttributeJobList = 0x0002000D;

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CreateProcess(
        string applicationName,
        [In, Out] char[] commandLine,
        IntPtr processAttributes,
        IntPtr threadAttributes,
        [MarshalAs(UnmanagedType.Bool)] bool inheritHandles,
        uint creationFlags,
        IntPtr environment,
        string currentDirectory,
        ref StartupInformationEx startupInformation,
        out ProcessInformation processInformation);

    [DllImport("kernel32.dll", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool InitializeProcThreadAttributeList(
        IntPtr attributeList,
        int attributeCount,
        uint flags,
        ref nuint size);

    [DllImport("kernel32.dll", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UpdateProcThreadAttribute(
        IntPtr attributeList,
        uint flags,
        IntPtr attribute,
        IntPtr value,
        nuint size,
        IntPtr previousValue,
        IntPtr returnSize);

    [DllImport("kernel32.dll")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern void DeleteProcThreadAttributeList(IntPtr attributeList);

    [DllImport("kernel32.dll", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(IntPtr handle);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct StartupInformation
    {
        internal int Size;
        internal string? Reserved;
        internal string? Desktop;
        internal string? Title;
        internal uint X;
        internal uint Y;
        internal uint XSize;
        internal uint YSize;
        internal uint XCountChars;
        internal uint YCountChars;
        internal uint FillAttribute;
        internal uint Flags;
        internal short ShowWindow;
        internal short Reserved2Size;
        internal IntPtr Reserved2;
        internal IntPtr StandardInput;
        internal IntPtr StandardOutput;
        internal IntPtr StandardError;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct StartupInformationEx
    {
        internal StartupInformation StartupInformation;
        internal IntPtr AttributeList;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ProcessInformation
    {
        internal IntPtr Process;
        internal IntPtr Thread;
        internal uint ProcessId;
        internal uint ThreadId;
    }
}
