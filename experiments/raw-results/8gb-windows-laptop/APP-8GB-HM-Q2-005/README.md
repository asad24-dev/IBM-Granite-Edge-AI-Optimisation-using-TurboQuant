# APP-8GB-HM-Q2-005 — fresh-restart compatibility baseline

This folder contains the raw evidence for the clean, minimal-workload
compatibility test of the packaged Q2_K route.

## Retained evidence

- `preflight.txt` records the fresh restart, app package identity, memory,
  storage, processor, and largest running processes before Granite launched.
- `screenshots/01-compatibility-insufficient-safe-ram.png` shows the final
  app assessment.

The screenshot reports a 2.7 GiB estimate for the Q2_K model at a 4K context,
2.4 GiB safe memory after the 512 MB reserve, and a 334 MB shortfall. The app
therefore did not offer configuration or chat.
