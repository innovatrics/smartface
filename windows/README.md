# Windows Powershell Scripts

Query all SmartFace services:
```
Get-Service -Name "SF*"
```

**WARNING!** 
Scripts query services by name pattern. It may happen that other application/system register service with similair pattern. It is always recomended to query all SmartFace services before to ensure only propper SmartFace services will be affected.

Stop all SF related services

*PowerShell 5.1 and later*
```
Get-Service -Name "SF*" | Stop-Service
```

Remove all SF related services
```
Get-Service -Name "SF*" | Remove-Service
```

Get-Service | where {$_.Name -like "SF*"} | foreach {sc.exe stop $_.Name}
Get-Service | where {$_.Name -like "SF*"} | foreach {sc.exe delete $_.Name}
Get-Service | where {$_.Name -like "SF*"} | foreach {sc.exe config $_.Name start= disabled}

Get-Service | where {$_.Name -like "SF*"} | foreach {sc.exe stop $_.Name}
Get-Service | where {$_.Name -like "SF*"} | foreach {sc.exe start $_.Name}