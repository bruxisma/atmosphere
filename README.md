# Overview 

Atmosphere is a Powershell Core module for pushing, popping, getting, setting,
updating, and importing environment variables. It currently works on Windows,
macOS, and Linux. It's released under the MIT License.

[![Unit Tests][]][Unit Test Workflow] [![Package][]][Package Workflow]

# Installation

## Install-Module

Atmosphere is currently published on the [Powershell Gallery][]:

```powershell
Install-Module -Name Atmosphere
```

If you are not in an administrator powershell instance, and lack the ability to
launch one, simply install into `CurrentUser` scope.

```powershell
Install-Module -Name Atmosphere -Scope CurrentUser
```

Implicit module importing should handle the rest.

## Building from source

Atmosphere is written directly in C# for performance, and as an example for
others who want to know how to write so-called "native" Powershell Modules. To
build, simply use the `dotnet` command.

```powershell
dotnet build --nologo --configuration Release
```

# Usage

Atmosphere provides several cmdlets for use

## Basic Usage

The most basic commands are setting and getting environment variables

```powershell
Set-EnvironmentVariable -Name <String> -Value <String> -Scope {Process|Machine|User}
```

Sets the environment variable `-Name` to the `-Value` given.
Use `Set-EnvironmentPathVariable` if setting multiple filepaths is desired.

If `-Scope` is not `Process` and the current operating system is not Windows,
it will be set to `Process`. The default value for `-Scope` is `Process`, and
rarely needs to be specified.

```powershell
Get-EnvironmentVariable -Name <Name> -Scope {Process|Machine|User}
```

Returns the value of the Environment Variable `-Name`. If no value is set, or
the variable does not exist, an empty string is returned.

See `Set-EnvironmentVariable` for information on the `-Scope` parameter.

## Getting Lists of Paths

There are some environment variables where a path separator is provided.
Having to constantly split these apart can be time consuming and as a result
Atmosphere provides *both* a generalized command to receive any environment
variable as a list of paths or specific well known environment variables as a
list of paths.

```powershell
Get-EnvironmentPath -Name PSModulePath
```

The following command (`Get-PSModulePath`) is functionally the same as the
example found above.

```powershell
Get-PSModulePath 
```

## Pushing and Popping

Atmosphere has an internal "stack" of environment variable states. This allows
users to push and pop their current state to permit temporary modifications to
and rollbacks of the set of environment variables in a given powershell
instance.

```powershell
Push-Environment
Set-EnvironmentVariable -Name DOCKER_BUILDKIT -Value 1
<# Put Docker Related Commands Here #>
Pop-Environment
```

## Updating and Importing

Atmosphere provides a small command to permit appending (or prepending)
data to an environment variable. Additionally, we can also "import"
environments, either from running foreign shell scripts (such as bash, nush,
fish, and batch files), dotenv, JSON, and PSD1 (powershell data file), 

Please note that at the moment we *only* support importing from a JSON file,
where the expected layout is `{ "key": "value" }`

[Powershell Gallery]: https://www.powershellgallery.com/
[Unit Tests]: https://github.com/slurps-mad-rips/atmosphere/workflows/Build%20and%20Test/badge.svg
[Package]: https://github.com/slurps-mad-rips/atmosphere/workflows/Package/badge.svg
[Unit Test Workflow]: https://github.com/slurps-mad-rips/atmosphere/actions/workflows/build.yml
[Package Workflow]: https://github.com/slurps-mad-rips/atmosphere/actions/workflows/package.yml