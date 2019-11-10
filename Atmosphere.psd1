@{
  RootModule = 'Atmosphere.psm1'
  ModuleVersion = '0.1.0'
  CompatiblePSEditions = 'Core'
  GUID = 'c0cde49c-3bf7-4b20-84ec-c56a7a7d5969'
  Author = 'Isabella Muerte'
  CompanyName = 'Unknown'
  Copyright = '(c) Isabella Muerte. All rights reserved.'
  Description = 'Functions for working with Environment Variables'
  PowerShellVersion = '6.0'
  FunctionsToExport = @(
    "Update-EnvironmentVariable",
    "Get-EnvironmentVariable",
    "Set-EnvironmentVariable",
    "Import-Environment",
    "Push-Environment",
    "Pop-Environment",
    "Update-SystemPath",
    "Update-PsPath"
  )
  CmdletsToExport = @()
  VariablesToExport = @()
  AliasesToExport = @(
    "Update-EnvVar",
    "Update-Path",
    "Get-EnvVar",
    "Set-EnvVar",
    "Push-Env",
    "Pop-Env"
  )
  PrivateData = @{
      PSData = @{
          Tags = @("Environment", "Utility", "EnvVars", "Linux", "Windows", "Mac")
          ProjectUri = 'https://github.com/slurps-mad-rips/atmosphere'
          ReleaseNotes = @'
# 0.1.0

Initial release!
'@

      }
  }
}
