# 8 GB Windows Laptop Campaign Definition

**Document ID:** `PLAN-8GB-CAMPAIGN-001`
**Version:** 1.0
**Status:** Student-authored working baseline; academic and IBM technical review invited
**Campaign ID:** `8GB-WINDOWS-LAPTOP-v1`
**Owner:** Muhammad Asad Majeed
**Effective date:** 21 November 2026
**Repository:** `asad24-dev/IBM-Granite-Edge-AI-Optimisation-using-TurboQuant`

## Purpose

This campaign evaluates whether a bounded IBM Granite configuration can run locally, practically and repeatably through the Granite Windows application on one defined 8 GB laptop. It will produce evidence for a tested configuration, or an evidence-based explanation of why no tested configuration is practical.

The campaign treats model size, weight quantisation, context length, runtime settings and workload state as separate variables. It does not assume that a model file which downloads or passes inspection will fit in memory during chat.

## Relationship to the inherited project

This is a new campaign baseline for this fork. It does not replace Arian's project definition, MoSCoW baseline or requirements traceability matrix. Those documents remain historical reference material unless a future campaign change explicitly adopts part of them.

New campaign work uses identifiers beginning with `8GB-`. It must not reuse an inherited requirement, objective or work-package identifier in a way that implies the original project owner approved this campaign.

## Target and boundaries

| Item | Definition |
| --- | --- |
| Primary device | Lenovo 82QC, Windows 11 Home, Intel Core i3-1215U, 8 GB installed RAM (about 7.73 GiB usable) and Intel UHD Graphics. |
| Core route | Packaged Granite app, local CPU inference and a pinned IBM Granite GGUF model. |
| Primary user goal | A student can follow a tested local route without needing to choose untested technical settings. |
| Development machine | A separate machine may build or diagnose the app. Its results are not evidence of 8 GB support. |
| Evidence boundary | Findings apply only to the recorded device, package, model hash, runtime route, settings and workload state. |

The campaign does not claim universal 8 GB compatibility, a production-ready installer, support for every Granite model, or a one-click recommendation before the selected configuration has passed the stated checks.

## Research questions

| ID | Question |
| --- | --- |
| `8GB-RQ1` | Which selected Granite model and configuration candidates can complete model inspection, hardware assessment, configuration and bounded local chat on the target device? |
| `8GB-RQ2` | How do model variant, quantisation, context length and supported runtime or cache settings affect memory use, response time, stability and output quality on that device? |
| `8GB-RQ3` | Can TurboQuant or another compatible optimisation route provide a reproducible and useful trade-off against the best official GGUF baseline? |

## Work packages

| ID | Work package | Status at this baseline |
| --- | --- | --- |
| `8GB-WP1` | Repair and validate the packaged workflow needed for controlled testing. | Repair release and initial target-device validation recorded. |
| `8GB-WP2` | Define and run the baseline and bounded context/configuration experiments. | In progress. |
| `8GB-WP3` | Measure chat, repeatability, memory, speed and response quality for candidates that pass compatibility. | Not started. |
| `8GB-WP4` | Assess TurboQuant or another compatible optimisation route against the best official baseline. | Not started. |
| `8GB-WP5` | Produce tested student guidance and, if evidence supports it, an in-app recommended setup path. | Not started. |

## Evidence and decision rules

Every planned run must have a new run identifier and a manifest before it is performed. It must record the app package, model hash, device state, available memory, storage, runtime route, settings, prompt set and observed outcome. Raw screenshots, logs and reports belong in the matching raw-results folder. The readable test log summarises them; it does not replace them.

For a controlled comparison, change one planned variable at a time. Do not overwrite a failed or blocked run. Do not turn a model download, inspection pass, pre-run estimate or one reply into a stability claim.

| Gate | Evidence needed before proceeding |
| --- | --- |
| `8GB-G1` Compatibility | The packaged app completes model inspection and hardware assessment and allows the candidate to proceed under its safety policy. |
| `8GB-G2` Functional chat | The candidate reaches local chat and completes the fixed first prompt and a second-turn prompt. |
| `8GB-G3` Repeatability | The candidate completes at least three separately recorded sessions with no unplanned configuration change. |
| `8GB-G4` Comparison | The working candidate is compared with feasible alternatives using recorded memory, timing, stability and quality evidence. |
| `8GB-G5` Recommendation | Any student-facing guidance or app preset maps only to a configuration that passed the relevant evidence gates and states its limits. |

Final pass/fail thresholds for latency, response-quality scoring, usable context and minimum memory headroom will be proposed to IBM technical contacts and reviewed with the academic supervisor. Until then, measurements are recorded without claiming that they meet an external production threshold.

## Current baseline and next decision

The repaired `1.0.5.0` package completes model inspection and hardware assessment on the target laptop. In `APP-8GB-HM-Q2-005`, the offered Q2_K profile with a 4K context was blocked before model load: it estimated 2.7 GiB peak memory against 2.4 GiB safe memory, a 334 MB shortfall. This establishes a controlled compatibility result, not an inference failure.

The next intervention will trace where the 4K context is selected, define one smaller context candidate without weakening the safety policy, package the change and repeat the clean-workload test. The model, device and runtime route will remain fixed for that comparison.

## Evidence locations

- [Manifest index](../../experiments/manifests/8gb-windows-laptop/README.md)
- [Readable 8 GB test log](../testing/8GB-Laptop-Test-Log.md)
- [Raw-results index](../../experiments/raw-results/8gb-windows-laptop/README.md)
- [Campaign progress record](8GB-Laptop-Campaign-Progress.md)
- [Repair and validation record](../../release-evidence/Repair-Validation-1.0.5.0.md)

## Change control

Any material scope change, new runtime route, new model family, new hardware class, new user study or student-facing preset must be recorded before its result is treated as campaign evidence. The campaign definition is updated by versioned change, not silently rewritten after an experiment.
