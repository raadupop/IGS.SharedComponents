using System;
using System.Linq;
using System.Reflection;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.TypeRules;

namespace IGS.SharedComponents.Container
{
	/// <summary>
	/// Finds all inheritors of a type or implementors of an interface 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <remarks>
	/// Similar to structure maps FindAllTypesFilter with a few life-cycle 
	/// management methods
	/// </remarks>
	public class CustomTypePluginConvention<T> : IRegistrationConvention
	{
		/// <summary>
		/// Create an instance of CustomTypePluginConvention
		/// </summary>
		public CustomTypePluginConvention()
		{
			PluginType = typeof(T);
			Name = type => null;
			AsSingleton = false;
		}

		/// <summary>
		/// The plug-in type
		/// </summary>
		protected Type PluginType { get; private set; }

		/// <summary>
		/// Specifies how instances should be named
		/// </summary>
		public Func<Type, string> Name { get; set; }

		/// <summary>
		/// Indicates if the types should be registered as singletons
		/// </summary>
		public bool AsSingleton { get; set; }

		/// <inheritdocs />
		public void ScanTypes(TypeSet types, Registry registry)
		{
			var type = Matches(types);

			if (type == null)
			{
				return;
			}

			var registerType = GetLeastSpecificButValidType(PluginType, type);
			var name = Name != null ? Name(type) : null;

			Register(registry, registerType, type, name);
		}

		/// <summary>
		/// Determine if the type should be registered as a plug-in of T
		/// </summary>
		/// <param name="types"></param>
		/// <returns>True if is a plugged type</returns>
		protected Type Matches(TypeSet types)
		{
			return types.FindTypes(TypeClassification.Concretes).FirstOrDefault(type => type.CanBeCastTo(PluginType));
		}

		// Taken right from structuremap 3.1 source code
		// https://github.com/structuremap/structuremap/blob/3.1/src/StructureMap/Graph/FindAllTypesFilter.cs
		private static Type GetLeastSpecificButValidType(Type pluginType, Type type)
		{
			if (pluginType.GetTypeInfo().IsGenericTypeDefinition && !type.IsOpenGeneric())
			{
				return type.FindFirstInterfaceThatCloses(pluginType);
			}

			return pluginType;
		}

		/// <summary>
		/// Register the type
		/// </summary>
		/// <param name="registry"></param>
		/// <param name="registerType"></param>
		/// <param name="type"></param>
		/// <param name="named"></param>
		protected virtual void Register(Registry registry, Type registerType, Type type, string named)
		{
			// Register the type as an implementer/plugged type of T
			var instance = AsSingleton
				               ? registry.For(registerType).Singleton().Add(type)
				               : registry.For(registerType).Add(type);

			if (!string.IsNullOrEmpty(named))
			{
				instance.Named(named);
			}
		}
	}
}
