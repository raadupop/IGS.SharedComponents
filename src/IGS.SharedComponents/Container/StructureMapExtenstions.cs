using System;
using System.IO;
using System.Reflection;
using StructureMap.Graph;

namespace IGS.SharedComponents.Container
{
	/// <summary>
	/// Helpers for common structure map operation
	/// </summary>
	public static class StructureMapExtenstions
	{
		private const string ExtensionAssemblyMatchString = ".extension.dll";
		private const string ExtensionsAssemblyMatchString = ".extension.dll";
		private const string ExtensionsBinPath = "bin\\ext";
		private const string ExtenstionsDirName = "ext";

		/// <summary>
		/// Scan for all of the IGS assemblies into the base directory
		/// </summary>
		/// <param name="scanner"></param>
		public static void IgsAssemblies(this IAssemblyScanner scanner)
		{
			scanner.AssembliesAndExecutablesFromApplicationBaseDirectory(IsIgsAssembly);
		}

		/// <summary>
		/// Check if is a valid IGS assembly
		/// </summary>
		/// <param name="assembly">The assembly to verify</param>
		/// <returns>True if is a valid assembly</returns>
		internal static bool IsIgsAssembly(Assembly assembly)
		{
			return assembly.FullName.ToLowerInvariant().Contains("igs") && !IsExtensionAssembly(assembly);
		}

		/// <summary>
		/// Check if the assembly is a valid extension
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns>True if ends with .extension.dll or extensions.dll and is located under the /bin/ext directory</returns>
		internal static bool IsExtensionAssembly(Assembly assembly)
		{
			return (assembly.ManifestModule.Name.ToLowerInvariant().EndsWith(ExtensionAssemblyMatchString) ||
							assembly.ManifestModule.Name.ToLowerInvariant().EndsWith(ExtensionsAssemblyMatchString)) &&
						 assembly.CodeBase.ToLowerInvariant().Contains("/ext/");
		}

		/// <summary>
		/// Include all assemblies from the bin\ext directory into the scanning operation
		/// </summary>
		/// <param name="scanner"></param>
		public static void ExtensionAssemblies(this IAssemblyScanner scanner)
		{
			var extensionDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ExtensionsBinPath);

			// based on the build configuration (Debug/Release), we end up with bin/ext or bin/<config>/ext
			if (!Directory.Exists(extensionDir))
			{
				extensionDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ExtenstionsDirName);
			}

			if (Directory.Exists(extensionDir))
			{
				scanner.AssembliesFromPath(extensionDir, IsExtensionAssembly);

				// TODO: implement FindExtensibleRegistryInstancesScanner
				// scanner.With(new FindExtensibleRegistryInstancesScanner());
			}
		}

		/// <summary>
		/// Scans for implementers or inheritors of T and registers them
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="scanner"></param>
		public static void WithPluginConvention<T>(this IAssemblyScanner scanner)
		{
			scanner.With(new CustomTypePluginConvention<T>());
		}
	}
}
