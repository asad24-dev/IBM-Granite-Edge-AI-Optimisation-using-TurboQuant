# 8 GB Student-Laptop MoSCoW — Draft v0.2

**Document ID:** PLAN-8GB-MOSCOW-001  
**Status:** Draft for John / replacement-mentor review  
**Revision basis:** Asad's first-pass MoSCoW and John McNamara's feedback of
20 October 2026.  
**Related criteria:** [8 GB Laptop Campaign Success Criteria](8GB-Laptop-Success-Criteria-Draft.md)

## Purpose and boundary

This is a proposed scoped addendum for the 8 GB student-laptop direction. It
does not replace the existing repository MoSCoW baseline or its RTM. It becomes
an active requirement source only after an owner approves it and the affected
requirement IDs, traceability and change control are agreed.

The primary intended outcome is that students can run an agreed IBM Granite
configuration locally on typical laptops with around 8 GB RAM, with minimal
setup and clear, truthful guidance about what will and will not work.

## Must have

| ID | Requirement | Success criteria / evidence |
| --- | --- | --- |
| M-8GB-01 | Build on the existing application so an agreed Granite model and runtime route can be run locally on the target 8 GB laptop. | `SC-8GB-01` to `SC-8GB-05`; one exact successful end-to-end configuration recorded. |
| M-8GB-02 | Show clear guidance about whether a selected tested configuration is suitable for the target 8 GB device, including limitations and recovery from unsupported or failed choices. | Inspection/hardware/configuration results and usability evidence; no unsupported compatibility claim. |
| M-8GB-03 | Run controlled experiments of agreed model and quantisation/configuration options, recording memory footprint, speed, response quality, stability, actual route/device and failures. | Reproducible manifests, logs, fixed prompts and comparison table. |
| M-8GB-04 | Make only the app changes needed to support the agreed proven configuration; document use, limitations and any fallback. | Reviewed implementation, tests, documentation and traceable evidence. |

## Should have

| ID | Requirement | Success criteria / evidence |
| --- | --- | --- |
| S-8GB-01 | Compare multiple agreed model/quantisation configurations and multiple bounded conversation/context lengths to assess memory growth and practical limits. | Controlled comparison with variables, measurements and results. |
| S-8GB-02 | Suggest tested alternatives when a configuration is unsuitable for the target device. | Suggestions map to recorded tested evidence and display their limitations. |
| S-8GB-03 | Provide practical recommendations that minimise student setup and confusion for the tested route. | Short task walkthrough and usability observations. |

## Could have

| ID | Requirement | Success criteria / evidence |
| --- | --- | --- |
| C-8GB-01 | Add a one-click **8 GB recommended setup** that selects a tested model and settings automatically. | It selects only a configuration that passed `SC-8GB-01` to `SC-8GB-06`, needs no manual setting selection, and explains its limits. |

## Won't have yet

| ID | Exclusion / boundary | Reason |
| --- | --- | --- |
| W-8GB-01 | Universal 8 GB compatibility claims across all Granite models, laptops, backends or contexts. | Evidence can support only exact tested configurations. |
| W-8GB-02 | A student preset that guesses an untested configuration or hides limitations. | A recommendation must be grounded in completed evidence. |


## Measures to agree before benchmarking

The following values must be agreed with the owner/mentor before they are used
as pass/fail thresholds: acceptable response latency, minimum free-memory
reserve, test context lengths, response-quality rubric/scoring threshold,
number of repetitions, stability duration and the definition of a usable
student workflow. Until then, record measurements and observations without
claiming that a configuration passes the final target.