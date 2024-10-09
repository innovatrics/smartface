$Form1 = New-Object -TypeName System.Windows.Forms.Form
[System.Windows.Forms.Panel]$pnl_white = $null
[System.Windows.Forms.Label]$lbl_documentation = $null
[System.Windows.Forms.Label]$lbl_evaluationDemo = $null
[System.Windows.Forms.LinkLabel]$lnk_documentation = $null
[System.Windows.Forms.PictureBox]$img_SmartFaceLogo = $null
[System.Windows.Forms.TabControl]$tab_control01 = $null
[System.Windows.Forms.TabPage]$tab_install = $null
[System.Windows.Forms.Label]$lbl_install_prerequisities_info03 = $null
[System.Windows.Forms.LinkLabel]$lnk_install_prerequisities_hyperv = $null
[System.Windows.Forms.Label]$lbl_install_prerequisities = $null
[System.Windows.Forms.Label]$lbl_install_prerequisities_info01 = $null
[System.Windows.Forms.LinkLabel]$lnk_install_prerequisities_multipass = $null
[System.Windows.Forms.LinkLabel]$lnk_install_prerequisities_chocolatey = $null
[System.Windows.Forms.Label]$lbl_install_prerequisities_info02 = $null
[System.Windows.Forms.Label]$lbl_install_yourcomputer = $null
[System.Windows.Forms.Label]$lbl_install_allinone_info = $null
[System.Windows.Forms.Button]$Button4 = $null
[System.Windows.Forms.TabPage]$tab_license = $null
[System.Windows.Forms.Label]$lbl_license_info2 = $null
[System.Windows.Forms.Button]$btn_license_loginregistry = $null
[System.Windows.Forms.Label]$lbl_license_header2 = $null
[System.Windows.Forms.Label]$lbl_license_info = $null
[System.Windows.Forms.LinkLabel]$lnk_license_customerportal = $null
[System.Windows.Forms.Label]$lbl_license_header1 = $null
[System.Windows.Forms.Button]$btn_license_clipboard = $null
[System.Windows.Forms.TextBox]$txt_license_hwid = $null
[System.Windows.Forms.Label]$lbl_license_hwid = $null
[System.Windows.Forms.LinkLabel]$lnk_license_obtainlicense = $null
[System.Windows.Forms.Label]$lbl_license_password = $null
[System.Windows.Forms.TextBox]$txt_license_password = $null
[System.Windows.Forms.Button]$btn_license_selectlicensefile = $null
[System.Windows.Forms.TextBox]$txt_license_filepath = $null
[System.Windows.Forms.Label]$lbl_license_filepath = $null
[System.Windows.Forms.TextBox]$txt_license_username = $null
[System.Windows.Forms.Label]$lbl_license_username = $null
[System.Windows.Forms.Button]$btn_license_continue = $null
[System.Windows.Forms.TabPage]$tab_run = $null
[System.Windows.Forms.Button]$btn_services_show = $null
[System.Windows.Forms.Label]$lbl_wmstatus = $null
[System.Windows.Forms.Label]$lbl_demoevaluationlinks = $null
[System.Windows.Forms.Label]$lbl_services_status_data = $null
[System.Windows.Forms.Label]$lbl_services_status = $null
[System.Windows.Forms.Button]$btn_services_status_stop = $null
[System.Windows.Forms.Button]$btn_services_status_start = $null
[System.Windows.Forms.Label]$lbl_data_ip_vm = $null
[System.Windows.Forms.Button]$btn_logintoterminal = $null
[System.Windows.Forms.Label]$lbl_demolink_restapi = $null
[System.Windows.Forms.Label]$lbl_demolink_graphql = $null
[System.Windows.Forms.Label]$lbl_demolink_station = $null
[System.Windows.Forms.LinkLabel]$lbl_demolink_station_data = $null
[System.Windows.Forms.LinkLabel]$lbl_demolink_restapi_data = $null
[System.Windows.Forms.LinkLabel]$lbl_demolink_graphql_data = $null
[System.Windows.Forms.Label]$lbl_wmstatus_data = $null
[System.Windows.Forms.Label]$Label22 = $null
[System.Windows.Forms.Button]$btn_wmstatus_start = $null
[System.Windows.Forms.Button]$btn_wmstatus_stop = $null
[System.Windows.Forms.TabPage]$tab_uninstall = $null
[System.Windows.Forms.Label]$lbl_uninstall = $null
[System.Windows.Forms.Button]$btn_uninstall = $null
function InitializeComponent
{
$resources = . (Join-Path $PSScriptRoot 'inidesign.resources.ps1')
$pnl_white = (New-Object -TypeName System.Windows.Forms.Panel)
$lbl_documentation = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_evaluationDemo = (New-Object -TypeName System.Windows.Forms.Label)
$lnk_documentation = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$img_SmartFaceLogo = (New-Object -TypeName System.Windows.Forms.PictureBox)
$tab_control01 = (New-Object -TypeName System.Windows.Forms.TabControl)
$tab_install = (New-Object -TypeName System.Windows.Forms.TabPage)
$lbl_install_prerequisities_info03 = (New-Object -TypeName System.Windows.Forms.Label)
$lnk_install_prerequisities_hyperv = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lbl_install_prerequisities = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_install_prerequisities_info01 = (New-Object -TypeName System.Windows.Forms.Label)
$lnk_install_prerequisities_multipass = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lnk_install_prerequisities_chocolatey = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lbl_install_prerequisities_info02 = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_install_yourcomputer = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_install_allinone_info = (New-Object -TypeName System.Windows.Forms.Label)
$Button4 = (New-Object -TypeName System.Windows.Forms.Button)
$tab_license = (New-Object -TypeName System.Windows.Forms.TabPage)
$lbl_license_info2 = (New-Object -TypeName System.Windows.Forms.Label)
$btn_license_loginregistry = (New-Object -TypeName System.Windows.Forms.Button)
$lbl_license_header2 = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_license_info = (New-Object -TypeName System.Windows.Forms.Label)
$lnk_license_customerportal = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lbl_license_header1 = (New-Object -TypeName System.Windows.Forms.Label)
$btn_license_clipboard = (New-Object -TypeName System.Windows.Forms.Button)
$txt_license_hwid = (New-Object -TypeName System.Windows.Forms.TextBox)
$lbl_license_hwid = (New-Object -TypeName System.Windows.Forms.Label)
$lnk_license_obtainlicense = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lbl_license_password = (New-Object -TypeName System.Windows.Forms.Label)
$txt_license_password = (New-Object -TypeName System.Windows.Forms.TextBox)
$btn_license_selectlicensefile = (New-Object -TypeName System.Windows.Forms.Button)
$txt_license_filepath = (New-Object -TypeName System.Windows.Forms.TextBox)
$lbl_license_filepath = (New-Object -TypeName System.Windows.Forms.Label)
$txt_license_username = (New-Object -TypeName System.Windows.Forms.TextBox)
$lbl_license_username = (New-Object -TypeName System.Windows.Forms.Label)
$btn_license_continue = (New-Object -TypeName System.Windows.Forms.Button)
$tab_run = (New-Object -TypeName System.Windows.Forms.TabPage)
$btn_services_show = (New-Object -TypeName System.Windows.Forms.Button)
$lbl_wmstatus = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_demoevaluationlinks = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_services_status_data = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_services_status = (New-Object -TypeName System.Windows.Forms.Label)
$btn_services_status_stop = (New-Object -TypeName System.Windows.Forms.Button)
$btn_services_status_start = (New-Object -TypeName System.Windows.Forms.Button)
$lbl_data_ip_vm = (New-Object -TypeName System.Windows.Forms.Label)
$btn_logintoterminal = (New-Object -TypeName System.Windows.Forms.Button)
$lbl_demolink_restapi = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_demolink_graphql = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_demolink_station = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_demolink_station_data = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lbl_demolink_restapi_data = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lbl_demolink_graphql_data = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lbl_wmstatus_data = (New-Object -TypeName System.Windows.Forms.Label)
$Label22 = (New-Object -TypeName System.Windows.Forms.Label)
$btn_wmstatus_start = (New-Object -TypeName System.Windows.Forms.Button)
$btn_wmstatus_stop = (New-Object -TypeName System.Windows.Forms.Button)
$tab_uninstall = (New-Object -TypeName System.Windows.Forms.TabPage)
$lbl_uninstall = (New-Object -TypeName System.Windows.Forms.Label)
$btn_uninstall = (New-Object -TypeName System.Windows.Forms.Button)
$pnl_white.SuspendLayout()
([System.ComponentModel.ISupportInitialize]$img_SmartFaceLogo).BeginInit()
$tab_control01.SuspendLayout()
$tab_install.SuspendLayout()
$tab_license.SuspendLayout()
$tab_run.SuspendLayout()
$tab_uninstall.SuspendLayout()
$Form1.SuspendLayout()
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
$pnl_white.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]863,[System.Int32]114))
$pnl_white.TabIndex = [System.Int32]1
#
#lbl_documentation
#
$lbl_documentation.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]599,[System.Int32]62))
$lbl_documentation.Name = [System.String]'lbl_documentation'
$lbl_documentation.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]216,[System.Int32]16))
$lbl_documentation.TabIndex = [System.Int32]11
$lbl_documentation.Text = [System.String]'SmartFace documentation'
$lbl_documentation.add_Click($lbl_documentation_Click)
#
#lbl_evaluationDemo
#
$lbl_evaluationDemo.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]8.25,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_evaluationDemo.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]242,[System.Int32]91))
$lbl_evaluationDemo.Name = [System.String]'lbl_evaluationDemo'
$lbl_evaluationDemo.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]145,[System.Int32]23))
$lbl_evaluationDemo.TabIndex = [System.Int32]10
$lbl_evaluationDemo.Text = [System.String]'Evaluation DEMO'
#
#lnk_documentation
#
$lnk_documentation.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]599,[System.Int32]78))
$lnk_documentation.Name = [System.String]'lnk_documentation'
$lnk_documentation.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]248,[System.Int32]23))
$lnk_documentation.TabIndex = [System.Int32]9
$lnk_documentation.TabStop = $true
$lnk_documentation.Text = [System.String]'http://developer.innovatrics.com/smartface'
$lnk_documentation.add_LinkClicked($lnk_documentation_LinkClicked)
#
#img_SmartFaceLogo
#
$img_SmartFaceLogo.BackgroundImage = ([System.Drawing.Image]$resources.'img_SmartFaceLogo.BackgroundImage')
$img_SmartFaceLogo.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]16,[System.Int32]14))
$img_SmartFaceLogo.Name = [System.String]'img_SmartFaceLogo'
$img_SmartFaceLogo.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]371,[System.Int32]87))
$img_SmartFaceLogo.TabIndex = [System.Int32]9
$img_SmartFaceLogo.TabStop = $false
#
#tab_control01
#
$tab_control01.Controls.Add($tab_install)
$tab_control01.Controls.Add($tab_license)
$tab_control01.Controls.Add($tab_run)
$tab_control01.Controls.Add($tab_uninstall)
$tab_control01.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]6,[System.Int32]120))
$tab_control01.Name = [System.String]'tab_control01'
$tab_control01.SelectedIndex = [System.Int32]0
$tab_control01.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]846,[System.Int32]490))
$tab_control01.TabIndex = [System.Int32]9
#
#tab_install
#
$tab_install.Controls.Add($lbl_install_prerequisities_info03)
$tab_install.Controls.Add($lnk_install_prerequisities_hyperv)
$tab_install.Controls.Add($lbl_install_prerequisities)
$tab_install.Controls.Add($lbl_install_prerequisities_info01)
$tab_install.Controls.Add($lnk_install_prerequisities_multipass)
$tab_install.Controls.Add($lnk_install_prerequisities_chocolatey)
$tab_install.Controls.Add($lbl_install_prerequisities_info02)
$tab_install.Controls.Add($lbl_install_yourcomputer)
$tab_install.Controls.Add($lbl_install_allinone_info)
$tab_install.Controls.Add($Button4)
$tab_install.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]8.25,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$tab_install.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]4,[System.Int32]22))
$tab_install.Name = [System.String]'tab_install'
$tab_install.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]838,[System.Int32]464))
$tab_install.TabIndex = [System.Int32]2
$tab_install.Text = [System.String]'Install'
$tab_install.UseVisualStyleBackColor = $true
#
#lbl_install_prerequisities_info03
#
$lbl_install_prerequisities_info03.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]560,[System.Int32]334))
$lbl_install_prerequisities_info03.Name = [System.String]'lbl_install_prerequisities_info03'
$lbl_install_prerequisities_info03.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]252,[System.Int32]27))
$lbl_install_prerequisities_info03.TabIndex = [System.Int32]18
$lbl_install_prerequisities_info03.Text = [System.String]'The Hyper-V vitualization is needed. 
To enable Hyper-V'
#
#lnk_install_prerequisities_hyperv
#
$lnk_install_prerequisities_hyperv.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]560,[System.Int32]361))
$lnk_install_prerequisities_hyperv.Name = [System.String]'lnk_install_prerequisities_hyperv'
$lnk_install_prerequisities_hyperv.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]261,[System.Int32]52))
$lnk_install_prerequisities_hyperv.TabIndex = [System.Int32]17
$lnk_install_prerequisities_hyperv.TabStop = $true
$lnk_install_prerequisities_hyperv.Text = [System.String]'https://learn.microsoft.com/en-us/virtualization/hyper-v-on-windows/quick-start/enable-hyper-v'
#
#lbl_install_prerequisities
#
$lbl_install_prerequisities.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Courier New',[System.Single]8.25,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_install_prerequisities.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]560,[System.Int32]16))
$lbl_install_prerequisities.Name = [System.String]'lbl_install_prerequisities'
$lbl_install_prerequisities.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]252,[System.Int32]108))
$lbl_install_prerequisities.TabIndex = [System.Int32]16
$lbl_install_prerequisities.Text = [System.String]'Prerequisities:

Windows version .... Check
Hyper-V ............ Check
Chocolatey ......... Check
Multipass .......... Check
CPU and RAM ........ Check'
#
#lbl_install_prerequisities_info01
#
$lbl_install_prerequisities_info01.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]560,[System.Int32]132))
$lbl_install_prerequisities_info01.Name = [System.String]'lbl_install_prerequisities_info01'
$lbl_install_prerequisities_info01.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]252,[System.Int32]67))
$lbl_install_prerequisities_info01.TabIndex = [System.Int32]15
$lbl_install_prerequisities_info01.Text = [System.String]'To sucessfully install the SmartFace Evaluation DEMO on Windows, please ensure you have at least the Microsoft Windows 10 PRO or the newer. HOME versions are not supported.'
#
#lnk_install_prerequisities_multipass
#
$lnk_install_prerequisities_multipass.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]560,[System.Int32]300))
$lnk_install_prerequisities_multipass.Name = [System.String]'lnk_install_prerequisities_multipass'
$lnk_install_prerequisities_multipass.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]123,[System.Int32]23))
$lnk_install_prerequisities_multipass.TabIndex = [System.Int32]14
$lnk_install_prerequisities_multipass.TabStop = $true
$lnk_install_prerequisities_multipass.Text = [System.String]'https://multipass.run/'
#
#lnk_install_prerequisities_chocolatey
#
$lnk_install_prerequisities_chocolatey.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]560,[System.Int32]277))
$lnk_install_prerequisities_chocolatey.Name = [System.String]'lnk_install_prerequisities_chocolatey'
$lnk_install_prerequisities_chocolatey.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]158,[System.Int32]23))
$lnk_install_prerequisities_chocolatey.TabIndex = [System.Int32]13
$lnk_install_prerequisities_chocolatey.TabStop = $true
$lnk_install_prerequisities_chocolatey.Text = [System.String]'https://chocolatey.org/'
#
#lbl_install_prerequisities_info02
#
$lbl_install_prerequisities_info02.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]560,[System.Int32]205))
$lbl_install_prerequisities_info02.Name = [System.String]'lbl_install_prerequisities_info02'
$lbl_install_prerequisities_info02.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]235,[System.Int32]65))
$lbl_install_prerequisities_info02.TabIndex = [System.Int32]12
$lbl_install_prerequisities_info02.Text = [System.String]'To setup the SmartFace Evaluation DEMO your computer needs to have some prerequisities installed. This includes Chocolatey and Multipass. They are installed automatically.'
#
#lbl_install_yourcomputer
#
$lbl_install_yourcomputer.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]30,[System.Int32]415))
$lbl_install_yourcomputer.Name = [System.String]'lbl_install_yourcomputer'
$lbl_install_yourcomputer.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]455,[System.Int32]37))
$lbl_install_yourcomputer.TabIndex = [System.Int32]11
$lbl_install_yourcomputer.Text = [System.String]'Your computer has X CPU cores and Y GB of Ram. Please ensure your host computer has the resources available for the SmartFace Evaluation DEMO.
'
#
#lbl_install_allinone_info
#
$lbl_install_allinone_info.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]40,[System.Int32]52))
$lbl_install_allinone_info.Name = [System.String]'lbl_install_allinone_info'
$lbl_install_allinone_info.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]202,[System.Int32]162))
$lbl_install_allinone_info.TabIndex = [System.Int32]6
$lbl_install_allinone_info.Text = [System.String]'Evaluation preset for 4 cameras


All features available


Needs 8 core CPU
Needs 16GB Ram
Needs 40 GB of HDD space
'
#
#Button4
#
$Button4.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]699,[System.Int32]416))
$Button4.Name = [System.String]'Button4'
$Button4.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]133,[System.Int32]40))
$Button4.TabIndex = [System.Int32]2
$Button4.Text = [System.String]'Install SmartFace'
$Button4.UseVisualStyleBackColor = $true
$Button4.add_Click($Button4_Click)
#
#tab_license
#
$tab_license.Controls.Add($lbl_license_info2)
$tab_license.Controls.Add($btn_license_loginregistry)
$tab_license.Controls.Add($lbl_license_header2)
$tab_license.Controls.Add($lbl_license_info)
$tab_license.Controls.Add($lnk_license_customerportal)
$tab_license.Controls.Add($lbl_license_header1)
$tab_license.Controls.Add($btn_license_clipboard)
$tab_license.Controls.Add($txt_license_hwid)
$tab_license.Controls.Add($lbl_license_hwid)
$tab_license.Controls.Add($lnk_license_obtainlicense)
$tab_license.Controls.Add($lbl_license_password)
$tab_license.Controls.Add($txt_license_password)
$tab_license.Controls.Add($btn_license_selectlicensefile)
$tab_license.Controls.Add($txt_license_filepath)
$tab_license.Controls.Add($lbl_license_filepath)
$tab_license.Controls.Add($txt_license_username)
$tab_license.Controls.Add($lbl_license_username)
$tab_license.Controls.Add($btn_license_continue)
$tab_license.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]4,[System.Int32]22))
$tab_license.Name = [System.String]'tab_license'
$tab_license.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]838,[System.Int32]464))
$tab_license.TabIndex = [System.Int32]3
$tab_license.Text = [System.String]'Credentials and license'
$tab_license.UseVisualStyleBackColor = $true
#
#lbl_license_info2
#
$lbl_license_info2.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]24,[System.Int32]272))
$lbl_license_info2.Name = [System.String]'lbl_license_info2'
$lbl_license_info2.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]140,[System.Int32]63))
$lbl_license_info2.TabIndex = [System.Int32]17
$lbl_license_info2.Text = [System.String]'For more information and detailed instruction visit'
#
#btn_license_loginregistry
#
$btn_license_loginregistry.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]555,[System.Int32]157))
$btn_license_loginregistry.Name = [System.String]'btn_license_loginregistry'
$btn_license_loginregistry.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]112,[System.Int32]50))
$btn_license_loginregistry.TabIndex = [System.Int32]18
$btn_license_loginregistry.Text = [System.String]'Log in Registry'
#
#lbl_license_header2
#
$lbl_license_header2.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]8.25,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_license_header2.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]22,[System.Int32]218))
$lbl_license_header2.Name = [System.String]'lbl_license_header2'
$lbl_license_header2.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]779,[System.Int32]49))
$lbl_license_header2.TabIndex = [System.Int32]15
$lbl_license_header2.Text = [System.String]'2. Get a license

                 1. Copy Hardware ID  >  2. Visit Customer Portal  >  3. Generate License  >  4. Download the license  >  5. Select License file'

#
#lbl_license_info
#
$lbl_license_info.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]13))
$lbl_license_info.Name = [System.String]'lbl_license_info'
$lbl_license_info.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]793,[System.Int32]92))
$lbl_license_info.TabIndex = [System.Int32]14
$lbl_license_info.Text = [System.String]$resources.'lbl_license_info.Text'
#
#lnk_license_customerportal
#
$lnk_license_customerportal.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]105))
$lnk_license_customerportal.Name = [System.String]'lnk_license_customerportal'
$lnk_license_customerportal.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]449,[System.Int32]23))
$lnk_license_customerportal.TabIndex = [System.Int32]13
$lnk_license_customerportal.TabStop = $true
$lnk_license_customerportal.Text = [System.String]'https://customerportal.innovatrics.com/'
#
#lbl_license_header1
#
$lbl_license_header1.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]8.25,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_license_header1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]134))
$lbl_license_header1.Name = [System.String]'lbl_license_header1'
$lbl_license_header1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]296,[System.Int32]23))
$lbl_license_header1.TabIndex = [System.Int32]12
$lbl_license_header1.Text = [System.String]'1. Provide registry credentials. '
#
#btn_license_clipboard
#
$btn_license_clipboard.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]555,[System.Int32]338))
$btn_license_clipboard.Name = [System.String]'btn_license_clipboard'
$btn_license_clipboard.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]112,[System.Int32]52))
$btn_license_clipboard.TabIndex = [System.Int32]11
$btn_license_clipboard.Text = [System.String]'Copy to Clipboard'
$btn_license_clipboard.UseVisualStyleBackColor = $true
#
#txt_license_hwid
#
$txt_license_hwid.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]27.75,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$txt_license_hwid.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]130,[System.Int32]338))
$txt_license_hwid.Name = [System.String]'txt_license_hwid'
$txt_license_hwid.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]419,[System.Int32]52))
$txt_license_hwid.TabIndex = [System.Int32]10
#
#lbl_license_hwid
#
$lbl_license_hwid.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]24,[System.Int32]338))
$lbl_license_hwid.Name = [System.String]'lbl_license_hwid'
$lbl_license_hwid.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]100,[System.Int32]23))
$lbl_license_hwid.TabIndex = [System.Int32]9
$lbl_license_hwid.Text = [System.String]'Hardware ID'
#
#lnk_license_obtainlicense
#
$lnk_license_obtainlicense.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]170,[System.Int32]272))
$lnk_license_obtainlicense.Name = [System.String]'lnk_license_obtainlicense'
$lnk_license_obtainlicense.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]497,[System.Int32]23))
$lnk_license_obtainlicense.TabIndex = [System.Int32]8
$lnk_license_obtainlicense.TabStop = $true
$lnk_license_obtainlicense.Text = [System.String]'https://developers.innovatrics.com/smartface/docs/get-started/installation/obtain_license/'
#
#lbl_license_password
#
$lbl_license_password.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]24,[System.Int32]189))
$lbl_license_password.Name = [System.String]'lbl_license_password'
$lbl_license_password.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]100,[System.Int32]23))
$lbl_license_password.TabIndex = [System.Int32]7
$lbl_license_password.Text = [System.String]'Password'
#
#txt_license_password
#
$txt_license_password.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]130,[System.Int32]186))
$txt_license_password.Name = [System.String]'txt_license_password'
$txt_license_password.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]419,[System.Int32]21))
$txt_license_password.TabIndex = [System.Int32]6
#
#btn_license_selectlicensefile
#
$btn_license_selectlicensefile.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]555,[System.Int32]399))
$btn_license_selectlicensefile.Name = [System.String]'btn_license_selectlicensefile'
$btn_license_selectlicensefile.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]112,[System.Int32]30))
$btn_license_selectlicensefile.TabIndex = [System.Int32]5
$btn_license_selectlicensefile.Text = [System.String]'Select License file'
$btn_license_selectlicensefile.UseVisualStyleBackColor = $true
#
#txt_license_filepath
#
$txt_license_filepath.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]14.25,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$txt_license_filepath.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]130,[System.Int32]399))
$txt_license_filepath.Name = [System.String]'txt_license_filepath'
$txt_license_filepath.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]419,[System.Int32]30))
$txt_license_filepath.TabIndex = [System.Int32]4
#
#lbl_license_filepath
#
$lbl_license_filepath.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]22,[System.Int32]408))
$lbl_license_filepath.Name = [System.String]'lbl_license_filepath'
$lbl_license_filepath.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]100,[System.Int32]23))
$lbl_license_filepath.TabIndex = [System.Int32]3
$lbl_license_filepath.Text = [System.String]'License file path'
#
#txt_license_username
#
$txt_license_username.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]130,[System.Int32]159))
$txt_license_username.Name = [System.String]'txt_license_username'
$txt_license_username.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]419,[System.Int32]21))
$txt_license_username.TabIndex = [System.Int32]2
#
#lbl_license_username
#
$lbl_license_username.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]24,[System.Int32]162))
$lbl_license_username.Name = [System.String]'lbl_license_username'
$lbl_license_username.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]100,[System.Int32]23))
$lbl_license_username.TabIndex = [System.Int32]1
$lbl_license_username.Text = [System.String]'Username'
#
#btn_license_continue
#
$btn_license_continue.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]699,[System.Int32]416))
$btn_license_continue.Name = [System.String]'btn_license_continue'
$btn_license_continue.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]133,[System.Int32]40))
$btn_license_continue.TabIndex = [System.Int32]0
$btn_license_continue.Text = [System.String]'Continue'
$btn_license_continue.UseVisualStyleBackColor = $true
#
#tab_run
#
$tab_run.Controls.Add($btn_services_show)
$tab_run.Controls.Add($lbl_wmstatus)
$tab_run.Controls.Add($lbl_demoevaluationlinks)
$tab_run.Controls.Add($lbl_services_status_data)
$tab_run.Controls.Add($lbl_services_status)
$tab_run.Controls.Add($btn_services_status_stop)
$tab_run.Controls.Add($btn_services_status_start)
$tab_run.Controls.Add($lbl_data_ip_vm)
$tab_run.Controls.Add($btn_logintoterminal)
$tab_run.Controls.Add($lbl_demolink_restapi)
$tab_run.Controls.Add($lbl_demolink_graphql)
$tab_run.Controls.Add($lbl_demolink_station)
$tab_run.Controls.Add($lbl_demolink_station_data)
$tab_run.Controls.Add($lbl_demolink_restapi_data)
$tab_run.Controls.Add($lbl_demolink_graphql_data)
$tab_run.Controls.Add($lbl_wmstatus_data)
$tab_run.Controls.Add($Label22)
$tab_run.Controls.Add($btn_wmstatus_start)
$tab_run.Controls.Add($btn_wmstatus_stop)
$tab_run.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]4,[System.Int32]22))
$tab_run.Name = [System.String]'tab_run'
$tab_run.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]838,[System.Int32]464))
$tab_run.TabIndex = [System.Int32]5
$tab_run.Text = [System.String]'Run SmartFace'
$tab_run.UseVisualStyleBackColor = $true
#
#btn_services_show
#
$btn_services_show.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]644,[System.Int32]301))
$btn_services_show.Name = [System.String]'btn_services_show'
$btn_services_show.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]156,[System.Int32]44))
$btn_services_show.TabIndex = [System.Int32]20
$btn_services_show.Text = [System.String]'Show running services'
$btn_services_show.UseVisualStyleBackColor = $true
#
#lbl_wmstatus
#
$lbl_wmstatus.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_wmstatus.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]57,[System.Int32]31))
$lbl_wmstatus.Name = [System.String]'lbl_wmstatus'
$lbl_wmstatus.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]301,[System.Int32]31))
$lbl_wmstatus.TabIndex = [System.Int32]19
$lbl_wmstatus.Text = [System.String]'Virtual Machine Status'
#
#lbl_demoevaluationlinks
#
$lbl_demoevaluationlinks.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]8.25,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_demoevaluationlinks.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]45,[System.Int32]281))
$lbl_demoevaluationlinks.Name = [System.String]'lbl_demoevaluationlinks'
$lbl_demoevaluationlinks.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]522,[System.Int32]23))
$lbl_demoevaluationlinks.TabIndex = [System.Int32]18
$lbl_demoevaluationlinks.Text = [System.String]'To evaluate the DEMO, open the following links in your browser:'
#
#lbl_services_status_data
#
$lbl_services_status_data.BackColor = [System.Drawing.Color]::FromArgb(([System.Int32]([System.Byte][System.Byte]192)),([System.Int32]([System.Byte][System.Byte]255)),([System.Int32]([System.Byte][System.Byte]192)))

$lbl_services_status_data.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]36,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_services_status_data.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]45,[System.Int32]178))
$lbl_services_status_data.Name = [System.String]'lbl_services_status_data'
$lbl_services_status_data.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]563,[System.Int32]66))
$lbl_services_status_data.TabIndex = [System.Int32]17
$lbl_services_status_data.Text = [System.String]'Up'
#
#lbl_services_status
#
$lbl_services_status.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_services_status.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]57,[System.Int32]145))
$lbl_services_status.Name = [System.String]'lbl_services_status'
$lbl_services_status.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]344,[System.Int32]23))
$lbl_services_status.TabIndex = [System.Int32]16
$lbl_services_status.Text = [System.String]'SmartFace Services'
#
#btn_services_status_stop
#
$btn_services_status_stop.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]644,[System.Int32]188))
$btn_services_status_stop.Name = [System.String]'btn_services_status_stop'
$btn_services_status_stop.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]75,[System.Int32]44))
$btn_services_status_stop.TabIndex = [System.Int32]15
$btn_services_status_stop.Text = [System.String]'Stop Services'
$btn_services_status_stop.UseVisualStyleBackColor = $true
#
#btn_services_status_start
#
$btn_services_status_start.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]725,[System.Int32]188))
$btn_services_status_start.Name = [System.String]'btn_services_status_start'
$btn_services_status_start.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]75,[System.Int32]44))
$btn_services_status_start.TabIndex = [System.Int32]14
$btn_services_status_start.Text = [System.String]'Start Services'
$btn_services_status_start.UseVisualStyleBackColor = $true
#
#lbl_data_ip_vm
#
$lbl_data_ip_vm.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]628,[System.Int32]18))
$lbl_data_ip_vm.Name = [System.String]'lbl_data_ip_vm'
$lbl_data_ip_vm.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]190,[System.Int32]33))
$lbl_data_ip_vm.TabIndex = [System.Int32]12
$lbl_data_ip_vm.Text = [System.String]'IP Address:
VM Name: '
#
#btn_logintoterminal
#
$btn_logintoterminal.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]644,[System.Int32]351))
$btn_logintoterminal.Name = [System.String]'btn_logintoterminal'
$btn_logintoterminal.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]156,[System.Int32]44))
$btn_logintoterminal.TabIndex = [System.Int32]11
$btn_logintoterminal.Text = [System.String]'Log into Linux Terminal'
$btn_logintoterminal.UseVisualStyleBackColor = $true
#
#lbl_demolink_restapi
#
$lbl_demolink_restapi.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]15.75,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_demolink_restapi.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]45,[System.Int32]339))
$lbl_demolink_restapi.Name = [System.String]'lbl_demolink_restapi'
$lbl_demolink_restapi.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]236,[System.Int32]23))
$lbl_demolink_restapi.TabIndex = [System.Int32]10
$lbl_demolink_restapi.Text = [System.String]'SmartFace REST API'
$lbl_demolink_restapi.add_Click($Label25_Click)
#
#lbl_demolink_graphql
#
$lbl_demolink_graphql.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]15.75,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_demolink_graphql.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]45,[System.Int32]371))
$lbl_demolink_graphql.Name = [System.String]'lbl_demolink_graphql'
$lbl_demolink_graphql.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]245,[System.Int32]23))
$lbl_demolink_graphql.TabIndex = [System.Int32]9
$lbl_demolink_graphql.Text = [System.String]'SmartFace GraphQL API'
#
#lbl_demolink_station
#
$lbl_demolink_station.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]15.75,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_demolink_station.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]45,[System.Int32]307))
$lbl_demolink_station.Name = [System.String]'lbl_demolink_station'
$lbl_demolink_station.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]236,[System.Int32]23))
$lbl_demolink_station.TabIndex = [System.Int32]8
$lbl_demolink_station.Text = [System.String]'SmartFace Station'
#
#lbl_demolink_station_data
#
$lbl_demolink_station_data.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_demolink_station_data.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]375,[System.Int32]312))
$lbl_demolink_station_data.Name = [System.String]'lbl_demolink_station_data'
$lbl_demolink_station_data.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]232,[System.Int32]23))
$lbl_demolink_station_data.TabIndex = [System.Int32]7
$lbl_demolink_station_data.TabStop = $true
$lbl_demolink_station_data.Text = [System.String]'http://192.168.101.100:8000'
$lbl_demolink_station_data.TextAlign = [System.Drawing.ContentAlignment]::TopRight
#
#lbl_demolink_restapi_data
#
$lbl_demolink_restapi_data.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_demolink_restapi_data.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]375,[System.Int32]342))
$lbl_demolink_restapi_data.Name = [System.String]'lbl_demolink_restapi_data'
$lbl_demolink_restapi_data.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]232,[System.Int32]23))
$lbl_demolink_restapi_data.TabIndex = [System.Int32]6
$lbl_demolink_restapi_data.TabStop = $true
$lbl_demolink_restapi_data.Text = [System.String]'http://192.168.101.100:8098'
$lbl_demolink_restapi_data.TextAlign = [System.Drawing.ContentAlignment]::TopRight
#
#lbl_demolink_graphql_data
#
$lbl_demolink_graphql_data.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_demolink_graphql_data.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]375,[System.Int32]372))
$lbl_demolink_graphql_data.Name = [System.String]'lbl_demolink_graphql_data'
$lbl_demolink_graphql_data.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]233,[System.Int32]45))
$lbl_demolink_graphql_data.TabIndex = [System.Int32]21
$lbl_demolink_graphql_data.TabStop = $true
$lbl_demolink_graphql_data.Text = [System.String]'http://192.168.101.100:8098'
$lbl_demolink_graphql_data.TextAlign = [System.Drawing.ContentAlignment]::TopRight
#
#lbl_wmstatus_data
#
$lbl_wmstatus_data.BackColor = [System.Drawing.Color]::FromArgb(([System.Int32]([System.Byte][System.Byte]192)),([System.Int32]([System.Byte][System.Byte]255)),([System.Int32]([System.Byte][System.Byte]192)))

$lbl_wmstatus_data.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]36,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_wmstatus_data.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]45,[System.Int32]62))
$lbl_wmstatus_data.Name = [System.String]'lbl_wmstatus_data'
$lbl_wmstatus_data.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]563,[System.Int32]66))
$lbl_wmstatus_data.TabIndex = [System.Int32]4
$lbl_wmstatus_data.Text = [System.String]'Running'
#
#Label22
#
$Label22.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$Label22.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]45,[System.Int32]74))
$Label22.Name = [System.String]'Label22'
$Label22.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]346,[System.Int32]23))
$Label22.TabIndex = [System.Int32]3
$Label22.Text = [System.String]'Virtual Machine Status'
$Label22.add_Click($Label22_Click)
#
#btn_wmstatus_start
#
$btn_wmstatus_start.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]727,[System.Int32]74))
$btn_wmstatus_start.Name = [System.String]'btn_wmstatus_start'
$btn_wmstatus_start.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]75,[System.Int32]44))
$btn_wmstatus_start.TabIndex = [System.Int32]2
$btn_wmstatus_start.Text = [System.String]'Start'
$btn_wmstatus_start.UseVisualStyleBackColor = $true
#
#btn_wmstatus_stop
#
$btn_wmstatus_stop.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]644,[System.Int32]74))
$btn_wmstatus_stop.Name = [System.String]'btn_wmstatus_stop'
$btn_wmstatus_stop.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]77,[System.Int32]44))
$btn_wmstatus_stop.TabIndex = [System.Int32]0
$btn_wmstatus_stop.Text = [System.String]'Stop'
$btn_wmstatus_stop.UseVisualStyleBackColor = $true
#
#tab_uninstall
#
$tab_uninstall.Controls.Add($lbl_uninstall)
$tab_uninstall.Controls.Add($btn_uninstall)
$tab_uninstall.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]4,[System.Int32]22))
$tab_uninstall.Name = [System.String]'tab_uninstall'
$tab_uninstall.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]838,[System.Int32]464))
$tab_uninstall.TabIndex = [System.Int32]6
$tab_uninstall.Text = [System.String]'Uninstall'
$tab_uninstall.UseVisualStyleBackColor = $true
#
#lbl_uninstall
#
$lbl_uninstall.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]14.25,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_uninstall.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]167,[System.Int32]68))
$lbl_uninstall.Name = [System.String]'lbl_uninstall'
$lbl_uninstall.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]567,[System.Int32]268))
$lbl_uninstall.TabIndex = [System.Int32]1
$lbl_uninstall.Text = [System.String]$resources.'lbl_uninstall.Text'
#
#btn_uninstall
#
$btn_uninstall.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$btn_uninstall.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]303,[System.Int32]355))
$btn_uninstall.Name = [System.String]'btn_uninstall'
$btn_uninstall.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]243,[System.Int32]68))
$btn_uninstall.TabIndex = [System.Int32]0
$btn_uninstall.Text = [System.String]'Uninstall SmartFace Evaluation DEMO'
$btn_uninstall.UseVisualStyleBackColor = $true
$btn_uninstall.add_Click($Button7_Click)
#
#Form1
#
$Form1.ClientSize = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]859,[System.Int32]617))
$Form1.Controls.Add($tab_control01)
$Form1.Controls.Add($pnl_white)
$Form1.Icon = ([System.Drawing.Icon]$resources.'$this.Icon')
$Form1.MaximizeBox = $false
$Form1.Text = [System.String]'SmartFace Demo'
$Form1.TopMost = $true
$pnl_white.ResumeLayout($false)
([System.ComponentModel.ISupportInitialize]$img_SmartFaceLogo).EndInit()
$tab_control01.ResumeLayout($false)
$tab_install.ResumeLayout($false)
$tab_license.ResumeLayout($false)
$tab_license.PerformLayout()
$tab_run.ResumeLayout($false)
$tab_uninstall.ResumeLayout($false)
$Form1.ResumeLayout($false)
Add-Member -InputObject $Form1 -Name pnl_white -Value $pnl_white -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_documentation -Value $lbl_documentation -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_evaluationDemo -Value $lbl_evaluationDemo -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lnk_documentation -Value $lnk_documentation -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name img_SmartFaceLogo -Value $img_SmartFaceLogo -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name tab_control01 -Value $tab_control01 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name tab_install -Value $tab_install -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_install_prerequisities_info03 -Value $lbl_install_prerequisities_info03 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lnk_install_prerequisities_hyperv -Value $lnk_install_prerequisities_hyperv -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_install_prerequisities -Value $lbl_install_prerequisities -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_install_prerequisities_info01 -Value $lbl_install_prerequisities_info01 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lnk_install_prerequisities_multipass -Value $lnk_install_prerequisities_multipass -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lnk_install_prerequisities_chocolatey -Value $lnk_install_prerequisities_chocolatey -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_install_prerequisities_info02 -Value $lbl_install_prerequisities_info02 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_install_yourcomputer -Value $lbl_install_yourcomputer -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_install_allinone_info -Value $lbl_install_allinone_info -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name Button4 -Value $Button4 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name tab_license -Value $tab_license -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_license_info2 -Value $lbl_license_info2 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_license_loginregistry -Value $btn_license_loginregistry -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_license_header2 -Value $lbl_license_header2 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_license_info -Value $lbl_license_info -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lnk_license_customerportal -Value $lnk_license_customerportal -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_license_header1 -Value $lbl_license_header1 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_license_clipboard -Value $btn_license_clipboard -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name txt_license_hwid -Value $txt_license_hwid -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_license_hwid -Value $lbl_license_hwid -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lnk_license_obtainlicense -Value $lnk_license_obtainlicense -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_license_password -Value $lbl_license_password -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name txt_license_password -Value $txt_license_password -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_license_selectlicensefile -Value $btn_license_selectlicensefile -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name txt_license_filepath -Value $txt_license_filepath -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_license_filepath -Value $lbl_license_filepath -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name txt_license_username -Value $txt_license_username -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_license_username -Value $lbl_license_username -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_license_continue -Value $btn_license_continue -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name tab_run -Value $tab_run -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_services_show -Value $btn_services_show -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_wmstatus -Value $lbl_wmstatus -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_demoevaluationlinks -Value $lbl_demoevaluationlinks -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_services_status_data -Value $lbl_services_status_data -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_services_status -Value $lbl_services_status -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_services_status_stop -Value $btn_services_status_stop -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_services_status_start -Value $btn_services_status_start -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_data_ip_vm -Value $lbl_data_ip_vm -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_logintoterminal -Value $btn_logintoterminal -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_demolink_restapi -Value $lbl_demolink_restapi -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_demolink_graphql -Value $lbl_demolink_graphql -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_demolink_station -Value $lbl_demolink_station -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_demolink_station_data -Value $lbl_demolink_station_data -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_demolink_restapi_data -Value $lbl_demolink_restapi_data -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_demolink_graphql_data -Value $lbl_demolink_graphql_data -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_wmstatus_data -Value $lbl_wmstatus_data -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name Label22 -Value $Label22 -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_wmstatus_start -Value $btn_wmstatus_start -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_wmstatus_stop -Value $btn_wmstatus_stop -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name tab_uninstall -Value $tab_uninstall -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name lbl_uninstall -Value $lbl_uninstall -MemberType NoteProperty
Add-Member -InputObject $Form1 -Name btn_uninstall -Value $btn_uninstall -MemberType NoteProperty
}
. InitializeComponent
