# Install Granite Edge AI 1.0.5.0

Use Windows 11 x64. This is a self-signed research/test package, not a production installer. Do not bypass Windows or organisational security controls.

1. Download `Granite-Edge-AI-Setup-1.0.5.0.zip` from the [fork's `v1.0.5.0` GitHub Release](https://github.com/asad24-dev/IBM-Granite-Edge-AI-Optimisation-using-TurboQuant/releases/tag/v1.0.5.0) and extract it to a regular local folder.
2. In ordinary **Windows PowerShell**, verify the bundle:

```powershell
& 'C:\path\to\Granite-Edge-AI-Setup-1.0.5.0\Setup-Granite.ps1' -Step Check
```

3. Close Granite if it is open. Open **Windows PowerShell as Administrator**, then install:

```powershell
& 'C:\path\to\Granite-Edge-AI-Setup-1.0.5.0\Setup-Granite.ps1' -Step Install
```

Type `INSTALL` only after reviewing the displayed certificate thumbprint. The script verifies the bundle, imports its public test certificate to `LocalMachine\TrustedPeople`, installs the required dependencies, and installs or updates Granite.

4. Close the administrator window. In ordinary Windows PowerShell, launch Granite:

```powershell
& 'C:\path\to\Granite-Edge-AI-Setup-1.0.5.0\Setup-Granite.ps1' -Step Launch
```

Import a supported model and use the hardware-fit result. A machine needs enough **currently free** RAM as well as sufficient installed RAM.
