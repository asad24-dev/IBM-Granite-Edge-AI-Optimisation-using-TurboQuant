# APP-8GB-HM-Q3-001 raw evidence

This folder preserves direct artefacts for an exploratory Q3_K_M 4K packaged
chat observation on the 8 GB Lenovo on 2026-11-21. It is separate from the
Q2/2K run and is not a controlled repeat or Q2-versus-Q3 comparison.

| File | SHA-256 | Provenance and direct support |
| --- | --- | --- |
| `postflight-after-q3-4k-chat.txt` | `87AB9613FA94C706E7E40D074E8ED9EA80FD8B5265397452DD8DF79DDB7B6F97` | Original command output captured at `2026-11-21T18:49:24.9749042+00:00`, after the two Q3 chat turns. It records the installed package inventory, OS/device facts, 1,936,504 KiB free physical memory, 12,989,845,504 bytes free C: storage, and a 2,079.5 MiB `llama-server` working set. It is **postflight**, not a clean-start baseline or a peak-memory measurement. |
| `screenshots/01-q3-4k-compatibility.png` | `F57D3A4F771A870236FEBCAB6721472FC41EE9E38B03893A28249D7502B071F4` | Displayed Q3_K_M compatibility result, 4,096-token context, displayed memory estimates, and pass decision. |
| `screenshots/02-q3-two-turn-chat.png` | `9148CBEAB5B770BE0C311042E93633C390785B9F0CF0F224232D956E1BDE138F` | The two Q3 chat prompts and complete responses, plus displayed llama.cpp route and selected F16 cache. |

No formal timing, generation-rate, model-load-duration, or peak-inference-memory
capture was supplied for this observation. The matching manifest links to the
already-recorded package evidence for `APP-8GB-HM-Q2-006`; no new package,
code change, build, or release is claimed here.
