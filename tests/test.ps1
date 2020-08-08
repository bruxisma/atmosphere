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
