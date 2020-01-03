# This is more of a "Does it import and execute" file. Better tests *are*
# needed. It'd be nice if we could use Pester but MSBuild is a dumpster fire
# and barely supports Powershell, so we're SoL. Thanks MSBuild. It's 2020, and
# I can't run powershell core under .net core. Also your RoslynCodeTaskFactory
# is broken as hell. I hate you, and I hope you die.
Import-Module $PWD/Atmosphere.dll

Write-Host "EnvironmentPathVariable: GOPATH"
Get-EnvironmentPath -Name GOPATH | %{ $_.Fullname }
Write-Host "SystemPath"
Get-SystemPath | %{ $_.Fullname }
Write-Host "PSModulePath"
Get-PSModulePath | %{ $_.Fullname }
Write-Host "PythonPath"
Get-PythonPath | %{ $_.Fullname }

Update-SystemPath -?
