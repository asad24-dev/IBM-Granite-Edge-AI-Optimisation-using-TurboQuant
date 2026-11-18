# APP-8GB-HM-Q2-004 — post-release compatibility check

This run records the first packaged-app compatibility result after the
v1.0.5.0 repair and LLM Fit provisioning. It occurred during setup activity,
so it is not the clean-baseline result for the campaign.

## Retained evidence

- `preflight-before-llmfit-provisioning.txt` is the direct PowerShell capture
  taken before LLM Fit was provisioned.
- `screenshots/Insufficient_RAM.png` shows the later compatibility rejection:
  the app estimated 2.7 GiB required, 669 MiB safe memory, and a 4K context.

The model did not reach configuration or chat in this run.
