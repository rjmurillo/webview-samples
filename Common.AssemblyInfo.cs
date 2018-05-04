using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

[assembly: AssemblyCulture("")]

[assembly: AssemblyProduct("WebView Samples for Windows Forms and Windows Presentation Foundation")]
[assembly: AssemblyDescription("Samples for using WebView in Windows Forms and Windows Presentation Foundation")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
 [assembly: AssemblyConfiguration("Release")]
#endif

[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]
