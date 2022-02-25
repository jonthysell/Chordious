param()

[string] $Product = "Chordious"
[string] $Target = "Win64.Setup"

& "$PSScriptRoot\BuildSetup.ps1" -Product $Product -Target $Target -BuildArgs "-p:Platform=x64" -ProjectPath "src\$Product.WPF.Setup\$Product.WPF.Setup.wixproj"
