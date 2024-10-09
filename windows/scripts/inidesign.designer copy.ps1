$Form1 = New-Object -TypeName System.Windows.Forms.Form
[System.Windows.Forms.RadioButton]$rbn_allinone = $null
[System.Windows.Forms.RadioButton]$rbn_lfis = $null
[System.Windows.Forms.Label]$lbl_allinone = $null
[System.Windows.Forms.Label]$lbl_lfis = $null
[System.Windows.Forms.Button]$btn_install = $null
[System.Windows.Forms.Label]$lbl_aboutoinstall = $null
[System.Windows.Forms.Label]$lbl_hoststats = $null
[System.Windows.Forms.Panel]$pnl_white = $null
[System.Windows.Forms.Label]$lbl_documentation = $null
[System.Windows.Forms.Label]$lbl_evaluationDemo = $null
[System.Windows.Forms.LinkLabel]$lnk_documentation = $null
[System.Windows.Forms.PictureBox]$img_SmartFaceLogo = $null
function InitializeComponent
{
$resources = . (Join-Path $PSScriptRoot 'inidesign.resources.ps1')
$rbn_allinone = (New-Object -TypeName System.Windows.Forms.RadioButton)
$rbn_lfis = (New-Object -TypeName System.Windows.Forms.RadioButton)
$lbl_allinone = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_lfis = (New-Object -TypeName System.Windows.Forms.Label)
$btn_install = (New-Object -TypeName System.Windows.Forms.Button)
$lbl_aboutoinstall = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_hoststats = (New-Object -TypeName System.Windows.Forms.Label)
$pnl_white = (New-Object -TypeName System.Windows.Forms.Panel)
$img_SmartFaceLogo = (New-Object -TypeName System.Windows.Forms.PictureBox)
$lnk_documentation = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lbl_evaluationDemo = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_documentation = (New-Object -TypeName System.Windows.Forms.Label)
$pnl_white.SuspendLayout()
([System.ComponentModel.ISupportInitialize]$img_SmartFaceLogo).BeginInit()
$Form1.SuspendLayout()
#
#rbn_allinone
#
$rbn_allinone.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]111,[System.Int32]178))
$rbn_allinone.Name = [System.String]'rbn_allinone'
$rbn_allinone.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]104,[System.Int32]24))
$rbn_allinone.TabIndex = [System.Int32]2
$rbn_allinone.TabStop = $true
$rbn_allinone.Text = [System.String]'All in one'
$rbn_allinone.UseVisualStyleBackColor = $true
#
#rbn_lfis
#
$rbn_lfis.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]376,[System.Int32]178))
$rbn_lfis.Name = [System.String]'rbn_lfis'
$rbn_lfis.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]104,[System.Int32]24))
$rbn_lfis.TabIndex = [System.Int32]3
$rbn_lfis.TabStop = $true
$rbn_lfis.Text = [System.String]'LFIS Demo'
$rbn_lfis.UseVisualStyleBackColor = $true
#
#lbl_allinone
#
$lbl_allinone.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]97,[System.Int32]226))
$lbl_allinone.Name = [System.String]'lbl_allinone'
$lbl_allinone.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]147,[System.Int32]147))
$lbl_allinone.TabIndex = [System.Int32]4
$lbl_allinone.Text = [System.String]'Evaluation preset for 4 cameras


All features available



Needs 8 core CPU
Needs 16GB Ram
Needs 40 GB of HDD space'
#
#lbl_lfis
#
$lbl_lfis.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]355,[System.Int32]226))
$lbl_lfis.Name = [System.String]'lbl_lfis'
$lbl_lfis.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]172,[System.Int32]147))
$lbl_lfis.TabIndex = [System.Int32]5
$lbl_lfis.Text = [System.String]'Evaluation preset for the Lightweight Facial Identification Service

Demo of a solution suitable for cloud instances


Needs 4 core CPU
Needs 8GB Ram 
Needs 40 GB of HDD space'
#
#btn_install
#
$btn_install.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]469,[System.Int32]405))
$btn_install.Name = [System.String]'btn_install'
$btn_install.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]123,[System.Int32]47))
$btn_install.TabIndex = [System.Int32]6
$btn_install.Text = [System.String]'Install'
$btn_install.UseVisualStyleBackColor = $true
$btn_install.add_Click($Button1_Click)
#
#lbl_aboutoinstall
#
$lbl_aboutoinstall.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]8.25,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_aboutoinstall.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]30,[System.Int32]137))
$lbl_aboutoinstall.Name = [System.String]'lbl_aboutoinstall'
$lbl_aboutoinstall.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]450,[System.Int32]23))
$lbl_aboutoinstall.TabIndex = [System.Int32]7
$lbl_aboutoinstall.Text = [System.String]'You are about to install the SmartFace Evaluation Demo. Choose your setup:'
#
#lbl_hoststats
#
$lbl_hoststats.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]30,[System.Int32]405))
$lbl_hoststats.Name = [System.String]'lbl_hoststats'
$lbl_hoststats.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]386,[System.Int32]30))
$lbl_hoststats.TabIndex = [System.Int32]8
$lbl_hoststats.Text = [System.String]'Your computer has X CPU cores and Y GB of Ram. Please ensure your host computer has available resources.'
#
#pnl_white
#
$pnl_white.BackColor = [System.Drawing.Color]::White
$pnl_white.Controls.Add($lbl_documentation)
$pnl_white.Controls.Add($lbl_evaluationDemo)
$pnl_white.Controls.Add($lnk_documentation)
$pnl_white.Controls.Add($img_SmartFaceLogo)
$pnl_white.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]0,[System.Int32]0))
$pnl_white.Name = [System.String]'pnl_white'
$pnl_white.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]626,[System.Int32]114))
$pnl_white.TabIndex = [System.Int32]1
#
#img_SmartFaceLogo
#
$img_SmartFaceLogo.BackgroundImage = ([System.Drawing.Image]$resources.'img_SmartFaceLogo.BackgroundImage')
$img_SmartFaceLogo.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]14))
$img_SmartFaceLogo.Name = [System.String]'img_SmartFaceLogo'
$img_SmartFaceLogo.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]371,[System.Int32]87))
$img_SmartFaceLogo.TabIndex = [System.Int32]9
$img_SmartFaceLogo.TabStop = $false
#
#lnk_documentation
#
$lnk_documentation.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]398,[System.Int32]91))
$lnk_documentation.Name = [System.String]'lnk_documentation'
$lnk_documentation.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]225,[System.Int32]23))
$lnk_documentation.TabIndex = [System.Int32]9
$lnk_documentation.TabStop = $true
$lnk_documentation.Text = [System.String]'http://developer.innovatrics.com/smartfrace'
#
#lbl_evaluationDemo
#
$lbl_evaluationDemo.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]8.25,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_evaluationDemo.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]238,[System.Int32]91))
$lbl_evaluationDemo.Name = [System.String]'lbl_evaluationDemo'
$lbl_evaluationDemo.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]145,[System.Int32]23))
$lbl_evaluationDemo.TabIndex = [System.Int32]10
$lbl_evaluationDemo.Text = [System.String]'Evaluation DEMO'
#
#lbl_documentation
#
$lbl_documentation.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]398,[System.Int32]78))
$lbl_documentation.Name = [System.String]'lbl_documentation'
$lbl_documentation.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]216,[System.Int32]13))
$lbl_documentation.TabIndex = [System.Int32]11
$lbl_documentation.Text = [System.String]'SmartFace documentation'
#
#Form1
#
$Form1.ClientSize = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]626,[System.Int32]472))
$Form1.Controls.Add($pnl_white)
$Form1.Controls.Add($lbl_hoststats)
$Form1.Controls.Add($lbl_aboutoinstall)
$Form1.Controls.Add($btn_install)
$Form1.Controls.Add($lbl_lfis)
$Form1.Controls.Add($lbl_allinone)
$Form1.Controls.Add($rbn_lfis)
$Form1.Controls.Add($rbn_allinone)
$Form1.Icon = ([System.Drawing.Icon]$resources.'$this.Icon')
$Form1.MaximizeBox = $false
$Form1.Text = [System.String]'SmartFace Demo'
$Form1.TopMost = $true
$pnl_white.ResumeLayout($false)
([System.ComponentModel.ISupportInitialize]$img_SmartFaceLogo).EndInit()
$Form1.ResumeLayout($false)
Add-Member -InputObject $Form1 -Name rbn_allinone -Value $rbn_allinone -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name rbn_lfis -Value $rbn_lfis -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_allinone -Value $lbl_allinone -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_lfis -Value $lbl_lfis -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_install -Value $btn_install -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_aboutoinstall -Value $lbl_aboutoinstall -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_hoststats -Value $lbl_hoststats -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name pnl_white -Value $pnl_white -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_documentation -Value $lbl_documentation -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_evaluationDemo -Value $lbl_evaluationDemo -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lnk_documentation -Value $lnk_documentation -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name img_SmartFaceLogo -Value $img_SmartFaceLogo -MemberType NoteProperty
}
. InitializeComponent
