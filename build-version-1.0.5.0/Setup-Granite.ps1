param([ValidateSet('Check', 'Install', 'Launch')][string]$Step = 'Check')

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$packageName = 'GraniteEdgeAI.Asad.Development'
$packageVersion = [version]'1.0.5.0'
$publisher = 'CN=Granite Edge AI Development'
$certificateThumbprint = 'D6B842420C0F0FBC36758CAC7F22B52D39CAD911'

function Assert-Hash([string]$Path, [string]$Hash) {
    if ((Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash -ne $Hash) {
        throw "Checksum mismatch: $Path. Stop; do not use this file."
    }
}

function Test-Bundle {
    Assert-Hash (Join-Path $PSScriptRoot 'Granite-Edge-AI-1.0.5.0-x64.msix') '609E5BE247399E9C0CEC120AF9C10D0F1016B76D21D9863656A0BAAD6A6BB946'
    Assert-Hash (Join-Path $PSScriptRoot 'Granite-Edge-AI-Development.cer') '45AF093205BAFEC1324666A37A2AC29F8E32A18CBC7560C7B97FA67AA97E705F'
    Assert-Hash (Join-Path $PSScriptRoot 'Microsoft.WindowsAppRuntime.2.2-x64.msix') 'A71C600B2FB719C8D70120151965891F360857C1FF3D9D5C47ED92622A60F665'

    $netInstaller = Join-Path $PSScriptRoot 'dotnet-runtime-8.0.31-win-x64.exe'
    if ((Get-FileHash -LiteralPath $netInstaller -Algorithm SHA512).Hash -ne '2DC2F346A4BCB53AB15AB6AB84348C02EB31B0A3D4E8E06AAC73B4C89F1B001AF01943ED4C5735FA5585F1DD731A942D2034577DFE937F4454367FF2D6EDF929') {
        throw '.NET installer checksum mismatch. Stop; do not use this file.'
    }

    foreach ($signedFile in @($netInstaller, (Join-Path $PSScriptRoot 'Microsoft.WindowsAppRuntime.2.2-x64.msix'))) {
        if ((Get-AuthenticodeSignature -FilePath $signedFile).Status -ne 'Valid') {
            throw "Microsoft signature could not be validated: $signedFile"
        }
    }

    $certificate = [Security.Cryptography.X509Certificates.X509Certificate2]::new((Join-Path $PSScriptRoot 'Granite-Edge-AI-Development.cer'))
    if ($certificate.Thumbprint -ne $certificateThumbprint -or $certificate.NotAfter -le (Get-Date)) {
        throw 'The supplied Granite test certificate is unexpected or expired.'
    }
}

try {
    if ($Step -eq 'Check') {
        Test-Bundle
        Write-Host 'Bundle hashes, certificate, and Microsoft dependency signatures passed. Nothing installed.'
        return
    }

    if ($Step -eq 'Install') {
        if ($PSVersionTable.PSEdition -ne 'Desktop') { throw 'Use Windows PowerShell (powershell.exe), not PowerShell 7, for installation.' }
        if ((Get-CimInstance Win32_OperatingSystem).BuildNumber -lt 22000) { throw 'Use Windows 11 for this test.' }
        if ((Get-CimInstance Win32_Processor | Select-Object -First 1).Architecture -ne 9) { throw 'An x64 laptop is required.' }

        $existing = Get-AppxPackage -Name $packageName
        if ($existing) {
            if ($existing.Status -ne 'Ok' -or $existing.Publisher -ne $publisher -or $existing.IsDevelopmentMode) {
                throw 'The existing installation is not a supported signed Granite package. Nothing was changed.'
            }
            if ([version]$existing.Version -ge $packageVersion) {
                Write-Host 'Granite 1.0.5.0 or a newer version is already installed. Continue to Launch.'
                return
            }
            Write-Host "Granite $($existing.Version) will be updated to $packageVersion. Close Granite before continuing."
        }

        $principal = [Security.Principal.WindowsPrincipal]::new([Security.Principal.WindowsIdentity]::GetCurrent())
        if (-not $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) { throw 'Run Windows PowerShell as administrator using your own account.' }

        Test-Bundle
        Write-Host 'This adds the test certificate to LocalMachine\TrustedPeople and installs dependencies and Granite.'
        Write-Host "Certificate thumbprint: $certificateThumbprint"
        if ((Read-Host 'Type INSTALL to approve these changes on this test laptop') -cne 'INSTALL') { throw 'Installation cancelled.' }

        $dotnetExe = Join-Path $env:ProgramFiles 'dotnet\dotnet.exe'
        $hasRuntime = $false
        if (Test-Path -LiteralPath $dotnetExe) { $hasRuntime = [bool]((& $dotnetExe --list-runtimes) -match '^Microsoft.NETCore.App 8\.') }
        if (-not $hasRuntime) {
            $process = Start-Process -FilePath (Join-Path $PSScriptRoot 'dotnet-runtime-8.0.31-win-x64.exe') -ArgumentList '/install /passive /norestart' -WindowStyle Hidden -Wait -PassThru
            if ($process.ExitCode -eq 3010) { throw '.NET installed and requested a restart. Restart Windows, then repeat Install.' }
            if ($process.ExitCode -ne 0) { throw ".NET installer failed: $($process.ExitCode)" }
        }

        Import-Certificate -FilePath (Join-Path $PSScriptRoot 'Granite-Edge-AI-Development.cer') -CertStoreLocation Cert:\LocalMachine\TrustedPeople | Out-Null
        Add-AppxPackage -Path (Join-Path $PSScriptRoot 'Granite-Edge-AI-1.0.5.0-x64.msix') -DependencyPath (Join-Path $PSScriptRoot 'Microsoft.WindowsAppRuntime.2.2-x64.msix')
        $confirmed = Get-AppxPackage -Name $packageName
        if (-not $confirmed -or [version]$confirmed.Version -lt $packageVersion) { throw 'Installation was not confirmed.' }
        Write-Host 'Installed. Close this administrator window and run the Launch step in ordinary PowerShell.'
        return
    }

    if ($Step -eq 'Launch') {
        $app = Get-AppxPackage -Name $packageName
        if (-not $app) { throw 'Granite is not installed for this account.' }
        if ([version]$app.Version -lt $packageVersion) { throw 'The older Granite installation needs the Install step first.' }
        Start-Process explorer.exe -ArgumentList ('shell:AppsFolder\{0}!App' -f $app.PackageFamilyName)
        return
    }
} catch {
    Write-Host 'STOP: Do not continue to the next step.' -ForegroundColor Red
    Write-Host 'Follow the specific error above. Do not bypass checksum, certificate, or Windows security checks.' -ForegroundColor Yellow
    throw
}
