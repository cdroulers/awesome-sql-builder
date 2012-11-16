<#    
    .PARAMETER KeyFile
    Path to NuGet key file.
    DEFAULT: ".\.nuget\key.txt"
    .PARAMETER Version
    Which version to push (will use it to list packages in the PackagesPath)
    .PARAMETER NuGetPath
    The path to the nuget.exe executable.
    DEFAULT: ".\.nuget\nuget.exe"
    .PARAMETER PackagesPath
    The path to the built packages folder.
    DEFAULT: ".\builds\nuget"
#>

PARAM(
    [String]
    $KeyFile = (Resolve-Path ".\.nuget\key.txt"),
    [String]
    $Version,
    [String]
    $NuGetPath = (Resolve-Path ".\.nuget\nuget.exe"),
    [String]
    $PackagesPath = (Resolve-Path ".\builds\nuget")
)
 
if (-not (Test-Path $KeyFile))
{
    throw "Could not find the NuGet access key at $KeyFile. Specify your file!!!"
}

if (-not (Test-Path $NuGetPath))
{
    throw "Nuget.exe not found at $NuGetPath";
}
if (-not (Test-Path $PackagesPath))
{
    throw "Packages folder not found at $PackagesPath";
}

pushd $PackagesPath

# get our secret key. This is not in the repository.
$key = Get-Content $KeyFile

# Find all the packages and display them for confirmation
$packages = dir "*.$version.nupkg"
"Packages to upload: "
$packages | % { write-host $_.Name }

# Ensure we haven't run this by accident.
$yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", "Uploads the packages."
$no = New-Object System.Management.Automation.Host.ChoiceDescription "&No", "Does not upload the packages."
$options = [System.Management.Automation.Host.ChoiceDescription[]]($no, $yes)

$result = $host.ui.PromptForChoice("Upload packages", "Do you want to upload the NuGet packages to the NuGet server?", $options, 0) 

if ($result -eq 0)
{
    "Upload aborted"
}
elseif($result -eq 1)
{
    $packages | % { 
        $package = $_.Name
        write-host "Uploading $package"
        & $NuGetPath push $package $key
        write-host ""
    }
}
popd