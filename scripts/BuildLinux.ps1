param()

[string] $Product = "Chordious"
[string] $Target = "Linux"

& "$PSScriptRoot\Build.ps1" -Product $Product -Target $Target -BuildArgs "-target:Publish -p:RuntimeIdentifier=linux-x64 -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:EnableCompressionInSingleFile=true"

& "$PSScriptRoot\TarRelease.ps1" -Product $Product -Target $Target
