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

[assembly: AssemblyVersion("0.1")]
[assembly: AssemblyFileVersion("0.1")]

// This attribute should be the SemanticVersion one.
[assembly: AssemblyInformationalVersion("0.1")]

[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en")]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#endif
#if RELEASE
[assembly: AssemblyConfiguration("RELEASE")]
#endif
