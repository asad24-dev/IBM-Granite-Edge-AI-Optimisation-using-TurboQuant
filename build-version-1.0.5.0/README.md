# Granite fork build 1.0.5.0 for Windows

This is the signed Windows x64 test package produced from the fork's five release commits. Download `Granite-Edge-AI-Setup-1.0.5.0.zip` from the [fork's `v1.0.5.0` GitHub Release](https://github.com/asad24-dev/IBM-Granite-Edge-AI-Optimisation-using-TurboQuant/releases/tag/v1.0.5.0), extract it, then follow `START-HERE.md` inside the ZIP.

The ZIP contains the signed MSIX, public certificate, Microsoft Windows App Runtime dependency, .NET 8 runtime installer, setup script, and checksums. It contains no model weights and no private signing key.

## Recorded validation

On 4 November 2026, the package was signed with the public certificate thumbprint `D6B842420C0F0FBC36758CAC7F22B52D39CAD911` and installed as an update on the development laptop. A Granite 4.0 H Micro Q2_K GGUF model loaded using llama.cpp on CPU. An existing conversation was restored and continued, and a new conversation returned multiple responses.

This confirms the recorded local update and chat route only. It does not replace clean-machine testing, demonstrate every model format, or guarantee a fit on a laptop with insufficient free RAM. Follow Windows and organisational security policies; do not bypass them.

## Source provenance

The package was built from commit `aaa9f32c3202b4670880a028ddfd48b1619414de` plus the `1.0.5.0` manifest version update. It includes the model-inspection, hardware-inspection, chat-storage, and pinned local-tool fixes recorded in the preceding commits. See [the release record](../release-evidence/Package-Handover.md) for hashes and remaining validation.
