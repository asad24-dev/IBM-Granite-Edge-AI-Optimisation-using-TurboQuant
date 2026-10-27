# 8 GB laptop test log

## Purpose and rules

This is the running physical-device record for Granite on the target 8 GB
Windows laptop. It distinguishes an application screen or prediction from a
successful local-inference run. Do not change a recommendation, memory reserve,
or supported configuration based on a single record.

For every model run, retain the exact error text and useful screenshots or logs.
A run is successful only if the model loads, answers the fixed short prompt,
remains responsive, and leaves reasonable Windows headroom.

This readable log is paired with the structured campaign records in
[`experiments/manifests/8gb-windows-laptop/`](../../experiments/manifests/8gb-windows-laptop/README.md).
The campaign uses its own identifiers and does not alter or reuse the frozen
16 GB route-campaign records.

## Device baseline

| Field | Observed value |
| --- | --- |
| Device | Lenovo 82QC |
| Operating system | Windows 11 Home, version 10.0.22631, build 22631, 64-bit |
| CPU | 12th Gen Intel(R) Core(TM) i3-1215U; 6 cores, 8 logical processors |
| Installed RAM | 8,301,043,712 bytes (7.73 GiB) |
| Graphics | Intel(R) UHD Graphics; driver 32.0.101.6129 |
| Repository clone | `C:\Users\Asad\src\granite` |
| Repository commit | `489a6f0d393c8a7c5e800a367c4d889099f40bcd` |
| Packaged application | Granite 1.0.4.0 setup package; bundle hashes and dependency signatures passed before installation |

The reported graphics adapter-memory value is not treated as proof of dedicated
or usable inference VRAM. Initial testing is CPU-oriented.

## Run records

### APP-8GB-BASE-001 — packaged application launch baseline

| Field | Record |
| --- | --- |
| Date/time | 2026-10-20 12:38:12 Europe/London |
| Windows last boot | 2026-10-20 10:44:02 |
| Available RAM before model load | 1,147 MiB |
| Free storage on C: | 19.28 GiB |
| Application action | `Setup-Granite.ps1 -Step Launch` in ordinary PowerShell |
| Result | Passed: Granite visibly opened to **Choose a model**. |
| Model loaded | No — no model was downloaded, imported, or loaded. |
| Selected configuration | None. The displayed catalogue initially showed Granite 4.0 H Micro, `Q4_K_M`, Balanced, 1.94 GB. |
| Screenshot evidence | Opening screen captured in the session record. |
| Interpretation | Application launch only. This is not evidence of model inference or 8 GB configuration support. |

### Preflight for APP-8GB-HM-Q2-001

| Field | Record |
| --- | --- |
| Date/time | 2026-10-20 13:40:24 Europe/London |
| Windows last boot | 2026-10-20 13:38:55 |
| Available RAM | 2,024 MiB |
| Free storage on C: | 19.43 GiB |
| Interpretation | A post-reboot measurement. Available RAM increased by 877 MiB compared with `APP-8GB-BASE-001`; it is not a model-load or inference measurement. |

## APP-8GB-HM-Q2-001 — H Micro Q2_K inspection attempt

| Field | Intended value |
| --- | --- |
| Test ID | `APP-8GB-HM-Q2-001` |
| Model | IBM Granite 4.0 H Micro GGUF, `Q2_K` (Maximum efficiency) |
| Expected file size | 1,226,247,840 bytes (~1.14 GiB) |
| Backend | CPU-oriented; record the exact backend shown by Granite |
| Cache/context | Record only the configuration actually offered and selected; start with a small context |
| Fixed prompt | `Explain what a computer processor does in two sentences.` |
| Success condition | Loads, returns a coherent short response, remains responsive, and does not leave Windows obviously memory-starved. |

**Observed result:** the app reached **Model inspection** after the in-app Q2_K
download flow, then stopped with **Inspection could not be completed**,
diagnostic code `INSPECT-OP-001`, status **RESULT UNKNOWN**, and the detail
**No model data was changed.** Hardware fit, configuration, model load and chat
were not reached. This is an operational inspection failure, not a measured
8 GB memory-fit result.

After the failure, the locally stored GGUF was independently checked: its
1,226,247,840-byte length and SHA-256
`e60b313fc0ce2a0a2c3903f4399ac61fe1048c38f219bcbd9f7a708e168b8ead`
match the pinned catalogue entry. The installed package is version `1.0.4.0`,
and all 44 files required by its ModelInspection worker manifest also matched
their pinned lengths and SHA-256 hashes. These checks rule out a damaged model
download or altered/missing packaged inspection-worker file; they do not yet
identify the operational failure cause.

### APP-8GB-HM-Q2-002 — controlled local-file inspection retry

| Field | Record |
| --- | --- |
| Test control | Granite was restarted; the already verified local Q2_K GGUF was selected through **Use a local model**. No new download or configuration change was made. |
| Result | Reproduced failure: **Inspection could not be completed**; status **RESULT UNKNOWN**; diagnostic code `INSPECT-OP-001`; **No model data was changed.** |
| Workflow reached | Model inspection only. Hardware fit, configuration, model load and chat were not reached. |
| Evidence | Screenshot captured in the session record. |
| Interpretation | The same error on the local-file route makes the original in-app download path unlikely to explain the failure. This remains an application inspection blocker; it is not evidence that the 8 GB hardware cannot run the model. |
| Next safe action | Pause model-selection retries. Investigate the packaged app's inspection path/version and collect a maintainable diagnostic trace before attempting a different model or configuration. |

### APP-8GB-HM-Q2-003 — short-path hard-link inspection test

| Field | Record |
| --- | --- |
| Test control | Granite selected `C:\Users\Asad\AppData\Local\GraniteDiagnostics\HMicro-Q2K.gguf` through **Use a local model**. This is a hard link to the same independently verified Q2_K bytes, not a new download or copy. |
| Result | Reproduced failure immediately: **Inspection could not be completed**; status **RESULT UNKNOWN**; diagnostic code `INSPECT-OP-001`; **No model data was changed.** |
| Visible stage | **Check model package**, 0 of 5 checks complete, 0%. |
| Workflow reached | Model inspection only. Hardware fit, configuration, model load and chat were not reached. |
| Interpretation | The failure also occurs from a short ordinary local path using the exact same bytes. The original package-local path and path length are unlikely explanations. This remains a packaged-app inspection blocker, not an 8 GB hardware-fit result. |
| Next safe action | Stop path/model-selection retries. Use an approved instrumented/development build or maintainable packaged-app diagnostic trace to obtain the hidden worker failure details. |

### DIAG-8GB-HM-Q2-001 — source VocabOnly inspection diagnostic

| Field | Record |
| --- | --- |
| Date | 2026-10-26 Europe/London |
| Scope | One read-only source-runtime inspection of the exact verified H Micro Q2_K GGUF. This is not an application chat/inference run. |
| Diagnostic route | Checked-out `VocabOnlyModelProbe` through the matched LLamaSharp CPU runtime; no context, KV cache, generation, GPU offload, conversion or quantisation. |
| Result | **Succeeded**. Granite 4.0 H Micro opened; tokenizer smoke passed; native handle closed successfully. |
| Timing | 10,982 ms total; 597 ms native model load. |
| Inspection-only working set | 23,842,816 bytes before; 80,502,784-byte peak; 46,940,160 bytes after disposal. These figures are not inference memory measurements. |
| Model integrity | Post-probe SHA-256 remained `e60b313fc0ce2a0a2c3903f4399ac61fe1048c38f219bcbd9f7a708e168b8ead`. |
| Interpretation | The current source inspection runtime can open this exact model on the Lenovo. The repeated packaged-app `INSPECT-OP-001` is therefore narrowed to the package's worker-launch/client/protocol/environment boundary, not proof of a corrupt model or an 8 GB memory-fit failure. |
| Next safe action | Do not call this an end-to-end success. Obtain package/source provenance or add privacy-safe worker-result diagnostics in a future instrumented build before retrying model/configuration experiments. |

### DIAG-8GB-HM-Q2-002 — source protected-worker/client inspection diagnostic

| Field | Record |
| --- | --- |
| Date | 2026-10-26 Europe/London |
| Scope | One read-only inspection of the exact verified H Micro Q2_K GGUF through the checked-out source's real `InspectionWorkerClient` and its separately staged, source-built worker. This is not the installed packaged app. |
| Diagnostic coverage | The source client resolved the fixed worker layout, launched the protected worker, completed its handshake and protocol exchange, received trusted terminal evidence, and observed a normal worker exit. |
| Result | **Completed**; client failure code `none`; worker failure code `none`; exit code `0`; no forced termination, retained stderr or secondary diagnostics. |
| Model integrity | Post-run SHA-256 remained `e60b313fc0ce2a0a2c3903f4399ac61fe1048c38f219bcbd9f7a708e168b8ead`. |
| Interpretation | The current checked-out source worker/client route works end-to-end on this laptop and model. The installed 1.0.4.0 app failure is thereby further narrowed to its package/source-version or package-specific environment boundary. This does not make the installed app pass. |
| Next safe action | Do not delete the verified model merely to create development space. Continue source-level diagnostics without Visual Studio where possible; an actual instrumented WinUI/MSIX app build still needs more storage, the development workload and matching packaging provenance. |

### DIAG-8GB-HM-Q2-003 — staged installed-worker comparison

| Field | Record |
| --- | --- |
| Date | 2026-10-26 Europe/London |
| Scope | One read-only comparison using a temporary copy of the installed 1.0.4.0 package's verified `ModelInspection\\Worker` folder. The original WindowsApps package and model were not changed. |
| Diagnostic route | The staged shipped worker was invoked through the same protected-client diagnostic used for `DIAG-8GB-HM-Q2-002`. |
| Result | **Completed**; client failure code `none`; worker failure code `none`; exit code `0`; no forced termination, retained stderr or secondary diagnostics. |
| Interpretation | The shipped worker executable and its staged dependencies can inspect the verified model on this device. Because staging does not reproduce the installed app's package context, this does not contradict the app failure. It narrows the remaining cause to installed-app composition, package-context launch/environment, or a source/package client-version boundary. |
| Next safe action | Preserve the installed package and model. Do not claim app success. The useful remaining target is the installed app's worker composition/launch boundary, which requires either package-specific telemetry/provenance or a rebuilt instrumented app. |

### SRC-8GB-HM-Q2-001 — source CPU inference configuration check

| Field | Record |
| --- | --- |
| Date | 2026-10-26 Europe/London |
| Scope | First bounded source-runtime inference attempt; not a packaged-app run. |
| Intended configuration | H Micro Q2_K; CPU only; 512-token context; Q4_0 key/value cache; 4 threads; batch size 32; Flash Attention **off**; 32-token output cap. |
| Available memory before / after | 1,840 MiB / 1,845 MiB. No peak process measurement was captured. |
| Result | Failed before generation with Windows exit code `-1073741819` (native access violation). |
| Relevant native diagnostic | `V cache quantization requires flash_attn`. |
| Interpretation | Q4_0 value-cache quantisation with Flash Attention disabled is an invalid runtime combination. This is a configuration finding, not evidence of a corrupted model or insufficient 8 GB hardware. |
| Model integrity after run | SHA-256 remained `e60b313fc0ce2a0a2c3903f4399ac61fe1048c38f219bcbd9f7a708e168b8ead`. |

### SRC-8GB-HM-Q2-002 — first bounded source CPU prompt-and-response run

| Field | Record |
| --- | --- |
| Date | 2026-10-26 Europe/London |
| Scope | One controlled source-runtime response; not a packaged-app success. |
| Configuration | H Micro Q2_K; CPU only; 512-token context; Q4_0 key/value cache; 4 threads; batch size 32; Flash Attention **on**; 32-token output cap. |
| Prompt | `Explain what a computer processor does in two short sentences.` |
| Available memory before / after | 1,815 MiB / 2,039 MiB. No peak process/system memory was captured. |
| Load / execution result | `G1READY`, `G1RESPONSE`, and normal exit code `0`. Total process time: 21.16 seconds. This is not a time-to-first-token or generation-rate measurement. |
| Output observation | The model produced a coherent response beginning: `A computer processor, also known as a "CPU" (Central Processing Unit), is the primary component that enables a computer to execute instructions and process data. It` |
| Completion | `G1DONE length`: the response was intentionally cut off by the 32-token cap, so it does not meet the two-sentence prompt requirement and is not yet a response-quality pass. |
| Model integrity after run | SHA-256 remained `e60b313fc0ce2a0a2c3903f4399ac61fe1048c38f219bcbd9f7a708e168b8ead`. |
| Interpretation | The exact H Micro Q2_K model can load, create a small CPU context, and generate text on the Lenovo under this exact bounded source-runtime configuration. This is the first local inference evidence, but it does not unblock or validate the packaged application. |
| Next safe action | Repeat after a clean baseline with a modestly larger output cap, record peak memory and timing per stage, then test a short second turn before changing context/model/quantisation variables. |

Before starting this run, reboot if practical and record date/time, available RAM,
free storage, app version/build, exact model hash and file size, selected
backend/cache/context, load result, time to first response where available,
generation speed where available, process/system memory, response observation,
stability, and full errors.

## Run template

### RUN-<ID> — <model/configuration>

| Field | Record |
| --- | --- |
| Date/time and operator | |
| Device/Windows/build | |
| Available RAM and free storage before start | |
| App build/commit | |
| Model source, filename, SHA-256, size, weight quantisation | |
| Backend/runtime and cache/TurboQuant option | |
| Context limit and other settings | |
| Prompt and output limit | |
| Load result and duration | |
| First response / generation speed | |
| Peak process/system memory | |
| Output-quality observation | |
| Stability/headroom observation | |
| Result and full error text | |
| Screenshot/log paths | |
| Interpretation and next safe action | |
