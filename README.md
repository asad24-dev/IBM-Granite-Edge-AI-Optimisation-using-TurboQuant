# Granite: local AI on Windows

This is Muhammad Asad Majeed's research fork of the Granite Windows application. It preserves the inherited project as historical context while continuing the application and running a separate, evidence-led campaign on a defined 8 GB Windows laptop.

## Current fork status

- The current test package is [Granite Edge AI 1.0.5.0](build-version-1.0.5.0/README.md). It includes repairs to packaged model inspection, hardware reporting and local GGUF chat-worker storage.
- The [8 GB Windows Laptop Campaign](docs/planning/8GB-Windows-Laptop-Campaign-Definition-v1.md) is evaluating whether a bounded Granite configuration can complete the full local workflow on a Lenovo 82QC with 8 GB installed RAM.
- The first repaired-package baseline reached compatibility assessment on that device. The default Q2_K profile at a 4K context was safely blocked before model load because its 2.7 GiB estimate exceeded the 2.4 GiB safe-memory budget. This is not a chat or inference result.

The campaign makes device-specific findings only. A successful download, inspection result or single prompt is not treated as proof that a model is practical on every 8 GB Windows laptop.

## Install the current test package

For a released-package installation, download [release `v1.0.5.0`](https://github.com/asad24-dev/IBM-Granite-Edge-AI-Optimisation-using-TurboQuant/releases/tag/v1.0.5.0) and follow the [short installation guide](build-version-1.0.5.0/START-HERE.md). No Git, Visual Studio or source build is required to run that package.

This is a self-signed research/test package for Windows 11 x64. Follow the verification steps in the guide and do not bypass Windows or organisational security controls. The package is not a claim that every model workflow will fit on every computer.

The 1.0.5.0 guide invokes an unsigned setup script directly, which may be blocked by PowerShell execution policy. The guide is being corrected; follow your device's security policy and do not assume that the package includes every optional hardware tool.

## 8 GB campaign

The campaign definition records its scope, identifiers, evidence rules and decision gates. Start with:

- [Campaign definition](docs/planning/8GB-Windows-Laptop-Campaign-Definition-v1.md)
- [Experiment manifest index](experiments/manifests/8gb-windows-laptop/README.md)
- [Readable test log](docs/testing/8GB-Laptop-Test-Log.md)
- [1.0.5.0 repair and validation record](release-evidence/Repair-Validation-1.0.5.0.md)

Each new experiment has a new run identifier, manifest and raw-results folder. Earlier evidence is retained, including blocked and failed conditions. The next intervention will assess one lower-context candidate while holding the target device, pinned Q2_K model and runtime route constant.

## What the application does

Granite is a WinUI 3 research application for selected IBM Granite models on Windows 11 Intel hardware. It supports model import or selected downloads, model inspection, hardware-fit assessment, configuration, local chat and evidence-led experimental routes. Supported choices depend on the exact model, runtime and device. Being selectable does not mean a configuration will run.

## Development and evidence

- [Set up from source](docs/manuals/Fresh-Computer-Setup.md)
- [Build and installation guide](docs/manuals/Build-and-Installation.md)
- [Developer manual](docs/manuals/Developer-Manual.md)
- [Test index](tests/README.md)
- [Release handover](release-evidence/README.md)

Application verification, experimental evidence and release evidence are kept separate. A standalone runtime experiment does not prove that the packaged application completed the same workflow.

## Historical baseline

The upstream project definition, MoSCoW requirements, traceability matrix, demonstration and report describe Arian's original project. They are retained as historical material and are not a requirement source for the 8 GB campaign unless this fork explicitly links them.

- [Original project definition](docs/planning/Project-Definition-v1.1.md)
- [Original requirements baseline](docs/requirements/MoSCoW-Requirements-v1.2.md)
- [Original traceability material](docs/traceability/README.md)
- [Historical 1.0.4.0 package](build-version-1.0.4.0/README.md)
- [Inherited project report](Project%20Report.pdf)
- [Inherited application demonstration](https://liveuclac-my.sharepoint.com/:v:/g/personal/ucab280_ucl_ac_uk/IQCYcBz0px0RTIhDx_pQuhWcARA23Skbx3Afw71i769hZ1Y?e=DzOiWz)

The inherited report and demonstration are not deliverables of this campaign.

## Repository layout

- `IBM Granite with TurboQuant (Intel)`: WinUI application source.
- `shared`, `infrastructure`, `runtime`, `workers`: contracts, services and runtime integration.
- `tests`: automated tests and fixtures.
- `experiments`: manifests, protocols, raw evidence and processed results.
- `docs`: application guidance, architecture and campaign records.
- `release-evidence`: package identity, validation and handover records.
- `research`: source records and research notes. Treat inherited material as historical unless it is explicitly adopted by the 8 GB campaign.

## Responsible use

This is a research prototype, not a clinical system or a replacement for professional judgement. Do not use real patient or pupil data. Review generated answers before relying on them. Do not commit model weights, private prompts, credentials, signing keys or personally identifying device data.
