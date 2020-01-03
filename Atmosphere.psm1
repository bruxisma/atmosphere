using namespace System.Collections.Generic
using namespace System.Collections
using namespace System.IO

# Used to convert Env: into a hashtable.
function script:into-hashtable {
  $table = @{}
  $env = Get-Item Env:
  foreach ($entry in $env) {
    $table.Add($entry.Name, $entry.Value)
  }
  return $table
}

function script:into-directory ($Path) {
  if (Test-Path -PathType Leaf $Path) { return Split-Path $Path }
  return $Path
}

$script:EnvironmentStack = New-Object Stack[HashTable]

<#
  .SYNOPSIS
    Get an environment variable's value
  .DESCRIPTION
    Returns the value of an environment variable with the given scope
  .PARAMETER Name
    Name of the environment variable
  .PARAMETER Scope
    Scope of the environment variable. This only affects Windows. If any value
    other than "Process" is passed under non-Windows, a warning will be emitted
    and the value will be set to "Process"
  .PARAMETER AsPath
    Splits the variable via [IO.Path]::PathSeparator before returning it.
  .EXAMPLE
    PS> Get-EnvironmentVariable -Name USERPROFILE -Scope Machine
  .EXAMPLE
    PS> Get-EnvironmentVariable PATH -AsPath
  .NOTES
    This is functionaly equivalent to $env:$Name, however it is usable in many
    more general contexts
#>
function Get-EnvironmentVariable {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
    [ValidateNotNull()]
    [String]
    $Name,
    [Parameter(Mandatory=$false)]
    [ValidateSet("Process", "Machine", "User")]
    [String]
    $Scope = "Process",
    [Parameter(Mandatory=$false)]
    [Alias("Split")]
    [Switch]
    $AsPath = $false
  )
  if ((-not $IsWindows) -and ($Scope -ne "Process")) {
    Write-Warning "`$Scope should only be 'Process' on non-Windows"
    $Scope = "Process"
  }
  $value = [Environment]::GetEnvironmentVariable($Name, $Scope)
  if ($AsPath) { return $value.Split([Path]::PathSeparator) }
  return $value
}

<#
  .SYNOPSIS
    Sets an environment variable.
  .DESCRIPTION
    Sets an environment variable with the given name and scope.
  .PARAMETER Name
    Name of the variable
  .PARAMETER Value
    Value to be set
  .PARAMETER Scope
    Scope of the environment variable. This only affects Windows. If any value
    other than "Process" is passed under non-Windows, a warning will be emitted
    and the value will be set to "Process"
  .PARAMETER AsPath
    Treat the Value as a "path" variable, and join it via the pathseparator
    before setting it. Without this, the value will be joined with " "
  .EXAMPLE
    PS> Set-EnvironmentVariable -Name XDG_CACHE_HOME -Value $HOME/.cache -Scope User
  .NOTES
    This is functionally equivalent to $env:<Name> = <Value>, but is usable in
    more contexts
#>
function Set-EnvironmentVariable {
  [CmdletBinding()]
  param(
    [Parameter(
      Mandatory=$true,
      ValueFromPipeline=$true,
      ValueFromPipelineByPropertyName=$true)]
    [ValidateNotNullOrEmpty()]
    [String]
    $Name,
    [Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true)]
    [ValidateNotNull()]
    [String[]]
    $Value,
    [Parameter(Mandatory=$false)]
    [ValidateSet("Process", "Machine", "User")]
    [String]
    $Scope = "Process",
    [Parameter(Mandatory=$false)]
    [Alias("Join")]
    [Switch]
    $AsPath = $false,
    [Parameter(Mandatory=$false)]
    [Switch]
    $Unique = $false
  )

  if ((-not $IsWindows) -and ($Scope -ne "Process")) {
    Write-Warning "`$Scope should only be 'Process' on non-Windows"
    $Scope = "Process"
  }

  if ($AsPath) {
    $Value = $Value `
           | Select-Object -Unique:$Unique `
           | Join-String -Separator ([Path]::PathSeparator)
  }
  [Environment]::SetEnvironmentVariable($Name, $Value, $Scope)
}

<#
  .SYNOPSIS
    Appends (or prepends) data to the given variable.
#>
function Update-EnvironmentVariable {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
    [ValidateNotNullOrEmpty()]
    [String]
    $Name,
    [Parameter(Mandatory=$true)]
    [ValidateNotNull()]
    [String[]]
    $Value,
    [Parameter(Mandatory=$false)]
    [ValidateSet("Process", "Machine", "User")]
    [String]
    $Scope = "Process",
    [Parameter(Mandatory=$false)]
    [Alias("Join")]
    [Switch]
    $AsPath = $false,
    [Parameter(Mandatory=$false)]
    [Switch]
    $Prepend = $false,
    [Parameter(Mandatory=$false)]
    [Switch]
    $Push = $false,
    [Parameter(Mandatory=$false)]
    [Switch]
    $Unique = $false
  )
  if ((-not $IsWindows) -and ($Scope -ne "Process")) {
    Write-Warning "`$Scope should only be 'Process' on non-Windows"
    $Scope = "Process"
  }

  $current = Get-EnvironmentVariable -Name $Name -Scope $Scope -AsPath:$AsPath
  $values = [ArrayList]@($current)
  if ($Prepend) { $values.Insert(0, $Value) } else { $values.Add($Value) | Out-Null }
  if ($Push) { Push-Environment }

  Set-EnvironmentVariable `
    -Name $Name `
    -Value $values `
    -Scope $Scope `
    -AsPath:$AsPath `
    -Unique:$Unique
}
<#
  .SYNOPSIS
    Imports data from a file into the current environment
  .DESCRIPTION
    Given a file that can be imported or read from in powershell into a
    hashtable, each key-value pair is then set in the current environment.
    Currently supported formats are
      * PowerShellDataFile (.psd1)
      * JSON (.json)
  .PARAMETER Path
    Path to the file to be imported from
  .PARAMETER Format
    Format of the data to import
  .PARAMETER Push
    Push the current environment before setting variables.
  .PARAMETER When
    Only execute if true. This allows for things like -When $IsWindows
#>
function Import-Environment {
  [CmdletBinding()]
  param(
    [Parameter(
      Mandatory=$true,
      ValueFromPipeline=$true)]
    [ValidateNotNullOrEmpty()]
    [String]
    $Path,
    [Parameter(Mandatory=$true)]
    [ValidateSet("JSON", "PSD1")]
    [String]
    $Format,
    [Parameter(Mandatory=$false)]
    [Switch]
    $Push = $true,
    [Parameter(Mandatory=$false)]
    [bool]
    $When = $true
  )
  if (-not (Test-Path -PathType Leaf $Path)) {
    Write-Warning "'$Path' does not exist"
    return
  }
  if (-not $When) { return }
  $content = if ($Format -eq "JSON") {
    $json = (Get-Content $Path) -as [String]
    ConvertFrom-Json $json -AsHashTable
  } elseif ($Format -eq "PSD1") {
    Import-PowerShellDataFile $Path
  }
  if ($Push) { Push-Environment $content }
  foreach ($entry in $content.GetEnumerator()) {
    Set-EnvironmentVariable -Name $entry.Name -Value $entry.Value
  }
}

<#
  .SYNOPSIS
    Reset the environment to the previous state that was pushed onto the
    internal stack. It then returns the state that was replaced in the form of
    a HashTable
#>
function Pop-Environment {
  [CmdletBinding()]
  param()
  trap { throw $_ }

  if (-not $script:EnvironmentStack) { return @{} }
  $state = $script:EnvironmentStack.Pop()
  $dict = @{}
  foreach ($entry in $state.GetEnumerator()) {
    $value = Get-EnvironmentVariable $entry.Name
    $dict.Add($entry.Name, $value)
    Set-EnvironmentVariable $entry.Name -Value $entry.Value
  }
  return $dict
}

<# Helper to just update the system path #>
function Update-SystemPath {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]
    $Path,
    [Parameter(Mandatory=$false)]
    [Switch]
    $Prepend = $false,
    [Parameter(Mandatory=$false)]
    [Switch]
    $Push = $false
  )
  Update-EnvironmentVariable `
    -Name PATH `
    -Value (script:into-directory $Path) `
    -Prepend:$Prepend `
    -Push:$Push `
    -Unique `
    -AsPath
}

<# Helper to just update the powershell module path #>
function Update-PsPath {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]
    $Path,
    [Parameter(Mandatory=$false)]
    [Switch]
    $Prepend = $false,
    [Parameter(Mandatory=$false)]
    [Switch]
    $Push = $false
  )
  Update-EnvironmentVariable `
    -Name PsModulePath `
    -Value (script:into-directory $Path) `
    -Prepend:$Prepend `
    -Push:$Push `
    -Unique `
    -AsPath
}

Set-Alias Update-EnvVar Update-EnvironmentVariable
Set-Alias Update-Path Update-SystemPath
Set-Alias Get-EnvVar Get-EnvironmentVariable
Set-Alias Set-EnvVar Set-EnvironmentVariable
Set-Alias Import-Env Import-Environment
Set-Alias Push-Env Push-Environment
Set-Alias Pop-Env Pop-Environment
