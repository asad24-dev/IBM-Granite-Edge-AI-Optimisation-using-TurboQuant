# 8 GB Windows laptop campaign

This folder contains the new physical-device campaign for the Lenovo 82QC with
8 GB installed RAM. It is separate from the frozen route campaigns and does
not reuse their test or run identifiers.

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

`APP-8GB-HM-Q2-001` reached model inspection after the in-app download flow but
failed before hardware fit, configuration, model load, or chat. The failure is
preserved as evidence and must not be overwritten by a retry.
