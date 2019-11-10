# Overview

Atmosphere is a Powershell Core 6.2+ module for pushing, popping, getting,
setting, and updating environment variables. It current works on Windows,
macOS, and Linux. It's released under the MIT License. 

# Installation

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

[Powershell Gallery]: https://www.powershellgallery.com/
