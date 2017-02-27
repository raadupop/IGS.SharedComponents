using StructureMap;
using StructureMap.Graph;

namespace IGS.SharedComponents.Container
{
	/// <summary>
	/// The default StructureMap registry which will scan all of the IGS assemblies and load all <see cref="Registry"/>s found within those assemblies.
	/// </summary>
	public sealed class DefaultRegistry : Registry
	{
		/// <summary>
		/// Perform the scan, over the <see cref="Registry"/>s
		/// </summary>
		public DefaultRegistry()
		{
			Scan(OnScan);

			// TODO Scan(s => s.ExtensionAssemblies());
		}

		/// <summary>
		/// Called when StructureMap scans assemblies
		/// </summary>
		/// <param name="scanner">The <see cref="IAssemblyScanner"/></param>
		private static void OnScan(IAssemblyScanner scanner)
		{
			scanner.IgsAssemblies();
			scanner.LookForRegistries();
		}
	}
}
