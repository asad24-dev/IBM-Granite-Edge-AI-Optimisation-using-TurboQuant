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

### SETUP-8GB-LLMFIT-001 — pinned LLM Fit provisioning

| Field | Record |
| --- | --- |
| Date | 2026-11-18 Europe/London |
| Purpose | Make the optional LLM Fit evidence route available to the installed packaged app without weakening its hardware-compatibility policy. |
| Candidate | LLM Fit `1.1.9`; `llmfit.exe` SHA-256 `db82bcb17f065b7ff7528ffe9904b2b0e1cce0c843fcd659b4ee1432a2e72e19`. |
| Acquisition | The repository's existing acquisition script downloaded and verified the pinned candidate without executing it. |
| Provisioned inventory | An administrator placed only `llmfit.exe`, `LICENSE`, and `README.md` in the machine-wide folder expected by the app. |
| Result | The subsequent app hardware report listed LLM Fit among its information sources. |
| Interpretation | This is a local test dependency setup result, not a Granite model or inference result. The binary and machine-wide copy are not repository artifacts. |

### APP-8GB-HM-Q2-004 — post-release packaged compatibility check

| Field | Record |
| --- | --- |
| Date | 2026-11-18 Europe/London |
| App package | Granite `1.0.5.0` (`GraniteEdgeAI.Asad.Development`). |
| Model | Existing verified Granite 4.0 H Micro `Q2_K` GGUF; 1,226,247,840 bytes; SHA-256 `e60b313fc0ce2a0a2c3903f4399ac61fe1048c38f219bcbd9f7a708e168b8ead`. |
| Workflow reached | Model inspection and hardware inspection completed. The hardware report used LLM Fit, Windows, DXGI, neural-processor and llama.cpp evidence. |
| Compatibility result | Rejected before configuration: 2.7 GiB estimated peak memory, 669 MiB safe memory, 4K context. |
| Model loaded / chat | No / no. |
| Interpretation | This validates that the repaired packaged app can move past the former inspection blocker. It was performed during setup activity, so its memory values are not the clean baseline. |

### APP-8GB-HM-Q2-005 — fresh-restart minimal-workload compatibility baseline

| Field | Record |
| --- | --- |
| Date/time | 2026-11-18 12:21:52 UTC |
| Windows last boot | 2026-11-18 12:19:55 local time |
| App package | Granite `1.0.5.0` (`GraniteEdgeAI.Asad.Development_1.0.5.0_x64__mzyd3a5swg3k4`). |
| Test control | Fresh Windows restart. The preflight's largest-process list contains Windows components, Windows Terminal and PowerShell, but not Chrome, Visual Studio Code, Teams or SkillsBuild. |
| Preflight free physical RAM / C: free storage | 3,620,900 KiB / 20,912,185,344 bytes. |
| Model | Existing verified Granite 4.0 H Micro `Q2_K` GGUF; SHA-256 `e60b313fc0ce2a0a2c3903f4399ac61fe1048c38f219bcbd9f7a708e168b8ead`. |
| Offered route | llama.cpp; 4K context; cache not reported. |
| App memory assessment | 2.9 GiB available now, 512 MiB safety reserve, 2.4 GiB safe memory, 2.7 GiB estimated peak memory, 334 MB shortfall. |
| Result | **Blocked before configuration and chat:** `Not enough free RAM`. |
| Evidence | `experiments/raw-results/8gb-windows-laptop/APP-8GB-HM-Q2-005/`. |
| Interpretation | The current packaged Q2_K profile at its offered 4K context does not meet the app's safety threshold even under the recorded clean-start condition. This is not an inference, response-quality, or stability result. |
| Next safe action | Investigate a bounded lower-context profile before trying another model, quantisation, or workload. |

### APP-8GB-HM-Q2-006 — first 2K-context packaged-app chat observation

| Field | Record |
| --- | --- |
| Date / preflight timestamp | 2026-11-21 / `2026-11-21T18:21:35.4146675+00:00` |
| App package | `GraniteEdgeAI.Asad.Context2048Experiment` `1.0.5.1`, x64; SHA-256 `1BE1668875A9B5344E062AE4C731F137133FC31D179F234F36B3A65678193174`. Operator-reported Authenticode result: Valid; signer `CN=Granite Edge AI Development`; certificate thumbprint `D6B842420C0F0FBC36758CAC7F22B52D39CAD911`. |
| Device / preflight | Windows 11 Home `10.0.22631` build `22631`; 12th Gen Intel Core i3-1215U; 6 physical cores / 8 logical processors; 8,301,043,712 bytes physical memory. Preflight free physical memory: 3,697,740 KiB; C: free: 14,563,500,032 bytes. |
| Model | Existing local Granite 4.0 H Micro `Q2_K` GGUF. The model hash is carried forward from the previous verified record; it was not rechecked in this run. |
| Inspections | Model inspection and hardware inspection completed, as reported by the run operator. |
| Displayed setup | Q2_K; llama.cpp; 2,048-token context; 3.5 GB free RAM now; 3 GB RAM available to the model; 2.5 GB estimated current-setup peak RAM; 512 MB safety reserve; no warning observed on the supplied configuration screen. These are displayed estimates/rounded values, not measured peak inference memory. |
| Chat route and cache | Chat screen displayed `llama.cpp · CPU` and `F16 cache (selected)`. |
| Turn 1 | Prompt: `Explain what a computer processor does in two short sentences`<br>Response: `A computer processor, also known as an "processor" or "CPU" (Central Processing Unit), is the main component in a computer that performs most of a computer's main tasks. It does this by executing instructions stored in computer memory.` |
| Turn 2 | Prompt: `What is one difference between a processor and memory?`<br>Response: `A processor is the part of the computer that executes instructions and performs calculations, while memory is the part of the computer that stores data and instructions temporarily or permanently.` |
| Measurements not captured | No formal response-time, generation-rate, or peak-inference-memory measurement was supplied. |
| Result | **One observed two-turn packaged-app chat session completed.** |
| Comparison with APP-8GB-HM-Q2-005 | The 4K baseline was blocked at 2.7 GiB estimated peak against 2.4 GiB safe memory. This run passed the displayed fit check at 2.5 GB estimated peak and 3 GB RAM available to the model. Context changed, but available safe memory also differed; this does **not** establish that context reduction alone caused the pass. |
| Evidence | `experiments/raw-results/8gb-windows-laptop/APP-8GB-HM-Q2-006/`; structured manifest; and `docs/testing/8GB-Laptop-Q2-2K-Code-Evidence.md`. |
| Interpretation / next safe action | This is an observed success for one session, not repeated stability, measured performance, or a complete response-quality evaluation. Repeat separately recorded sessions and capture timing and memory before making a student recommendation. |

### APP-8GB-HM-Q3-001 — exploratory packaged-app Q3_K_M 4K chat observation

| Field | Record |
| --- | --- |
| Date / scope | 2026-11-21. Unplanned exploratory observation after APP-8GB-HM-Q2-006; not a controlled repeat or Q2-versus-Q3 comparison. |
| Device | Lenovo 82QC; Windows 11 Home `10.0.22631` build `22631`; Intel Core i3-1215U; 8 GB RAM. |
| App package | Postflight package inventory lists the existing `GraniteEdgeAI.Asad.Context2048Experiment` `1.0.5.1` package. Package verification is linked to APP-8GB-HM-Q2-006; no new package verification, code change, build, or release is claimed. |
| Model | Granite 4.0 H Micro `Q3_K_M` GGUF; 1,555,472,032 bytes; SHA-256 `BB684177D546A2C6D3AEC9CC591FC8FA75D9C16A27E798CBB2400FE52D9B7E29`. Q3_K_M is a higher-precision quantisation of the same 3B H Micro base model as Q2_K, not a larger-parameter model. |
| Displayed compatibility result | Q3_K_M; llama.cpp; 4,096-token context; 4 GB free RAM now; 3.5 GB available to the model; 3 GB estimated current-setup peak RAM; **Yes — this model should run**. These are displayed estimates/rounded values, not measured inference memory. |
| Chat route and cache | Chat screen displayed llama.cpp and `F16 cache (selected)`. |
| Turn 1 | Prompt: `Explain what a computer processor does in two short sentences.`<br>Response: `A computer processor, or CPU, executes instructions from software programs to perform tasks, and manages data flow within the computer system.` |
| Turn 2 | Prompt: `What is one difference between a processor and memory?`<br>Response: `A processor, or CPU, is responsible for executing instructions and processing data, while memory (RAM) is used to temporarily store data that the processor needs for immediate access.` |
| Postflight snapshot | Captured after chat at `2026-11-21T18:49:24.9749042+00:00`: `llama-server` working set 2,079.5 MiB; free physical memory 1,936,504 KiB; C: free storage 12,989,845,504 bytes. This is not a clean-start baseline or a peak-memory measurement. |
| Measurements not captured | No formal response-time, generation-rate, model-load-duration, or peak-inference-memory measurement was captured. |
| Result | **One exploratory two-turn packaged-app chat observation completed successfully.** |
| Comparison limit | APP-8GB-HM-Q2-005 was blocked at 2.4 GB safe memory against a 2.7 GiB estimate. This run displayed 3.5 GB available to the model against a 3.0 GB estimate. The live available-memory conditions differed materially, so this does not disprove the Q2 4K block or establish a quantisation comparison. |
| Evidence | `experiments/raw-results/8gb-windows-laptop/APP-8GB-HM-Q3-001/` and `experiments/manifests/8gb-windows-laptop/runs/APP-8GB-HM-Q3-001.run-manifest.json`. |
| Next safe action | Do not download another model yet. Plan a controlled Q2/Q3 × 2K/4K matrix with fresh preflight before each run, fixed prompts, the same CPU/F16-cache route, and repeat sessions. |

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
