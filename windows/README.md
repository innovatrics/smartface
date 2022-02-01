# Windows Powershell Scripts


### Get SmartFace services
```
Get-Service -Name "SF*"
```

***********************************************************************************************************************************************
**WARNING!**
  
Scripts query services by the Service Name pattern. 
It may happen that other application/system register service with similar pattern. 
We always recommend you to list all SmartFace services before running other commands to ensure that only SmartFace are affected.
***********************************************************************************************************************************************

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

### Start all SmartFace services

```
Get-Service -Name "SF*" | foreach {sc.exe start $_.Name}
```


# Windows Command Line Scripts

### Add extra SmartFace service

```
SC CREATE "SFBodyPartsDetectCpu4" binpath= "C:\Program Files\Innovatrics\SmartFace\RpcBodyPartsDetector.exe Gpu:GpuEnabled=false" DisplayName= "SmartFace Body Parts 4" start= auto
```