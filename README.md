# ChargingStatus

`ChargingStatus` is a PowerToys Command Palette extension that appears in the UI as `Battery Charging Status` and shows battery status, charge or discharge rate, and a compact Dock band with a state-aware battery icon.

## Features

- Top-level `Battery Charging Status` command in Command Palette
- Dock band for persistent battery visibility
- Event-driven refresh from Windows battery and power notifications, with a short fallback poll
- Battery percentage, status, remaining/full capacity, and charge or discharge rate
- Shortcuts to Windows battery and power settings

## Build

```powershell
$env:DOTNET_CLI_HOME="$PWD\\.dotnet"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE='1'
dotnet restore .\ChargingStatus\ChargingStatus.csproj
dotnet build .\ChargingStatus\ChargingStatus.csproj -c Debug
```

The generated package is written under:

- `ChargingStatus\bin\Debug\net9.0-windows10.0.26100.0\win-x64\AppPackages\`

## Local install

For local development you can register the unpackaged output directly:

```powershell
Add-AppxPackage -Register .\ChargingStatus\bin\Debug\net9.0-windows10.0.26100.0\win-x64\AppxManifest.xml
```

Then open Command Palette and run `reload` -> `Reload Command Palette extensions`.

## Publish notes

Before publishing outside your machine, update these release-specific values:

- `Package.appxmanifest` publisher to match the subject of a real signing certificate
- package signing configuration for your release certificate
- package version and release notes for the build you want to distribute
