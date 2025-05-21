param()

[string] $Product = "Chordious"
[string] $Target = "WinArm64.Setup"

& "$PSScriptRoot\BuildSetup.ps1" -Product $Product -Target $Target -BuildArgs "-p:Platform=ARM64" -ProjectPath "src\$Product.WPF.Setup\$Product.WPF.Setup.wixproj"
