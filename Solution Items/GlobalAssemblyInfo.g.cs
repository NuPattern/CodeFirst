using System;
using System.Reflection;
using System.Resources;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("NuPattern")]
[assembly: AssemblyProduct("NuPattern")]
[assembly: AssemblyCopyright("Copyright 2012, NuPattern under Apache 2.0 license.")]
[assembly: AssemblyTrademark("NuPattern")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion(ThisAssembly.VersionString)]
[assembly: AssemblyFileVersion(ThisAssembly.VersionString)]

// This attribute should be the SemanticVersion one.
[assembly: AssemblyInformationalVersion(ThisAssembly.VersionString)]

[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en")]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#endif
#if RELEASE
[assembly: AssemblyConfiguration("RELEASE")]
#endif

internal static class ThisAssembly
{
    public const string VersionString = "0.1";

    static ThisAssembly()
    {
        AssemblyVersion = new Version(VersionString);
    }

    public static Version AssemblyVersion { get; private set; }
}