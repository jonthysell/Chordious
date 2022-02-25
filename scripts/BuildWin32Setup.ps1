param()

[string] $Product = "Chordious"
[string] $Target = "Win32.Setup"

& "$PSScriptRoot\BuildSetup.ps1" -Product $Product -Target $Target -BuildArgs "-p:Platform=x86" -ProjectPath "src\$Product.WPF.Setup\$Product.WPF.Setup.wixproj"
