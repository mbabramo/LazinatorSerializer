# Define target directory
$targetDir = "C:\Localsource"

# Clear the target directory
if (Test-Path $targetDir) {
    Get-ChildItem -Path $targetDir -File | Remove-Item -Force
} else {
    New-Item -ItemType Directory -Path $targetDir
}

# Define source directories
$sourceDirs = @(
    "C:\Users\Admin\Documents\GitHub\LazinatorSerializer\Lazinator.Collections\bin\Release",
    "C:\Users\Admin\Documents\GitHub\LazinatorSerializer\Lazinator\bin\Release",
    "C:\Users\Admin\Documents\GitHub\LazinatorSerializer\LazinatorGenerator\bin\Release"
)

foreach ($sourceDir in $sourceDirs) {
    # Copy all .nupkg files from source directory to target directory
    Get-ChildItem -Path $sourceDir -Filter *.nupkg -File | Copy-Item -Destination $targetDir

    # Copy all .snupkg files from source directory to target directory
    Get-ChildItem -Path $sourceDir -Filter *.snupkg -File | Copy-Item -Destination $targetDir
}

Write-Host "Clearing and copying of files completed successfully."
