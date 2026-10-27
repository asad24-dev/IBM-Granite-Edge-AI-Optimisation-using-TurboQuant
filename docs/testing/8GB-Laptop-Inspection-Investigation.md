# 8 GB Laptop — Model Inspection Investigation

**Investigation ID:** `INV-8GB-INSPECT-001`  
**Date:** 20–26 October 2026  
**Status:** Packaged-app inspection blocker narrowed; exact packaged worker-client failure remains hidden  
**Related runs:** `APP-8GB-HM-Q2-001`, `APP-8GB-HM-Q2-002`

## Observed behaviour

The packaged Granite application version `1.0.4.0` reached **Model inspection**
for the verified Granite 4.0 H Micro `Q2_K` GGUF, then displayed:

- `RESULT UNKNOWN`
- `Inspection could not be completed`
- `Diagnostic code: INSPECT-OP-001`
- `No model data was changed.`

The same outcome occurred both after the in-app verified download and after
restarting the app and choosing the same local GGUF. Hardware fit,
configuration and chat were not reached.

## What `INSPECT-OP-001` means in the source

It is a fixed UI label for the generic operational-failure presentation, not a
root-cause diagnostic. The current source displays that label for any failure
that reaches the operational-failure state.

The protected worker-client taxonomy includes, among others:

- worker launch, containment, handle-policy or environment-policy failure;
- handshake/protocol/output failure;
- unexpected worker exit, timeout or cleanup failure;
- worker-package-integrity failure; and
- a worker-reported controlled operational failure.

The native inspection engine can also classify model continuity, file access,
native runtime availability/architecture, model load and I/O failures. The
normal app UI does not expose which of these occurred, retained worker stderr,
or the client secondary diagnostics.

## Evidence checked

| Check | Result | Interpretation |
| --- | --- | --- |
| GGUF size and SHA-256 | Matches the pinned expected artefact. | The downloaded model is not evidently corrupted. |
| Installed inspection-worker closure | All 44 worker-manifest files matched their expected lengths and SHA-256 values. | Missing or altered packaged worker files are unlikely. |
| Package metadata | Package status is `Ok`; installed as a non-development package. | The registered package itself is present. |
| Setup re-check | Bundle hashes and Microsoft dependency signatures passed; the required `Microsoft.WindowsAppRuntime.2` version `2.5.1.0` x64 dependency is installed with status `Ok`. | An incomplete documented setup/dependency installation is unlikely. |
| Windows Application events | No Granite/ModelInspection crash event found around the attempts. | No evidence of an unhandled process crash. |
| Code Integrity, AppLocker and Defender operational logs | No matching block/deny event found. | No evidence of an obvious Windows security block. |
| Local-file retry | Same failure as downloaded-model route. | The original download path is unlikely to explain the failure. |

## Controlled standalone-worker diagnostic attempt

The packaged worker was launched directly from ordinary PowerShell only to try
its documented read-only inspection protocol. Windows denied the process start
from the protected `WindowsApps` installation folder. This is expected to be a
different execution context from the packaged application and does **not** show
that the application itself was denied access. It did not execute the worker,
read the model, or change the model/repository.

## Current conclusion

The failure is reproducible but is intentionally collapsed by the packaged app
into a generic safe error. The available evidence rules out damaged model bytes,
altered worker files and an obvious logged security or crash event. It does not
identify whether the remaining cause is worker launch, protocol, native runtime
initialisation, model-load handling, or an application/worker version boundary.

This is not evidence that the 8 GB laptop cannot run the model: no hardware-fit
or inference step occurred.

## Controlled source-runtime probe — 26 October 2026

To test the model/runtime layer without changing the installed application, a
temporary diagnostic harness outside the repository invoked the checked-out
source `VocabOnlyModelProbe` once against the exact verified Q2_K GGUF. This
operation opens the GGUF read-only, does not create a context or KV cache, and
does not generate text.

| Field | Observed result |
| --- | --- |
| Development prerequisite | .NET SDK `10.0.301`, the version selected by `global.json`, was installed. No Visual Studio or app rebuild was installed. |
| Probe completion | `Succeeded` |
| Probe duration | 10,982 ms |
| Native model-load duration | 597 ms |
| Runtime result | Granite H Micro opened through the matched LLamaSharp CPU `VocabOnly` path. |
| Metadata | Architecture `granitehybrid`; model name `Granite 4.0 H Micro`; declared context `1,048,576`. |
| Tokenizer smoke | Passed. |
| Native-handle cleanup | Closed successfully. |
| Working set | 23,842,816 bytes before; 80,502,784-byte peak; 46,940,160 bytes after disposal. This is inspection-only process memory, not inference memory. |
| Model integrity after probe | SHA-256 remained `e60b313fc0ce2a0a2c3903f4399ac61fe1048c38f219bcbd9f7a708e168b8ead`. |

This is **not** an end-to-end application or inference success. It does show
that this exact model can be opened by the current source inspection runtime on
this laptop. Therefore, the immediate packaged-app failure is unlikely to be a
corrupt model, model path, native model-load failure, or an 8 GB memory-fit
failure.

## Bounded source inference follow-up — 26 October 2026

The repository's existing source CPU runtime was then used outside the packaged
application. With the same Q2_K GGUF, a 512-token context, Q4_0 key/value
cache, four CPU threads, batch size 32, Flash Attention enabled and a 32-token
cap, the runtime reached `G1READY`, generated text and exited normally. The
model SHA-256 remained unchanged afterwards.

An immediately preceding run with Q4_0 value cache and Flash Attention disabled
failed before generation; the native runtime reported `V cache quantization
requires flash_attn`. That is a useful runtime-configuration constraint but is
separate from the packaged inspection failure, which occurs before a user can
choose cache settings.

The successful bounded source result establishes that the current source route
can load and generate from the model on this device. It does **not** establish
packaged-app success, a stable conversation, response-quality success, peak
memory headroom or a recommended 8 GB configuration.

## Package/source boundary finding

The installed `1.0.4.0` package has a healthy registered status and its worker
manifest matches its packaged files, but its inspection-worker assemblies are
not byte-identical in size to the currently checked-out source build. The
repository's own package handover record states that the source-to-package
build provenance was not independently established. The current source probe
therefore cannot be treated as proof that the supplied package contains the
same worker behaviour.

The remaining likely boundary is the packaged worker launch/client/protocol or
its package-specific worker environment. The generic UI label still hides the
specific safe failure code, exit code, retained redacted stderr and secondary
diagnostics needed to identify that subfailure.

## Next diagnostic decision

### Controlled path/location test prepared

The observed screen remains at 0% of **Check model package** and then fails
within about a second. In the source, this is before native model loading:
the worker first checks the expected file identity and opens/hashes the GGUF
read-only. A hard link to the same verified bytes has been created at:

`C:\Users\Asad\AppData\Local\GraniteDiagnostics\HMicro-Q2K.gguf`

Its size and SHA-256 match the original model. A hard link is not a second
download or copy and does not alter the original GGUF. The app reproduced the
same immediate `INSPECT-OP-001` failure from this path, with **Check model
package** still at 0%. The original package-local path and its length are
therefore unlikely explanations.

### Recommended next diagnostic action

Do not continue changing models or settings randomly. Add a maintainable,
privacy-safe diagnostic presentation to a future instrumented build that
records the existing bounded worker result fields (failure code, exit code,
retained redacted stderr and secondary diagnostics) without exposing model
paths or prompts. Rebuilding and re-signing a replacement MSIX requires the
separate Visual Studio/packaging setup and release inputs that are not currently
available or appropriate for this storage-constrained 8 GB laptop.

## Development-build preflight — 26 October 2026

The documented diagnostic-build route was checked without changing the
repository source or the installed Granite package. Visual Studio/MSBuild was
not installed. The official Visual Studio Community 2026 installer was invoked
with the repository's observed Universal Windows, Windows App SDK C# and x64
C++ components. Its preflight stopped before installing components because the
system drive had **10.92 GiB** free and Visual Studio requires at least
**15.4 GiB**.

No Visual Studio instance or partial development workload was installed. This
is a reproducible storage blocker for an instrumented application build, not
evidence of a model, setup-package or application-code failure. The current
source tree can still support the smaller worker/runtime investigations already
recorded above, but it cannot be compiled into a testable WinUI/MSIX app here
until sufficient storage and the repository's verified packaging inputs are
available.

## Source worker/client end-to-end diagnostic — 26 October 2026

A temporary diagnostic outside the repository used the installed .NET SDK and
the checked-out `InspectionWorkerClient`, with a separately staged copy of the
already-built source worker in its required fixed layout. It invoked one
read-only inspection of the exact verified H Micro Q2_K GGUF.

The protected worker launched, completed its handshake and protocol exchange,
returned a trusted `Completed` terminal message, and exited with code `0`.
There was no client failure, worker controlled failure, forced termination,
retained stderr or secondary diagnostic. The model SHA-256 remained
`e60b313fc0ce2a0a2c3903f4399ac61fe1048c38f219bcbd9f7a708e168b8ead`.

This is stronger than the direct runtime probe: the current source's actual
worker-client containment path works on the laptop. It does not repair or
validate the installed 1.0.4.0 package, because the source and signed package
worker artefacts have not been proven identical. It does mean Visual Studio is
not required for the remaining source-worker diagnostics; it is required only
to compile and test a changed WinUI/MSIX application package.

## Staged installed-worker comparison — 26 October 2026

For a package-boundary comparison, a temporary copy of the installed 1.0.4.0
package's verified `ModelInspection\\Worker` folder was placed outside
`WindowsApps` and invoked by the same protected-client diagnostic. The original
package and model were not changed. The staged shipped worker returned trusted
`Completed` evidence with exit code `0`, no client or worker failure code, no
forced termination, no retained stderr and no secondary diagnostics.

This demonstrates that the shipped worker executable and its staged
dependencies can inspect the verified model on this laptop. It cannot reproduce
the installed app's package context, so it does not establish app success. The
remaining failure boundary is now specifically the installed app's composition
or package-context worker launch/environment, including the possibility of a
client/version difference between source and supplied package.
