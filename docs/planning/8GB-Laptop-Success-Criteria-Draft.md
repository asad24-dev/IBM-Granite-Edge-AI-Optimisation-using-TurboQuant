# Draft: 8 GB Laptop Campaign Success Criteria

**Document ID:** PLAN-8GB-SC-001  
**Status:** Draft reflecting John McNamara's feedback of 20 October 2026 — mentor/project-owner review still required before it changes the MoSCoW baseline  
**Related campaign:** `8GB-WINDOWS-LAPTOP-v1`

## Purpose

This draft translates the 8 GB student-laptop direction into measurable outcomes. John confirmed that the primary goal is practical Granite use on typical student laptops (around 8 GB RAM), with experimentation, benchmarking, quantisation options and clear guidance. It must still be reviewed before it is added to the project MoSCoW, requirements traceability matrix, scope baseline or implementation plan.

## Proposed success criteria

| ID | Priority | Proposed criterion | Evidence needed |
| :---- | :---- | :---- | :---- |
| SC-8GB-01 | Must | The exact laptop, Windows build, application version, model file hash, runtime route and settings are recorded for every claimed run. | Environment/run manifests and test log. |
| SC-8GB-02 | Must | A supported selected model completes inspection, hardware assessment and configuration without an unclassified failure. | Screenshots, app result and run manifest. |
| SC-8GB-03 | Must | At least one pinned configuration completes a fixed short prompt locally and produces a coherent response against a documented response-quality rubric. | Prompt, response, rubric result, timing, actual runtime/backend/device and run log. |
| SC-8GB-04 | Must | The successful run records memory footprint/headroom, response quality, stability and usability observations, including any failure or fallback. | Pre/post/peak measurements or clearly stated measurement limitation; rubric and usability notes. |
| SC-8GB-05 | Must | No claim of 8 GB support is made beyond the exact tested model, context, runtime and device configuration. | Review of update, evidence and final wording. |
| SC-8GB-06 | Should | A repeat of the successful configuration has the same completed workflow and no uncontrolled configuration changes. | Second run record and comparison. |
| SC-8GB-07 | Should | The app gives a non-specialist comprehensible guidance/recovery path for the tested configuration. | Usability notes and screenshot(s). |
| SC-8GB-08 | Could | A reviewed one-click student-facing 8 GB preset automatically selects only a proven configuration, needs no manual settings choice, and clearly explains limitations. | Approved requirement/design, tests and usability evidence. |

## Decisions requested from John / mentor

1. What is the minimum acceptable definition of a successful 8 GB run: short-prompt feasibility only, or a specified context/latency/stability target?  
2. Is the student-facing preset in scope for research/design only, or for implementation in this project?  
3. Who is the replacement mentor after Markus's departure?

## Review record

| Reviewer | Decision | Date | Notes / affected requirement IDs |
| :---- | :---- | :---- | :---- |
| John / mentor | Pending | — | — |
