# PowerShell script to deploy SmartFace on a virtual machine using Multipass

# Function to check if Chocolatey is installed
function Install-Chocolatey {
    Write-Output "Checking for Chocolatey installation..."
    if (-Not (Get-Command choco.exe -ErrorAction SilentlyContinue)) {
        Write-Output "Chocolatey is not installed. Installing Chocolatey..."
        Set-ExecutionPolicy Bypass -Scope Process -Force;
        [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072;
        iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
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
        Write-Output "Multipass installed successfully."
    } else {
        Write-Output "Multipass is already installed."
    }
}

# Function to create and configure a virtual machine using Multipass
function Setup-VirtualMachine {
    Write-Output "Launching a virtual machine with Multipass..."
    $vmName = "smartface-vm"
    multipass launch --name $vmName --cpus 2 --mem 4G --disk 20G
    Write-Output "Virtual machine $vmName launched successfully."

    # Install Docker inside the VM
    Write-Output "Installing Docker inside the virtual machine..."
    multipass exec $vmName -- bash -c "sudo apt-get update && sudo apt-get install -y docker.io"
    Write-Output "Docker installed successfully inside the VM."

    # Copy the SmartFace directory into the VM
    Write-Output "Copying SmartFace files to the virtual machine..."
    multipass transfer ./* ${vmName}:/home/ubuntu/smartface
    Write-Output "SmartFace files transferred successfully."
}

# Function to deploy SmartFace using Docker Compose
function Deploy-SmartFace {
    Write-Output "Deploying SmartFace using Docker Compose..."
    $vmName = "smartface-vm"

    # Ensure Docker service is running
    multipass exec $vmName -- bash -c "sudo systemctl start docker"

    # Run the SmartFace Docker Compose setup
    multipass exec $vmName -- bash -c "cd /home/ubuntu/smartface/sf-docker/all-in-one && sudo bash run.sh"
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
Setup-VirtualMachine
Deploy-SmartFace

Write-Output "SmartFace platform deployment completed successfully!"