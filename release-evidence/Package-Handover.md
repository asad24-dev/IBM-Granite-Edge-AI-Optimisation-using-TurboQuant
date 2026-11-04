# Package handover

This is a release record, not an installer receipt. The existing installed-app tests are linked in the [release index](README.md). They do not prove a clean installation.

| Required detail | Current position |
| --- | --- |
| Download location and package filename | GitHub Release `v1.0.5.0`, asset `Granite-Edge-AI-Setup-1.0.5.0.zip`; SHA-256 `96FEAEACE319CD6ECCA8003B4DBC2100F9E889DDE6EB13ADF91888148F18F982` (601,568,384 bytes). `Checksums.csv` inside the ZIP records each payload hash. |
| Package identity and source provenance | `GraniteEdgeAI.Asad.Development`, `1.0.5.0`, Windows x64. The signed MSIX SHA-256 is `609E5BE247399E9C0CEC120AF9C10D0F1016B76D21D9863656A0BAAD6A6BB946`; its main application and fix-bearing assemblies were compared with the build output. |
| Source commit and package recipe | `aaa9f32c3202b4670880a028ddfd48b1619414de` plus the version-manifest change recorded by this release commit. Verified worker stages were copied from the known-working local package, with their manifests checked before packaging. |
| Tool versions and runtime inputs | MSIX signed with public certificate thumbprint `D6B842420C0F0FBC36758CAC7F22B52D39CAD911`; Windows App Runtime x64 SHA-256 `A71C600B2FB719C8D70120151965891F360857C1FF3D9D5C47ED92622A60F665`; public certificate only is bundled. No signing key is published. |
| Local install and chat route | Recorded on 4 November 2026: installed update, restored and continued an existing Granite 4.0 H Micro Q2_K llama.cpp CPU conversation, and received multiple responses in a new conversation. |
| Clean install, Start launch and both inspection routes | Not recorded as a clean-machine test. Use a separate suitable environment, not the only working installation. |
| Uninstall and retained data | Not verified. Back up required data first. |
| Release tag and final report bundle | `v1.0.5.0` after the asset is uploaded to the fork release. |

For each completed row, record the date, exact package and an evidence link. Do not replace “not recorded” with “passed” just because compilation succeeds.

Documentation, test evidence and diagram files also need inclusion in the final reviewed commit. Check `git status --short`; a local file is not part of a fresh checkout until committed. Review unrelated files separately rather than staging everything.
