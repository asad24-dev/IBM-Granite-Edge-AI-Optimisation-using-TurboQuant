# Packaged Application Repair and Validation — 1.0.5.0

**Package:** `GraniteEdgeAI.Asad.Development` `1.0.5.0`
**Purpose:** Explain the engineering repair that followed the 8 GB laptop's
packaged-app failures, and distinguish that repair from the later 8 GB
configuration experiments.

## Scope

This is an engineering and release-validation record. It is not a performance
benchmark, and it does not claim that Granite has a working configuration on
every 8 GB laptop.

The separate 8 GB campaign records model selection, hardware fit, memory,
configuration and chat outcomes on the Lenovo 82QC device. Its historical
`1.0.4.0` records showed that the app stopped at model inspection with
`INSPECT-OP-001`, before hardware fit or chat.

## User-visible problem

The inherited package could fail in packaged Windows execution at several
stages:

- model inspection could end with the generic `INSPECT-OP-001` screen;
- hardware inspection could fail to produce an actionable compatibility result;
- the local GGUF chat worker could fail to start after a model was assessed.

The generic inspection code did not expose a root cause. Earlier 8 GB
diagnostics narrowed the failure to the packaged app and worker environment;
they did not prove one individual low-level cause for every `1.0.4.0` failure.

## Repair delivered

The fork contains five focused commits.

| Commit | Change | Engineering outcome |
| --- | --- | --- |
| `1089632c` | Model-inspection worker uses package-local storage. | The worker client receives an app-owned local storage root and reports bounded storage or access-control failures instead of collapsing them into a generic worker failure. |
| `5d498c74` | Hardware inspection produces reliable packaged reports. | Memory collection can fall back to Windows-visible memory, memory-source differences are reported without discarding primary Windows facts, and safe hardware diagnostic categories are mapped explicitly. |
| `22115a75` | GGUF chat worker uses package-local storage. | The local chat route receives a valid package-owned operation-storage location. |
| `aaa9f32c` | LLM Fit `1.1.9` candidate pinned. | The repository records the exact candidate version, archive hash, executable hash and allowed commands used for optional hardware evidence. |
| `1ae870b0` | Versioned 1.0.5.0 package prepared. | The package version, installer material, checksums and handover documentation were updated for the fork release. |

The first three changes address package-context working locations used by
helper processes. The repair does not bundle LLM Fit: it remains a separately
verified local dependency because its redistribution was not approved for the
research package.

## Automated checks changed with the repair

The repair commits changed tests around the new behaviour, including:

- model-inspection package-local storage resolution and worker-result mapping;
- worker-client recognition of bounded storage and access-control failures;
- Windows-memory fallback and conflict handling;
- hardware-inspection outcome mapping and contained native-process creation;
- local runtime-probe diagnostic mapping; and
- worker handshake coverage in the integration test project.

These tests support the code paths and diagnostic contracts. They do not by
themselves prove a complete user workflow on every device.

## Recorded validation

### Package integrity and development-laptop route

The `v1.0.5.0` handover records the signed MSIX identity, package checksum and
dependency inputs. The package notes record that a development-laptop update
and Granite 4.0 H Micro Q2_K CPU chat route were validated on 4 November 2026.
The handover further records continuation of an existing CPU conversation and
multiple responses in a new conversation.

This is an update-and-chat validation on an existing development machine. It is
not a clean-machine install or uninstall result.

### Target 8 GB laptop validation

On the Lenovo 82QC target laptop, the `1.0.5.0` setup check passed bundle
hashes, the test certificate check and Microsoft dependency signature checks.
The same verified local Granite 4.0 H Micro Q2_K GGUF then completed packaged
model inspection and hardware inspection. This no longer reproduced the
historical `INSPECT-OP-001` stopper.

The first hardware attempt identified that the optional LLM Fit evidence tool
was absent. The exact pinned `1.1.9` candidate was acquired and verified, then
the expected three files were provisioned locally. The app subsequently listed
LLM Fit among its hardware-information sources.

The repaired app then produced a genuine compatibility result. Under a fresh
restart and the recorded minimal workload, its default Q2_K 4K profile was
334 MB below the safe-memory threshold. This is a configuration-fit result,
not a regression of the repair and not a model-inference result.

## Evidence links

- [1.0.5.0 package handover](Package-Handover.md)
- [1.0.5.0 package notes](../build-version-1.0.5.0/PACKAGE-NOTES.md)
- [Historical inspection investigation](../docs/testing/8GB-Laptop-Inspection-Investigation.md)
- [8 GB readable test log](../docs/testing/8GB-Laptop-Test-Log.md)
- [8 GB structured campaign index](../experiments/manifests/8gb-windows-laptop/README.md)

## Limits and next work

- The repair record does not replace clean-machine install or uninstall testing.
- The LLM Fit provisioning path is currently a controlled local test setup, not
  a self-contained production installer feature.
- The 8 GB laptop has not reached packaged-app configuration or chat for the
  default 4K Q2_K profile.

The next engineering step is to inspect and evaluate a bounded lower-context
configuration profile while retaining the app's memory-safety policy. Any new
profile must pass the same fresh-start compatibility checks before it is used
for chat, quality or stability experiments.
