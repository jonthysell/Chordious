param(
    [string]$BumpPart = "patch",
    [string]$DirectoryBuildPropsFile = "src\Directory.Build.props",
    [string]$AppxManifestFile = "src\Chordious.WinStore\Package.appxmanifest",
    [string]$ChangelogFile = ".\CHANGELIST.md",
    [string]$UpdateXmlFile = ".\update.xml",
    [boolean]$Test = $False
)

# Adapted from https://gist.github.com/derantell/b8a4c87ea50177b900379df92a25de9d
function Bump-Version {
    param(
        [string]$Version,
        [string]$BumpPart
    )

    $Version -Match "^(\d+)\.(\d+)\.(\d+)" | Out-Null
    $major, $minor, $patch = [int]$matches[1], [int]$matches[2], [int]$matches[3]

    switch ($BumpPart) {
        "major" { $major += 1; $minor = 0; $patch = 0 } 
        "minor" { $minor += 1; $patch = 0 }
        "patch" { $patch += 1 }
    }

    return "$major.$minor.$patch"
}

[string] $RepoRoot = Resolve-Path "$PSScriptRoot\.."

$StartingLocation = Get-Location
Set-Location -Path $RepoRoot

Write-Host "Building release commit for publish..."
try
{
    [xml]$DirectoryBuildProps = Get-Content $DirectoryBuildPropsFile

    [string]$CurrentVersion = ([string]$DirectoryBuildProps.Project.PropertyGroup.Version).Trim()

    Write-Host "Current version: $CurrentVersion"

    [string]$NewVersion = Bump-Version $CurrentVersion $BumpPart

    Write-Host "New version: $NewVersion"

    Write-Host "Updating $DirectoryBuildPropsFile with new version..."
    ((Get-Content $DirectoryBuildPropsFile).Replace($CurrentVersion, $NewVersion)) | Set-Content $DirectoryBuildPropsFile

    Write-Host "Updating $AppxManifestFile with new version..."
    ((Get-Content $AppxManifestFile).Replace($CurrentVersion, $NewVersion)) | Set-Content $AppxManifestFile -Encoding 'utf8BOM'

    Write-Host "Updating $ChangelogFile with new version..."
    ((Get-Content $ChangelogFile).Replace("## next ##", "## v$NewVersion ##")) | Set-Content $ChangelogFile

    Write-Host "Updating $UpdateXmlFile with new version..."
    ((Get-Content $UpdateXmlFile).Replace($CurrentVersion, $NewVersion)) | Set-Content $UpdateXmlFile

    if (!$Test) {
        Write-Host "Creating release commit..."
        &git commit -a -m "Version v$NewVersion release"

        Write-Host "Tagging release commit..."
        &git tag v$NewVersion
    
        Write-Host "Pushing release commit..."
        &git push

        Write-Host "Pushing tags..."
        &git push --tags
    }
}
finally
{
    Set-Location -Path "$StartingLocation"
}
