# Install Granite Edge AI 1.0.5.0

Use Windows 11 x64. This is a self-signed research/test package, not a production installer. Do not bypass Windows or organisational security controls.

1. Download `Granite-Edge-AI-Setup-1.0.5.0.zip` from the [fork's `v1.0.5.0` GitHub Release](https://github.com/asad24-dev/IBM-Granite-Edge-AI-Optimisation-using-TurboQuant/releases/tag/v1.0.5.0). Verify the ZIP SHA-256 against the independently published [package handover](../release-evidence/Package-Handover.md) before extracting it to a regular local folder.

The expected ZIP SHA-256 is `96FEAEACE319CD6ECCA8003B4DBC2100F9E889DDE6EB13ADF91888148F18F982`. Check it with `Get-FileHash -Algorithm SHA256 -LiteralPath 'C:\path\to\Granite-Edge-AI-Setup-1.0.5.0.zip'`. Stop if it differs.
2. In ordinary **Windows PowerShell**, replace the example path with the extracted setup script's path. Check its SHA-256 against the value in the block before removing its download mark and running the bundle check:

```powershell
$graniteSetup = 'C:\path\to\Granite-Edge-AI-Setup-1.0.5.0\Setup-Granite.ps1'
if ((Get-FileHash -LiteralPath $graniteSetup -Algorithm SHA256).Hash -ne '6573E6E13873877BA3903998B620145CC94D50E1CE0ADC0B8A986129099DD565') { throw 'Setup script checksum mismatch. Stop.' }
Unblock-File -LiteralPath $graniteSetup
powershell.exe -NoProfile -ExecutionPolicy RemoteSigned -File $graniteSetup -Step Check
```

Continue only if the check reports that bundle hashes, certificate and Microsoft dependency signatures passed.

3. Close Granite if it is open. Open **Windows PowerShell as Administrator**, then install:

```powershell
$graniteSetup = 'C:\path\to\Granite-Edge-AI-Setup-1.0.5.0\Setup-Granite.ps1'
powershell.exe -NoProfile -ExecutionPolicy RemoteSigned -File $graniteSetup -Step Install
```

Type `INSTALL` only after reviewing the displayed certificate thumbprint. The script verifies the bundle, imports its public test certificate to `LocalMachine\TrustedPeople`, installs the required dependencies, and installs or updates Granite.

4. Close the administrator window. In ordinary Windows PowerShell, launch Granite:

```powershell
$graniteSetup = 'C:\path\to\Granite-Edge-AI-Setup-1.0.5.0\Setup-Granite.ps1'
powershell.exe -NoProfile -ExecutionPolicy RemoteSigned -File $graniteSetup -Step Launch
```

Import a supported model and use the hardware-fit result. A machine needs enough **currently free** RAM as well as sufficient installed RAM.

`RemoteSigned` applies to the child PowerShell process only. If an organisation's policy prevents the script from running, stop and contact its administrator. The guide inside the original release ZIP predates this clarification; updating this repository file does not change the published ZIP.
