# 8 GB Windows Laptop — Raw Evidence

This folder preserves direct, unedited evidence from the Lenovo 82QC campaign
(`8GB-WINDOWS-LAPTOP-v1`). It is separate from inherited llama.cpp, OpenVINO
and TurboQuant campaigns. Nothing here proves support for a different laptop.

## Per-run layout

Create one folder for each completed or blocked run, named with its immutable
run ID. For example:

```text
APP-8GB-HM-Q2-004/
├── README.md                 # index of evidence captured for this run
├── preflight.txt             # direct PowerShell preflight output
├── installer-check.txt       # release verification output, when applicable
├── screenshots/              # captured application states and errors
├── logs/                     # exported app/runtime logs, if available
└── measurements/             # direct memory/timing exports, if captured
```

Do not edit captured raw files after recording them. The matching structured
run manifest belongs in
`experiments/manifests/8gb-windows-laptop/runs/`, and the human-readable
interpretation belongs in `docs/testing/8GB-Laptop-Test-Log.md`.

## Privacy and size rules

- Do not add model weights, downloaded installers, secrets or private account
  data to Git.
- Redact personal paths, account names and unrelated window content from any
  screenshot that will be committed.
- Keep an evidence index in the run folder's `README.md`, including the local
  capture date, run ID and SHA-256 checksums for irreplaceable exported files.
- A failed or blocked run is retained with the same care as a successful run.

## Future derived results

Calculated comparison tables and charts must be generated from these raw files
and stored separately under an 8 GB campaign path in
`experiments/processed-results/`. They must identify their source run IDs and
must never overwrite the raw capture.
