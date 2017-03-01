using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("IGS.SharedComponents")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Integrated Global Services")]
[assembly: AssemblyProduct("IGS.SharedComponents")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("aaec7d3d-10e0-4aa8-b454-d3a814423579")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0")]
[assembly: AssemblyFileVersion("1.0.0")]

// Restricting some of the Ioc methods to prevent propagation of life-cycle
// defects.  Most of the Ioc methods should only be used by framework code in these
// assemblies
[assembly: InternalsVisibleTo("IGS.Web.Api")]
[assembly: InternalsVisibleTo("IGS.Web.Api.Owin")]

// Make the internal classes accessible to StructureMap
[assembly: InternalsVisibleTo("StructureMap")]