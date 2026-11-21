# 8 GB Windows laptop campaign

This folder contains the new physical-device campaign for the Lenovo 82QC with
8 GB installed RAM. It is separate from the frozen route campaigns and does
not reuse their test or run identifiers.

The campaign's scope, identifiers, evidence rules and decision gates are in
the [8 GB Windows Laptop Campaign Definition](../../../docs/planning/8GB-Windows-Laptop-Campaign-Definition-v1.md).

The human-readable running record is
[`docs/testing/8GB-Laptop-Test-Log.md`](../../../docs/testing/8GB-Laptop-Test-Log.md).
Structured manifests record the environment and each observed execution. Raw
evidence must be retained without overwriting earlier failed, blocked, or
inconclusive attempts.

## Identifiers

| Identifier | Meaning |
| --- | --- |
| `ENV-8GB-LENOVO-82QC-001` | Initial captured target-device environment |
| `APP-8GB-BASE-001` | Packaged-app launch baseline; no model loaded |
| `APP-8GB-HM-Q2-001` | H Micro Q2_K inspection attempt; failed before hardware fit |
| `APP-8GB-HM-Q2-002` | Local-file inspection retry; reproduced the same generic inspection failure |
| `APP-8GB-HM-Q2-003` | Short-path hard-link inspection test; reproduced the same generic inspection failure |
| `SETUP-8GB-LLMFIT-001` | Provisioning of the exact pinned LLM Fit 1.1.9 package required by the packaged app's compatibility route |
| `APP-8GB-HM-Q2-004` | Post-release compatibility check; inspection and hardware assessment completed, but the active setup had insufficient safe RAM |
| `APP-8GB-HM-Q2-005` | Fresh-restart minimal-workload compatibility baseline; the default Q2_K 4K profile remained 334 MB below the safe-memory threshold |

`APP-8GB-HM-Q2-001` reached model inspection after the in-app download flow but
failed before hardware fit, configuration, model load, or chat. The failure is
preserved as evidence and must not be overwritten by a retry.

The later v1.0.5.0 records are separate from those historical inspection
failures. They show that model inspection and hardware assessment now complete
in the packaged app. They do not establish a working chat configuration.
