Add-Type -AssemblyName System.Windows.Forms
. (Join-Path $PSScriptRoot 'inidesign.designer.ps1')
$Form1.ShowDialog()