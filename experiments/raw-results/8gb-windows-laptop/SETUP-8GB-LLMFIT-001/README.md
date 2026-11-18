# SETUP-8GB-LLMFIT-001 — LLM Fit provisioning evidence

This folder records the setup event that made the packaged app's hardware
compatibility route available on the Lenovo test laptop. It is not a model
inference result.

## Retained evidence

- `screenshots/pre-provisioning-warning.png` shows the app's earlier
  display-only hardware result when its optional LLM Fit evidence was absent.
- `llamacpp-probe-diagnostic-inconclusive.txt` is an earlier diagnostic capture.
  It contains no recorded probe output or exit code and is not used to explain
  the final result.

## Provisioning facts

The existing repository acquisition script obtained the pinned LLM Fit `1.1.9`
candidate without executing it. Its `llmfit.exe` SHA-256 was verified as
`db82bcb17f065b7ff7528ffe9904b2b0e1cce0c843fcd659b4ee1432a2e72e19`.

An administrator then placed only `llmfit.exe`, `LICENSE`, and `README.md` in
the machine-wide folder required by the installed app:

`C:\ProgramData\GraniteEdgeAI\HardwareInspection\llmfit\1.1.9\win-x64`

The installed app subsequently listed LLM Fit among its information sources.
The executable and the machine-wide copy are local test dependencies and are
not repository artifacts.
