using System;
using System.Runtime.Remoting.Messaging;
using StructureMap;

namespace IGS.SharedComponents.Container
{
	/// <summary>
	/// The container scope is used to provide an <see cref="IContainer"/> to classes and methods that 
	/// cannot participate in the dependency injection. This is useful for short lived operation. 
	/// Nested Container per HTTP request is a better, lightweight way to scope services to an HTTP request.
	/// </summary>
	/// <code>
	/// using(var scope = new ContainerScope())
	/// {
	///   var myClass = scope.Container.GetInstance&lt;Foo&gt;();
	///   return myClass.GetSerializableValue();
	/// }
	/// </code>
	public sealed class ContainerScope : IDisposable
	{
		private static readonly string CurrentContainerKey = string.Format("Container-{0}", Guid.NewGuid().ToString("N"));
		private readonly string _key;
		private readonly IContainer _container;
		private readonly bool _owner;
		private bool _disposed;

		/// <summary>
		/// Create a new Container Scope with a container key
		/// </summary>
		public ContainerScope()
			: this(CurrentContainerKey)
		{
		}

		/// <summary>
		/// Create a new Container Scope with the specified Container Key
		/// </summary>
		/// <param name="key">Key is used to aces a logical call context data slot for an IContainer</param>
		public ContainerScope(string key)
		{
			_key = key;

			var wrapper = (ContainerWrapper)CallContext.LogicalGetData(_key);

			if (wrapper != null && wrapper.Container != null)
			{
				_container = wrapper.Container;
				_owner = false;
				return;
			}

			_container = Ioc.GetDisposableContainer();
			_owner = true;

			wrapper = new ContainerWrapper(_container);
			CallContext.LogicalSetData(CurrentContainerKey, wrapper);
		}

		/// <summary>
		/// Instance of <see cref="IContainer"/>
		/// </summary>
		public IContainer Container
		{
			get
			{
				return _container;
			}
		}

		/// <summary>
		/// True if <see cref="IContainer"/> already existed on the logical call context when the 
		/// <see cref="ContainerScope"/> was constructed
		/// </summary>
		public bool Enrolled
		{
			get
			{
				return !_owner;
			}
		}

		/// <summary>
		/// Disposes the ContainerScope if is the owner of the <see cref="IContainer"/>
		/// </summary>
		public void Dispose()
		{
			if (!_disposed)
			{
				if (_owner)
				{
					CallContext.FreeNamedDataSlot(_key);
					_container.Dispose();
				}

				_disposed = true;
			}
		}

		/// <summary>
		/// Wrapper around IContainer, usable by Remoting
		/// </summary>
		[Serializable]
		internal sealed class ContainerWrapper : MarshalByRefObject
		{
			[NonSerializedAttribute]
			internal readonly IContainer Container;

			internal ContainerWrapper(IContainer container)
			{
				Container = container;
			}
		}
	}
}
