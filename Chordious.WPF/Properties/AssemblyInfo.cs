using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows;

#if PORTABLE
[assembly: AssemblyTitle("ChordiousPortable")]
[assembly: AssemblyDescription("Self-contained Chordious executable.")]
#else
[assembly: AssemblyTitle("Chordious.WPF")]
[assembly: AssemblyDescription("Desktop UI for Chordious.")]
#endif

[assembly: AssemblyCopyright("Copyright © 2015-2018 Jon Thysell <http://jonthysell.com>")]

[assembly: NeutralResourcesLanguage("en")]

[assembly: ComVisible(false)]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly
)]
