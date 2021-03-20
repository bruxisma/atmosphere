@{
  GUID = 'c0cde49c-3bf7-4b20-84ec-c56a7a7d5969'
  Author = 'Isabella Muerte'
  CompanyName = ''
  Copyright = '(c) Isabella Muerte. All rights reserved.'
  Description = 'Cmdlets for working with environment variables and paths'
  ModuleVersion = '1.0.0'
  CompatiblePSEditions = 'Core'
  PowerShellVersion = '7.0'
  RootModule = 'Atmosphere.dll'

  CmdletsToExport = @(
    'Get-EnvironmentVariable'
    'Set-EnvironmentVariable'
    'Get-EnvironmentPath'
    'Get-LDLibraryPath'
    'Get-PkgConfigPath'
    'Get-PSModulePath'
    'Get-PythonPath'
    'Get-SystemPath'
    'Update-LDLibraryPath'
    'Update-PkgConfigPath'
    'Update-PSModulePath'
    'Update-PythonPath'
    'Update-SystemPath'
    'Import-Environment'
    'Push-Environment'
    'Pop-Environment'
  )
  FunctionsToExport = @()
  VariablesToExport = @()
  AliasesToExport = @()
  PrivateData = @{
    PSData = @{
      Tags = @("Environment", "Utility", "EnvVars", "Linux", "Windows", "Mac")
      ProjectUri = 'https://github.com/slurps-mad-rips/atmosphere'
      Prerelease = 'Alpha'
      ReleaseNotes = @'
# 1.0.0-Alpha

♻ Rewrote all cmdlets as Binary Cmdlets.
  While this might be considered unnecessary it did result in less memory usage
  and paved a way to better understand using .NET Core with Powershell. In
  other words, we're one step closer to "automatically binding" C and C++
  libraries to .NET and then turning them into Cmdlets. This means nearly
  anything can be done in powershell. And that's terrifying.

  Another reason this rewrite was done was to fix some type system hacks that
  we had to work around. Now you can pass a properly typed dictionary into
  `Push-Environment`.

✨ Added several new cmdlets.

 * `Get-LDLibraryPath`
 * `Get-PkgConfigPath`
 * `Get-PSModulePath`
 * `Get-SystemPath`
 * `Get-PythonPath`

 * `Update-EnvironmentPath`

🚚 Renamed several cmdlets and reorganized operations.

 * `Get-EnvironmentVariable -AsPath` is now `Get-EnvironmentPath`
 * Likewise, `Set-EnvironmentVariable -AsPath` is now `Set-EnvironmentPath`
 * Update-EnvironmentVariable will now error if the environment variable does
   not exist

🔥 Removed builtin Aliases.
  These could easily clash and users should alias cmdlets themselves. We aren't
  microsoft, so providing aliases should be done in a user's profile.

# 0.1.1

🐛 Fix output showing up when updating an environment variable
🐛 Fix inability to deduplicate environment variables
✨ The path to a file can now be added. It's directory will be used instead.

# 0.1.0

Initial release!
'@
    }
  }

}
