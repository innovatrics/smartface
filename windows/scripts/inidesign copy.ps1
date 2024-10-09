$GroupBox1_Enter = {
}
$Button1_Click = {
}
Add-Type -AssemblyName System.Windows.Forms
. (Join-Path $PSScriptRoot 'inidesign.designer.ps1')
$Form1.ShowDialog()