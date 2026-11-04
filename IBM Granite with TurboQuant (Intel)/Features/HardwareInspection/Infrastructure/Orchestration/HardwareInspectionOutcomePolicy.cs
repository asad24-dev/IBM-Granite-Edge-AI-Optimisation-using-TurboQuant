using GraniteEdgeAI.Features.HardwareInspection.Application;
using GraniteEdgeAI.Features.HardwareInspection.Resolution;
using System;
using System.Collections.Generic;

namespace GraniteEdgeAI.Features.HardwareInspection.Orchestration;

internal static class HardwareInspectionOutcomePolicy
{
    internal static HardwareInspectionRunResult FromResolution(
        Guid inspectionId,
        HardwareEvidenceResolutionResult resolution)
    {
        ArgumentNullException.ThrowIfNull(resolution);
        if (!resolution.IsResolved)
        {
            return HardwareInspectionRunResult.CreateFailed(
                inspectionId,
                HardwareInspectionFailureKind.CriticalEvidence,
                MapUnresolvedEvidenceCategory(resolution.Diagnostics));
        }

        HardwareInspectionOutcome outcome = resolution.Diagnostics.Count == 0
            ? HardwareInspectionOutcome.Completed
            : HardwareInspectionOutcome.CompletedWithWarnings;
        return HardwareInspectionRunResult.CreateCompleted(
            inspectionId,
            outcome,
            resolution.Snapshot!);
    }

    internal static HardwareInspectionRunResult FromAcquisitionFailure(
        Guid inspectionId,
        HardwareToolAcquisitionDiagnosticCode diagnostic) => diagnostic switch
        {
            HardwareToolAcquisitionDiagnosticCode.ToolNotAvailable => RepairFailure(
                inspectionId,
                "HI-TOOL-NOT-AVAILABLE"),
            HardwareToolAcquisitionDiagnosticCode.ToolIntegrityFailure => RepairFailure(
                inspectionId,
                "HI-TOOL-INTEGRITY"),
            HardwareToolAcquisitionDiagnosticCode.PackagedProbeUnavailable => RepairFailure(
                inspectionId,
                "HI-RUNTIME-PACKAGE"),
            _ => throw new ArgumentOutOfRangeException(nameof(diagnostic)),
        };

    internal static HardwareInspectionRunResult FromCollectionFailure(
        Guid inspectionId,
        HardwareEvidenceCollectionFailureCode failureCode) => failureCode switch
        {
            HardwareEvidenceCollectionFailureCode.ProviderUnavailable =>
                HardwareInspectionRunResult.CreateFailed(
                    inspectionId,
                    HardwareInspectionFailureKind.TransientOperation,
                    "HI-PROVIDER-UNAVAILABLE"),
            HardwareEvidenceCollectionFailureCode.OrchestrationFailure => RepairFailure(
                inspectionId,
                "HI-ORCHESTRATION-FAILED"),
            HardwareEvidenceCollectionFailureCode.ProgressCallbackFailure => RepairFailure(
                inspectionId,
                "HI-PROGRESS-CALLBACK"),
            _ => throw new ArgumentOutOfRangeException(nameof(failureCode)),
        };

    private static HardwareInspectionRunResult RepairFailure(
        Guid inspectionId,
        string diagnosticCode) => HardwareInspectionRunResult.CreateFailed(
            inspectionId,
            HardwareInspectionFailureKind.ApplicationRepairRequired,
            diagnosticCode);

    private static string MapUnresolvedEvidenceCategory(
        IReadOnlyList<HardwareResolutionDiagnosticCode> diagnostics)
    {
        ArgumentNullException.ThrowIfNull(diagnostics);

        if (diagnostics.Contains(HardwareResolutionDiagnosticCode.LlamaCppUnavailable))
        {
            return "HI-RUNTIME-UNAVAILABLE";
        }

        if (diagnostics.Contains(HardwareResolutionDiagnosticCode.LlamaCppProbeStartFailed))
        {
            return "HI-RUNTIME-PROBE-START";
        }

        if (diagnostics.Contains(HardwareResolutionDiagnosticCode.LlamaCppProbeExecutionFailed))
        {
            return "HI-RUNTIME-PROBE-EXECUTION";
        }

        if (diagnostics.Contains(HardwareResolutionDiagnosticCode.LlamaCppProbeProtocolInvalid))
        {
            return "HI-RUNTIME-PROBE-PROTOCOL";
        }

        if (diagnostics.Contains(HardwareResolutionDiagnosticCode.StorageUnavailable))
        {
            return "HI-STORAGE-UNAVAILABLE";
        }

        if (diagnostics.Contains(HardwareResolutionDiagnosticCode.WindowsSystemUnavailable) ||
            diagnostics.Contains(HardwareResolutionDiagnosticCode.NormalizationInvalid) ||
            diagnostics.Contains(HardwareResolutionDiagnosticCode.NormalizationOverflow))
        {
            return "HI-MEMORY-UNRESOLVED";
        }

        if (diagnostics.Contains(HardwareResolutionDiagnosticCode.WindowsProcessorUnavailable) ||
            diagnostics.Contains(HardwareResolutionDiagnosticCode.ProcessorNameConflict) ||
            diagnostics.Contains(HardwareResolutionDiagnosticCode.LogicalProcessorConflict) ||
            diagnostics.Contains(HardwareResolutionDiagnosticCode.ProcessorTopologyConflict))
        {
            return "HI-PROCESSOR-UNRESOLVED";
        }

        if (diagnostics.Contains(HardwareResolutionDiagnosticCode.CaptureSpanExceeded) ||
            diagnostics.Contains(HardwareResolutionDiagnosticCode.ClockFuture) ||
            diagnostics.Contains(HardwareResolutionDiagnosticCode.EvidenceStale))
        {
            return "HI-EVIDENCE-TIMING";
        }

        return "HI-EVIDENCE-UNRESOLVED";
    }
}
