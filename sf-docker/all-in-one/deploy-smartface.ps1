# PowerShell script to deploy SmartFace on a virtual machine using Multipass
# Load Windows Forms assembly
Add-Type -AssemblyName System.Windows.Forms

# Function to reset the Multipass to have a fresh start
function Restart-Multipass {
	net stop multipass
	net start multipass
}

# Function to check if Chocolatey is installed
function Install-Chocolatey {
    Write-Output "Checking for Chocolatey installation..."
    if (-Not (Get-Command choco.exe -ErrorAction SilentlyContinue)) {
        Write-Output "Chocolatey is not installed. Installing Chocolatey..."
        Set-ExecutionPolicy Bypass -Scope Process -Force;
        [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072;
        iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Failed to install Chocolatey. Exiting."
            Exit 1
        }
        Write-Output "Chocolatey installed successfully."
    } else {
        Write-Output "Chocolatey is already installed."
    }
}

# Function to install Multipass using Chocolatey
function Install-Multipass {
    Write-Output "Checking for Multipass installation..."
    if (-Not (Get-Command multipass.exe -ErrorAction SilentlyContinue)) {
        Write-Output "Multipass is not installed. Installing Multipass..."
        choco install multipass -y
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Failed to install Multipass. Exiting."
            Exit 1
        }
        Write-Output "Multipass installed successfully."

        # Allow mounts in Multipass
        Write-Output "Allowing mounts in Multipass..."
        multipass set local.privileged-mounts=Yes
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Failed to set privileged mounts. Exiting."
            Exit 1
        }
    } else {
        Write-Output "Multipass is already installed."
    }
}

# Function to create and configure a virtual machine using Multipass
function Prepare-VM {
    $vmName = "smartface-vm"

    # Check if the VM already exists
    Write-Output "Checking if the virtual machine $vmName already exists..."
    $vmExists = multipass list | Select-String -Pattern $vmName

    if ($vmExists) {
        Write-Output "Virtual machine $vmName already exists. Checking its status..."
        $vmStatus = multipass info $vmName | Select-String -Pattern "State" | ForEach-Object { $_ -replace "State: ", "" }

        if ($vmStatus -eq "Running") {
            Write-Output "Virtual machine $vmName is already running. Skipping launch."
        } else {
            Write-Output "Virtual machine $vmName exists but is not running. Starting it..."
            multipass start $vmName
            if ($LASTEXITCODE -ne 0) {
                Write-Error "Failed to start existing virtual machine $vmName. Exiting."
                Exit 1
            }
        }
    } else {
        Write-Output "Launching a new virtual machine with Multipass..."
		Write-Output $vmName
        multipass launch --name $vmName --cpus 4 --memory 16G --disk 60G
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Failed to launch virtual machine. Exiting."
            Exit 1
        }
        Write-Output "Virtual machine $vmName launched successfully."
    }


    # Install Docker and Docker Compose inside the VM
    Write-Output "Installing Docker and Docker Compose inside the virtual machine..."
    multipass exec $vmName -- bash -c "sudo apt-get update && sudo apt-get install -y docker.io docker-compose-v2"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to install Docker and Docker Compose. Exiting."
        Exit 1
    }
    Write-Output "Docker and Docker Compose installed successfully inside the VM."

	#  Update docker group:
		# Create the docker group: 
	#multipass exec $vmName -- bash -c "sudo groupadd docker"
		# Add your user to the docker group: 
	#multipass exec $vmName -- bash -c "sudo usermod -aG docker $USER"
		#Run the following command to activate the changes to groups:
	#multipass exec $vmName -- bash -c "newgrp docker"

    # Mount the current directory into the VM
    $currentDir = (Get-Location).Path
    Write-Output "Mounting current directory $currentDir to the virtual machine..."
    multipass mount $currentDir ${vmName}:/home/ubuntu/smartface
    if ($LASTEXITCODE -ne 0) {
        # this needs to be updated
		#Write-Error "Failed to mount directory. Exiting."
        #Exit 1
    }
    Write-Output "Current directory mounted successfully."
}

# Function to log in to the Docker registry
function Docker-Login {

	while ($passwordFilePath -eq $null) {
		Write-Output "Logging in to Docker registry..."
		$vmName = "smartface-vm"
		$passwordFilePath = "registrypass.txt"

		if (-Not (Test-Path $passwordFilePath)) {
			Write-Host "Password file $passwordFilePath not found. "
			Write-Host "Requesting the password"

			$form = New-Object System.Windows.Forms.Form
			$form.Text = "Please enter the registry credentials"
			$form.Size = New-Object System.Drawing.Size(450,150)
			$form.FormBorderStyle = [System.Windows.Forms.FormBorderStyle]::FixedDialog
			$form.StartPosition = [System.Windows.Forms.FormStartPosition]::CenterScreen
			$form.ShowInTaskbar = $true
			$form.TopMost = $true
				# Create a label for the password field
			$label = New-Object System.Windows.Forms.Label
			$label.Text = "Password:"
			$label.Location = New-Object System.Drawing.Point(10,20)
			$label.AutoSize = $true
			$form.Controls.Add($label)
				# Create a textbox for the password input
			$passwordBox = New-Object System.Windows.Forms.TextBox
			$passwordBox.Location = New-Object System.Drawing.Point(100,18)
			$passwordBox.Size = New-Object System.Drawing.Size(300,20)
			$passwordBox.UseSystemPasswordChar = $true
			$form.Controls.Add($passwordBox)
				# Create an OK button
			$okButton = New-Object System.Windows.Forms.Button
			$okButton.Text = "OK"
			$okButton.Location = New-Object System.Drawing.Point(50,60)
			$okButton.DialogResult = [System.Windows.Forms.DialogResult]::OK
			$form.Controls.Add($okButton)
				# Create a Cancel button
			$cancelButton = New-Object System.Windows.Forms.Button
			$cancelButton.Text = "Cancel"
			$cancelButton.Location = New-Object System.Drawing.Point(150,60)
			$cancelButton.DialogResult = [System.Windows.Forms.DialogResult]::Cancel
			$form.Controls.Add($cancelButton)
				# Set the form's AcceptButton and CancelButton properties
			$form.AcceptButton = $okButton
			$form.CancelButton = $cancelButton
				# Show the form as a dialog and get the result
			$result = $form.ShowDialog()
				# If the user clicked OK, return the password
			if ($result -eq [System.Windows.Forms.DialogResult]::OK) {
				$password = $passwordBox.Text
				multipass exec $vmName -- bash -c "echo $password >> /home/ubuntu/smartface/$passwordFilePath"
				if ($LASTEXITCODE -ne 0) {
					Write-Error "Failed to save the registry credentials. Exiting."
					
				}

				Write-Output "Password written to file."

			} else {
				Write-Output "Operation cancelled."
			}
				# Clean up
			$form.Dispose()
			
		}

		multipass exec $vmName -- bash -c "cat /home/ubuntu/smartface/$passwordFilePath | sudo docker login registry.gitlab.com -u sf-distribution --password-stdin"
		if ($LASTEXITCODE -ne 0) {
			Write-Error "Failed to log in to Docker registry. Exiting."
			multipass exec $vmName -- bash -c "rm /home/ubuntu/smartface/$passwordFilePath"
			$passwordFilePath = $null
		}
	}
    Write-Output "Logged in to Docker registry successfully."
}

# Function to deploy SmartFace using Docker Compose
function Deploy-SmartFace {
    Write-Output "Deploying SmartFace using Docker Compose..."
    $vmName = "smartface-vm"

	$licenseFile = "iengine.lic"

    # Ensure Docker service is running
    multipass exec $vmName -- bash -c "sudo systemctl start docker"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to start Docker service. Exiting."
        Exit 1
    }

	# Check if iengine.lic file is present
	if (-Not (Test-Path $licenseFile)) {
		Write-Host "License file $licenseFile not found. "
		
		$currentDir = (Get-Location).Path

		# Run the License Manager
		multipass exec $vmName -- bash -c "sudo docker run registry.gitlab.com/innovatrics/smartface/license-manager:3.2.7 | grep 'Hardware ID of this device is:' | grep -o '[^ ]*$' | tr -d '\n' | sed 's/\.$//'  > hw_id.txt"
		
		if ($LASTEXITCODE -ne 0) {
			Write-Error "Failed to run the License Manager. Exiting."
			Exit 1
		}
		Write-Host "The VM Hardware ID:"
		multipass exec $vmName -- bash -c "cat hw_id.txt"
		if ($LASTEXITCODE -ne 0) {
			Write-Error "Failed to save the Hardware ID. Exiting."
			Exit 1
		}

			# Get the Value
		$hardwareID = Get-Content -Path "${currentDir}\hw_id.txt" -Raw
		if ($LASTEXITCODE -ne 0) {
			Write-Error "Failed to read the Hardware ID. Exiting."
			Exit 1
		}

		$HardwareIDForm = New-Object -TypeName System.Windows.Forms.Form
		[System.Windows.Forms.Button]$Button1 = $null
		[System.Windows.Forms.Button]$CopyButton = $null
		[System.Windows.Forms.TextBox]$TextBox1 = $null
		[System.Windows.Forms.Label]$Label1 = $null
		[System.Windows.Forms.LinkLabel]$LinkLabel1 = $null
		[System.Windows.Forms.Label]$Label2 = $null
		function InitializeComponent
		{
		$Button1 = (New-Object -TypeName System.Windows.Forms.Button)
		$CopyButton = New-Object -TypeName System.Windows.Forms.Button
		$TextBox1 = (New-Object -TypeName System.Windows.Forms.TextBox)
		$Label1 = (New-Object -TypeName System.Windows.Forms.Label)
		$LinkLabel1 = (New-Object -TypeName System.Windows.Forms.LinkLabel)
		$Label2 = (New-Object -TypeName System.Windows.Forms.Label)
		$HardwareIDForm.SuspendLayout()
		#
		#Button1
		#
		$Button1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]154,[System.Int32]137))
		$Button1.Name = [System.String]'Button1'
		$Button1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]112,[System.Int32]42))
		$Button1.TabIndex = [System.Int32]0
		$Button1.Text = [System.String]'Get License'
		$Button1.UseVisualStyleBackColor = $true
		$Button1.add_Click({
			#$global:readytogetlicense = $true
			$readytogetlicense = $true
			Write-Host $readytogetlicense
			$HardwareIDForm.Close()
		})
		
		# CopyButton
		$CopyButton.Location = New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12, [System.Int32]137)
		$CopyButton.Name = 'CopyButton'
		$CopyButton.Size = New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]112, [System.Int32]42)
		$CopyButton.TabIndex = 5
		$CopyButton.Text = 'Copy HW ID to Clipboard'
		$CopyButton.UseVisualStyleBackColor = $true
		$CopyButton.add_Click({
			[System.Windows.Forms.Clipboard]::SetText($TextBox1.Text)
			[System.Windows.Forms.MessageBox]::Show("Copied to clipboard")
		})
		
		#
		#TextBox1
		#
		$TextBox1.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]20.25,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
		$TextBox1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]42))
		$TextBox1.Name = [System.String]'TextBox1'
		$TextBox1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]300,[System.Int32]40))
		$TextBox1.TabIndex = [System.Int32]1
		$TextBox1.Text = $hardwareID
		#
		#Label1
		#
		$Label1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]16))
		$Label1.Name = [System.String]'Label1'
		$Label1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]100,[System.Int32]23))
		$Label1.TabIndex = [System.Int32]2
		$Label1.Text = [System.String]'Hardware ID'
		$Label1.add_Click($Label1_Click)
		#
		#LinkLabel1
		#
		$LinkLabel1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]111))
		$LinkLabel1.Name = [System.String]'LinkLabel1'
		$LinkLabel1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]254,[System.Int32]23))
		$LinkLabel1.TabIndex = [System.Int32]3
		$LinkLabel1.TabStop = $true
		$LinkLabel1.Text = [System.String]'https://customerportal.innovatrics.com/'

		$LinkLabel1.add_LinkClicked({
			Start-Process $LinkLabel1.Text
		})

		#
		#Label2
		#
		$Label2.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]88))
		$Label2.Name = [System.String]'Label2'
		$Label2.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]254,[System.Int32]23))
		$Label2.TabIndex = [System.Int32]4
		$Label2.Text = [System.String]'the Visit Customer Portal to get the License'
		#
		#Hardware ID
		#
		$HardwareIDForm.ClientSize = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]324,[System.Int32]240))
		$HardwareIDForm.ControlBox = $true
		$HardwareIDForm.Controls.Add($Label2)
		$HardwareIDForm.Controls.Add($LinkLabel1)
		$HardwareIDForm.Controls.Add($Label1)
		$HardwareIDForm.Controls.Add($TextBox1)
		$HardwareIDForm.Controls.Add($Button1)
		$HardwareIDForm.Controls.Add($CopyButton)
		$HardwareIDForm.MaximizeBox = $false
		$HardwareIDForm.MinimizeBox = $false
		$HardwareIDForm.StartPosition = [System.Windows.Forms.FormStartPosition]::CenterScreen
		$HardwareIDForm.TopMost = $true
		$HardwareIDForm.ShowInTaskbar = $true
		$HardwareIDForm.Name = [System.String]'HardwareID'
		$HardwareIDForm.Text = [System.String]'Hardware ID - Get License'
		$HardwareIDForm.ResumeLayout($false)
		$HardwareIDForm.PerformLayout()
		Add-Member -InputObject $HardwareIDForm -Name Button1 -Value $Button1 -MemberType NoteProperty
		Add-Member -InputObject $HardwareIDForm-Name TextBox1 -Value $TextBox1 -MemberType NoteProperty
		Add-Member -InputObject $HardwareIDForm -Name Label1 -Value $Label1 -MemberType NoteProperty
		Add-Member -InputObject $HardwareIDForm -Name LinkLabel1 -Value $LinkLabel1 -MemberType NoteProperty
		Add-Member -InputObject $HardwareIDForm -Name Label2 -Value $Label2 -MemberType NoteProperty
		Add-Member -InputObject $HardwareIDForm -Name CopyButton -Value $CopyButton -MemberType NoteProperty
	
		}
		. InitializeComponent
		$HardwareIDForm.ShowDialog()	
			
		# Request iengine.lic file
			# Create an OpenFileDialog object
		
		$OpenFileDialog = New-Object System.Windows.Forms.OpenFileDialog
			# Set dialog properties (optional)
		$OpenFileDialog.InitialDirectory = $currentDir
		$OpenFileDialog.Filter = "SmartFace License files (*.lic)| *.lic"
		$OpenFileDialog.FilterIndex = 1
		$OpenFileDialog.Multiselect = $false
			# Show the dialog
		$DialogResult = $OpenFileDialog.ShowDialog()
			# Check if the user selected a file
		if ($DialogResult -eq [System.Windows.Forms.DialogResult]::OK) {
			# Get the selected file
			$SelectedFile = $OpenFileDialog.FileName
			Write-Output "Selected file: $SelectedFile"
			
		# Set iengine.lic file	
			# Check if a file was selected
			if ($SelectedFile -ne $null) {
				# Transfer the license file
				multipass transfer $SelectedFile ${vmName}:/home/ubuntu/smartface/iengine.lic
			}		
			
		} else {
			Write-Output "No file selected."
		}
	
	}
	
    # Run the SmartFace Docker Compose setup
    multipass exec $vmName -- bash -c "cd /home/ubuntu/smartface/ && sudo bash run.sh"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to deploy SmartFace. Exiting."
        Exit 1
    }
    Write-Output "SmartFace deployed successfully."
}

# Check if running as administrator
function Test-IsAdmin {
    $currentUser = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
    return $currentUser.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

# Relaunch as admin if not already
if (-not (Test-IsAdmin)) {
    Write-Output "Administrator privileges are missing..."
    Exit
}

# Main script execution
Restart-Multipass
Install-Chocolatey
Install-Multipass
Prepare-VM
Docker-Login
Deploy-SmartFace

$output = multipass info smartface-vm
Write-Output $output

# Extract the line starting with "IPv4"
$ipv4_line = $output -split "`n" | Where-Object { $_ -match "^IPv4" }

# Extract the first IP address from the line using regex
if ($ipv4_line -match "(\d{1,3}\.){3}\d{1,3}") {
    $first_ip = $matches[0]
}

# Show links for the SmartFace Station, SmartFace REST API and SmartFace GraphQL API

Write-Output "SmartFace platform deployment completed successfully!"

$formFinal = New-Object -TypeName System.Windows.Forms.Form
[System.Windows.Forms.PictureBox]$PictureBox1 = $null
[System.Windows.Forms.LinkLabel]$lnk_sfstation = $null
[System.Windows.Forms.Label]$lbl_sfstation = $null
[System.Windows.Forms.Label]$lbl_restapi = $null
[System.Windows.Forms.Label]$lbl_graphql = $null
[System.Windows.Forms.LinkLabel]$lnk_restapi = $null
[System.Windows.Forms.LinkLabel]$lnk_graphql = $null
[System.Windows.Forms.Label]$lbl_demo = $null
function InitializeComponent
{
$resources = . (Join-Path $PSScriptRoot 'winforms\deploy.resources.ps1')
$PictureBox1 = (New-Object -TypeName System.Windows.Forms.PictureBox)
$lnk_sfstation = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lbl_sfstation = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_restapi = (New-Object -TypeName System.Windows.Forms.Label)
$lbl_graphql = (New-Object -TypeName System.Windows.Forms.Label)
$lnk_restapi = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lnk_graphql = (New-Object -TypeName System.Windows.Forms.LinkLabel)
$lbl_demo = (New-Object -TypeName System.Windows.Forms.Label)
([System.ComponentModel.ISupportInitialize]$PictureBox1).BeginInit()
$formFinal.SuspendLayout()
#
#PictureBox1
#
$PictureBox1.BackgroundImage = ([System.Drawing.Image]$resources.'PictureBox1.BackgroundImage')
$PictureBox1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]21,[System.Int32]28))
$PictureBox1.Name = [System.String]'PictureBox1'
$PictureBox1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]110,[System.Int32]110))
$PictureBox1.TabIndex = [System.Int32]0
$PictureBox1.TabStop = $false
#
#lnk_sfstation
#
$lnk_sfstation.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lnk_sfstation.LinkColor = [System.Drawing.Color]::White
$lnk_sfstation.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]199,[System.Int32]155))
$lnk_sfstation.Name = [System.String]'lnk_sfstation'
$lnk_sfstation.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]271,[System.Int32]30))
$lnk_sfstation.TabIndex = [System.Int32]1
$lnk_sfstation.TabStop = $true
$lnk_sfstation.Text = "http://${first_ip}:8000"
$lnk_sfstation.add_LinkClicked({
	Start-Process $lnk_sfstation.Text
})

#
#lbl_sfstation
#
$lbl_sfstation.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_sfstation.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]155))
$lbl_sfstation.Name = [System.String]'lbl_sfstation'
$lbl_sfstation.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]181,[System.Int32]23))
$lbl_sfstation.TabIndex = [System.Int32]2
$lbl_sfstation.Text = [System.String]'SmartFace Station'
#
#lbl_restapi
#
$lbl_restapi.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_restapi.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]189))
$lbl_restapi.Name = [System.String]'lbl_restapi'
$lbl_restapi.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]181,[System.Int32]23))
$lbl_restapi.TabIndex = [System.Int32]3
$lbl_restapi.Text = [System.String]'SmartFace Rest API'
#
#lbl_graphql
#
$lbl_graphql.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_graphql.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]223))
$lbl_graphql.Name = [System.String]'lbl_graphql'
$lbl_graphql.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]181,[System.Int32]23))
$lbl_graphql.TabIndex = [System.Int32]4
$lbl_graphql.Text = [System.String]'SmartFace GraphQL'
#
#lnk_restapi
#
$lnk_restapi.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lnk_restapi.LinkColor = [System.Drawing.Color]::White
$lnk_restapi.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]199,[System.Int32]189))
$lnk_restapi.Name = [System.String]'lnk_restapi'
$lnk_restapi.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]271,[System.Int32]23))
$lnk_restapi.TabIndex = [System.Int32]5
$lnk_restapi.TabStop = $true
$lnk_restapi.Text = "http://${first_ip}:8098"
$lnk_restapi.add_LinkClicked({
	Start-Process $lnk_restapi.Text
})
#
#lnk_graphql
#
$lnk_graphql.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]12,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lnk_graphql.LinkColor = [System.Drawing.Color]::White
$lnk_graphql.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]199,[System.Int32]223))
$lnk_graphql.Name = [System.String]'lnk_graphql'
$lnk_graphql.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]271,[System.Int32]23))
$lnk_graphql.TabIndex = [System.Int32]6
$lnk_graphql.TabStop = $true
$lnk_graphql.Text = "http://${first_ip}:8097"
$lnk_graphql.add_LinkClicked({
	Start-Process $lnk_graphql.Text
})
#
#lbl_demo
#
$lbl_demo.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Tahoma',[System.Single]36,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$lbl_demo.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]229,[System.Int32]59))
$lbl_demo.Name = [System.String]'lbl_demo'
$lbl_demo.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]202,[System.Int32]56))
$lbl_demo.TabIndex = [System.Int32]7
$lbl_demo.Text = [System.String]'DEMO'
#
#formFinal
#
$formFinal.BackColor = [System.Drawing.Color]::FromArgb(([System.Int32]([System.Byte][System.Byte]56)),([System.Int32]([System.Byte][System.Byte]66)),([System.Int32]([System.Byte][System.Byte]80)))

$formFinal.ClientSize = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]495,[System.Int32]276))
$formFinal.Controls.Add($lbl_demo)
$formFinal.Controls.Add($lnk_graphql)
$formFinal.Controls.Add($lnk_restapi)
$formFinal.Controls.Add($lbl_graphql)
$formFinal.Controls.Add($lbl_restapi)
$formFinal.Controls.Add($lbl_sfstation)
$formFinal.Controls.Add($lnk_sfstation)
$formFinal.Controls.Add($PictureBox1)
$formFinal.ForeColor = [System.Drawing.Color]::White
$formFinal.MaximizeBox = $false
$formFinal.MinimizeBox = $false
$formFinal.Name = [System.String]'formFinal'
$formFinal.StartPosition = [System.Windows.Forms.FormStartPosition]::CenterScreen
$formFinal.TopMost = $true
$formFinal.ShowInTaskbar = $true
$formFinal.Text = [System.String]'SmartFace Demo'
([System.ComponentModel.ISupportInitialize]$PictureBox1).EndInit()
$formFinal.ResumeLayout($false)
Add-Member -InputObject $formFinal -Name PictureBox1 -Value $PictureBox1 -MemberType NoteProperty
Add-Member -InputObject $formFinal -Name lnk_sfstation -Value $lnk_sfstation -MemberType NoteProperty
Add-Member -InputObject $formFinal -Name lbl_sfstation -Value $lbl_sfstation -MemberType NoteProperty
Add-Member -InputObject $formFinal -Name lbl_restapi -Value $lbl_restapi -MemberType NoteProperty
Add-Member -InputObject $formFinal -Name lbl_graphql -Value $lbl_graphql -MemberType NoteProperty
Add-Member -InputObject $formFinal -Name lnk_restapi -Value $lnk_restapi -MemberType NoteProperty
Add-Member -InputObject $formFinal -Name lnk_graphql -Value $lnk_graphql -MemberType NoteProperty
Add-Member -InputObject $formFinal -Name lbl_demo -Value $lbl_demo -MemberType NoteProperty
}
. InitializeComponent
$formFinal.ShowDialog()
