$Form1 = New-Object -TypeName System.Windows.Forms.Form
[System.Windows.Forms.Label]$Label1 = $null
[System.Windows.Forms.RadioButton]$RadioButton1 = $null
[System.Windows.Forms.RadioButton]$RadioButton2 = $null
[System.Windows.Forms.RadioButton]$RadioButton3 = $null
[System.Windows.Forms.Label]$Label2 = $null
[System.Windows.Forms.Label]$Label3 = $null
[System.Windows.Forms.Button]$Button1 = $null
[System.Windows.Forms.Label]$Label4 = $null
[System.Windows.Forms.Label]$Label5 = $null
function InitializeComponent
{
$Label1 = (New-Object -TypeName System.Windows.Forms.Label)
$RadioButton1 = (New-Object -TypeName System.Windows.Forms.RadioButton)
$RadioButton2 = (New-Object -TypeName System.Windows.Forms.RadioButton)
$RadioButton3 = (New-Object -TypeName System.Windows.Forms.RadioButton)
$Label2 = (New-Object -TypeName System.Windows.Forms.Label)
$Label3 = (New-Object -TypeName System.Windows.Forms.Label)
$Button1 = (New-Object -TypeName System.Windows.Forms.Button)
$Label4 = (New-Object -TypeName System.Windows.Forms.Label)
$Label5 = (New-Object -TypeName System.Windows.Forms.Label)
$Form1.SuspendLayout()
#
#Label1
#
$Label1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]60,[System.Int32]111))
$Label1.Name = [System.String]'Label1'
$Label1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]162,[System.Int32]130))
$Label1.TabIndex = [System.Int32]0
$Label1.Text = [System.String]'Ready for 2 cameras


Nothing is preset



Needs 4 core CPU
Needs 8 GB Ram
Needs 40 GB of HDD space'
#
#RadioButton1
#
$RadioButton1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]82,[System.Int32]63))
$RadioButton1.Name = [System.String]'RadioButton1'
$RadioButton1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]104,[System.Int32]24))
$RadioButton1.TabIndex = [System.Int32]1
$RadioButton1.TabStop = $true
$RadioButton1.Text = [System.String]'Mini Demo'
$RadioButton1.UseVisualStyleBackColor = $true
#
#RadioButton2
#
$RadioButton2.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]320,[System.Int32]63))
$RadioButton2.Name = [System.String]'RadioButton2'
$RadioButton2.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]104,[System.Int32]24))
$RadioButton2.TabIndex = [System.Int32]2
$RadioButton2.TabStop = $true
$RadioButton2.Text = [System.String]'Full Demo'
$RadioButton2.UseVisualStyleBackColor = $true
#
#RadioButton3
#
$RadioButton3.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]557,[System.Int32]63))
$RadioButton3.Name = [System.String]'RadioButton3'
$RadioButton3.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]104,[System.Int32]24))
$RadioButton3.TabIndex = [System.Int32]3
$RadioButton3.TabStop = $true
$RadioButton3.Text = [System.String]'LFIS Demo'
$RadioButton3.UseVisualStyleBackColor = $true
#
#Label2
#
$Label2.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]306,[System.Int32]111))
$Label2.Name = [System.String]'Label2'
$Label2.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]147,[System.Int32]130))
$Label2.TabIndex = [System.Int32]4
$Label2.Text = [System.String]'Ready for 4 cameras


Includes presets for cameras and video investigation


Needs 8 core CPU
Needs 16GB Ram
Needs 40 GB of HDD space'
#
#Label3
#
$Label3.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]537,[System.Int32]111))
$Label3.Name = [System.String]'Label3'
$Label3.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]172,[System.Int32]130))
$Label3.TabIndex = [System.Int32]5
$Label3.Text = [System.String]'Preset for the Lightweight Facial Identification Service

Demo of a solution suitable for cloud instances


Needs 4 core CPU
Needs 8GB Ram 
Needs 40 GB of HDD space'
#
#Button1
#
$Button1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]624,[System.Int32]283))
$Button1.Name = [System.String]'Button1'
$Button1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]123,[System.Int32]47))
$Button1.TabIndex = [System.Int32]6
$Button1.Text = [System.String]'Install'
$Button1.UseVisualStyleBackColor = $true
#
#Label4
#
$Label4.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]21,[System.Int32]20))
$Label4.Name = [System.String]'Label4'
$Label4.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]386,[System.Int32]23))
$Label4.TabIndex = [System.Int32]7
$Label4.Text = [System.String]'You are about to install the SmartFace Demo. Choose your setup:'
#
#Label5
#
$Label5.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]60,[System.Int32]300))
$Label5.Name = [System.String]'Label5'
$Label5.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]558,[System.Int32]23))
$Label5.TabIndex = [System.Int32]8
$Label5.Text = [System.String]'Your computer has X cores and Y GB of Ram available. Please ensure your host computer has available resources.'
#
#Form1
#
$Form1.ClientSize = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]776,[System.Int32]350))
$Form1.Controls.Add($Label5)
$Form1.Controls.Add($Label4)
$Form1.Controls.Add($Button1)
$Form1.Controls.Add($Label3)
$Form1.Controls.Add($Label2)
$Form1.Controls.Add($RadioButton3)
$Form1.Controls.Add($RadioButton2)
$Form1.Controls.Add($RadioButton1)
$Form1.Controls.Add($Label1)
$Form1.MaximizeBox = $false
$Form1.Text = [System.String]'SmartFace Demo'
$Form1.TopMost = $true
$Form1.ResumeLayout($false)
Add-Member -InputObject $Form1 -Name Label1 -Value $Label1 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name RadioButton1 -Value $RadioButton1 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name RadioButton2 -Value $RadioButton2 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name RadioButton3 -Value $RadioButton3 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name Label2 -Value $Label2 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name Label3 -Value $Label3 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name Button1 -Value $Button1 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name Label4 -Value $Label4 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name Label5 -Value $Label5 -MemberType NoteProperty
}
. InitializeComponent
