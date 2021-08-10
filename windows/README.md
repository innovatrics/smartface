# Windows Powershell Scripts


### Get SmartFace services
```
Get-Service -Name "SF*"
```

****************************************************************************************************************************************************************************
**WARNING!**
  
Scripts query services by name pattern. It may happen that other application/system register service with similair pattern. It is always recomended to query all SmartFace services before to ensure only propper SmartFace services will be affected.
****************************************************************************************************************************************************************************

### Stop SmartFace services

```
Get-Service -Name "SF*" | Stop-Service
```


### Remove all SmartFace services

```
Get-Service -Name "SF*" | foreach {sc.exe delete $_.Name}
```

### Disable auto-start on all SmartFace services

```
Get-Service -Name "SF*" | foreach {sc.exe config $_.Name start= disabled}
```

### Start on all SmartFace services

```
Get-Service | where {$_.Name -like "SF*"} | foreach {sc.exe start $_.Name}
```