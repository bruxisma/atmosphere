@{
  GUID = 'c0cde49c-3bf7-4b20-84ec-c56a7a7d5969'
  Author = 'Isabella Muerte'
  CompanyName = ''
  Copyright = '(c) Isabella Muerte. All rights reserved.'
  Description = 'Cmdlets for working with environment variables and paths'
  ModuleVersion = '0.3.0'
  CompatiblePSEditions = 'Core'
  PowerShellVersion = '7.2'
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
    'Push-Environment'
    'Pop-Environment'
  )
  FunctionsToExport = @()
  VariablesToExport = @()
  AliasesToExport = @()
  PrivateData = @{
    PSData = @{
      Tags = @("Environment", "Utility", "EnvVars", "Linux", "Windows", "Mac")
      ProjectUri = 'https://github.com/bruxisma/atmosphere'
      Prerelease = 'Alpha'
      ReleaseNotes = @'
# 0.3.0-Alpha

⬆ Upgrade to Powershell 7.2 and .NET 6

This was a long time coming, as 7.2 is am LTS release now. The primary changes
with this release are a bump in dependencies and a change in how the project is
generated. Having the prior 0.2.0-Alpha allows us to break free from Powershell
7.0 and still have a general 'upgrade' path for users.

Of note: This release will most likely be the last one, with some brief cooking
before a proper 1.0 release.

🔨 Modify build system to be more streamlined/helpful

The build system was streamlined ever so slightly to improve the build
experience. We still have a bunch of work to go, but the build itself is now
more helpful in GitHub Actions.

🔥 Removed the Import-Environment cmdlet

After a bunch of frustration it was basically impossible to properly test the
Import-Environment cmdlet for PSD files, JSON files required strings on both
sides, and .env files were a nightmare to support. In the end, this feature has
been cut for the 0.3.0 release, and any future releases because I have no
desire to support any of this.

# 0.2.0-Alpha

♻ Rewrote all cmdlets as Binary Cmdlets.

While this might be considered unnecessary, it has resulted in less memory
usage and faster execution. Additionally, several type system "hacks" were
fixed with this move. As an example a properly typed dictionary can be given to
`Push-Environment`

✨ Added Several Convenience Cmdlets

 - `Get-LDLibraryPath`
 - `Get-PkgConfigPath`
 - `Get-PSModulePath`
 - `Get-PythonPath`
 - `Get-SystemPath`

These all return well known environment variables. `Get-SystemPath` refers to
the `${env:PATH}` variable. There are also `Update-` versions of each.

 - `Update-LDLibraryPath`
 - `Update-PkgConfigPath`
 - `Update-PSModulePath`
 - `Update-PythonPath`
 - `Update-SystemPath`

✨ Added `Import-Environment` cmdlet

This is currently underpowered (hence the 0.2.0-Alpha), but it is currently
capable of importing JSON files into a user's environment. Soon we will support
PowerShell's PSD1 format, and .env files as well.

🚚 Renamed several cmdlets and reorganized operations.

 * `Get-EnvironmentVariable -AsPath` is now `Get-EnvironmentPath`
 * Likewise, `Set-EnvironmentVariable -AsPath` is now `Set-EnvironmentPath`
 * Update-EnvironmentVariable will now error if the environment variable does
   not exist

🔥 Removed builtin Aliases.

These could easily clash and users should alias cmdlets themselves. We aren't
Microsoft, so providing aliases should be done in a user's profile.

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
