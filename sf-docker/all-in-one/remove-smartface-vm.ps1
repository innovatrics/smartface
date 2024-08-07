# PowerShell script to remove the SmartFace virtual machine using Multipass

# Function to remove the virtual machine
function Remove-VirtualMachine {
    $vmName = "smartface-vm"

    Write-Output "Checking if virtual machine $vmName exists..."
    $vmExists = multipass list | Select-String -Pattern $vmName

    if ($vmExists) {
        Write-Output "Stopping the virtual machine $vmName..."
        multipass stop $vmName

        Write-Output "Deleting the virtual machine $vmName..."
        multipass delete $vmName

        Write-Output "Purging the virtual machine $vmName from Multipass..."
        multipass purge

        Write-Output "Virtual machine $vmName removed successfully."
    } else {
        Write-Output "Virtual machine $vmName does not exist."
    }
}

# Main script execution
Remove-VirtualMachine

Write-Output "Virtual machine removal process completed!"

# Prompt the user for input
$userInput = Read-Host "Please press enter to exit"
