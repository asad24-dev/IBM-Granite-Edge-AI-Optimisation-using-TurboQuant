# APP-8GB-HM-Q2-006 raw evidence

This folder preserves the direct artefacts supplied for the first 2K-context
packaged-app run on the 8 GB Lenovo. Run date: 2026-11-21. Files are retained
unchanged; the structured interpretation is in the matching run manifest and
readable test log.

| File | SHA-256 | What it directly supports |
| --- | --- | --- |
| `preflight.txt` | `65E87ED3D7C8D79261F1D1CB3E6171BCD3A61FF107C455C0FF100E55A8D56DEA` | The preflight timestamp, installed package inventory, Windows/build, CPU, physical memory, free physical memory, storage, and listed processes. |
| `screenshots/01-compatibility-2k-fit.png` | `379AA95CB98BF67A59BE2797800A437D4D9AC08DC47A9F6360679705343E6BB9` | The displayed Q2_K compatibility decision, 2,048-token context, displayed memory estimates, llama.cpp route, and 8 GB installed memory. |
| `screenshots/02-two-turn-chat.png` | `A2C337DAC18F27A69977B61903800C52918EBE93B51F5A199BA7A5C82D84AE2F` | The two prompts and responses, plus the displayed `llama.cpp · CPU` route and selected F16 cache. |

The package SHA-256 and signing outcome are reported by the run operator in
the matching manifest and test log; their command output was not supplied as
a raw file. No timing, generation-rate, or peak-inference-memory capture was
supplied for this run.
