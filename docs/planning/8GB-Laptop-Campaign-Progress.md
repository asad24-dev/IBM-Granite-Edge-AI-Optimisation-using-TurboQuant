# 8 GB Laptop Campaign — Progress Record

**Document ID:** PLAN-8GB-PROGRESS-001  
**Status:** Working record — John feedback received; replacement-mentor and project-owner review pending  
**Campaign:** `8GB-WINDOWS-LAPTOP-v1`  
**Working repository:** `asad24-dev/IBM-Granite-Edge-AI-Optimisation-using-TurboQuant` (fork)  
**Upstream reference:** `arian20020/IBM-Granite-Edge-AI-Optimisation-using-TurboQuant`  
**Purpose:** A concise, dated record for weekly work and Pulse updates. It is not a replacement for the repository's approved project definition, MoSCoW baseline, RTM, or final-results evidence.

## How to maintain this record

- Add one entry for each reporting week, including the 7th and 21st Pulse submissions when applicable.  
- Link detailed experimental evidence rather than copying it here.  
- State blockers and unknowns plainly; do not turn setup or an attempted run into a performance/support claim.  
- Record a mentor or project-owner decision with date and name when one is received.

## Current project description — draft for review

**Proposed title:** Evidence-led feasibility of IBM Granite Edge AI on an 8 GB Windows laptop.

**Working aim:** Establish, with reproducible device-specific evidence, whether an appropriately bounded Granite configuration can complete the app workflow and a minimal local prompt-and-response run on an 8 GB Windows laptop. Use the evidence to inform, rather than prematurely implement, a future simplified student-facing configuration path.

## Weekly updates

### Week of 20–26 October 2026 — setup and first controlled experiment

**Completed**

- Cloned the repository at commit `489a6f0d393c8a7c5e800a367c4d889099f40bcd`; long-path support was enabled and the checkout was verified.  
- Verified and installed the provided Granite setup package. The setup bundle, Microsoft dependency signatures and packaged application material passed the available integrity checks.  
- Launched Granite 1.0.4.0 on the Lenovo 82QC test device (Windows 11 Home, Intel i3-1215U, 7.73 GiB installed RAM, Intel UHD Graphics).  
- Created a separate 8 GB campaign environment manifest, launch baseline and human-readable test log. These do not alter or relabel the repository's prior frozen campaigns.  
- Downloaded the recommended Granite 4.0 H Micro `Q2_K` GGUF through the app. Its stored size and SHA-256 match the pinned expected artefact.  
- Performed a controlled retry using the same verified GGUF through **Use a local model**. Both routes failed at model inspection with `INSPECT-OP-001` before hardware fit, configuration or chat.
- Created a personal GitHub fork for the 8 GB campaign; the local clone now uses the fork as `origin` and Arian's repository as `upstream`.

**Next actions**

1. Investigate the packaged-app inspection failure with maintainable diagnostic evidence before trying other models, runtimes or model settings.  
2. Once inspection is unblocked, perform the first bounded end-to-end run using a documented small context and fixed prompt; record memory, timing, output and stability.  
3. Update this record and the Pulse submission on the next reporting date.

**Risks / decisions needed**

- The package version under test may not map exactly to the checked-out source diagnostic identifiers; the cause of `INSPECT-OP-001` is not yet established.  
- Current available disk space and memory headroom are constrained; every run needs a preflight measurement.  
- Markus has advised that he can no longer mentor the project; a replacement mentor is pending. Ask John to confirm the new mentor and the review route.

## Pulse-ready short update — 20 October 2026

> Set up and launched the Granite packaged app on the 8 GB Lenovo Windows test laptop, and added reproducible environment/run records to the project. The first Granite 4.0 H Micro Q2\_K experiment exposed a repeatable model inspection blocker (`INSPECT-OP-001`) before hardware fit or inference. The model and packaged inspection-worker integrity checks passed; next work is diagnostic investigation and mentor-confirmed 8 GB success criteria.

## Future update template

### Week of YYYY-MM-DD to YYYY-MM-DD

**Completed**

**Evidence**

**Result / interpretation**

**Next actions and decisions needed**  
