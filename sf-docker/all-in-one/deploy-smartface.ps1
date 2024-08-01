# PowerShell script to deploy SmartFace on a virtual machine using Multipass

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
        multipass launch --name $vmName --cpus 2 --memory 4G --disk 20G
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

    # Mount the current directory into the VM
    $currentDir = (Get-Location).Path
    Write-Output "Mounting current directory $currentDir to the virtual machine..."
    multipass mount $currentDir ${vmName}:/home/ubuntu/smartface
    if ($LASTEXITCODE -ne 0) {
        #Write-Error "Failed to mount directory. Exiting."
        #Exit 1
    }
    Write-Output "Current directory mounted successfully."
}

# Function to log in to the Docker registry
function Docker-Login {
    Write-Output "Logging in to Docker registry..."
    $vmName = "smartface-vm"
    $passwordFilePath = "registrypass.txt"

    if (-Not (Test-Path $passwordFilePath)) {
        Write-Error "Password file $passwordFilePath not found. Exiting."
        Exit 1
    }

    multipass exec $vmName -- bash -c "cat /home/ubuntu/smartface/$passwordFilePath | sudo docker login registry.gitlab.com -u sf-distribution --password-stdin"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to log in to Docker registry. Exiting."
        Exit 1
    }
    Write-Output "Logged in to Docker registry successfully."
}

# Function to deploy SmartFace using Docker Compose
function Deploy-SmartFace {
    Write-Output "Deploying SmartFace using Docker Compose..."
    $vmName = "smartface-vm"

    # Ensure Docker service is running
    multipass exec $vmName -- bash -c "sudo systemctl start docker"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to start Docker service. Exiting."
        Exit 1
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
Install-Chocolatey
Install-Multipass
Prepare-VM
Docker-Login
Deploy-SmartFace

Write-Output "SmartFace platform deployment completed successfully!"