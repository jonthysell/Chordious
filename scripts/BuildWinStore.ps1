param()

[string] $Product = "Chordious"
[string] $Target = "WinStore"

& "$PSScriptRoot\BuildAppxBundle.ps1" -Product $Product -Target $Target
