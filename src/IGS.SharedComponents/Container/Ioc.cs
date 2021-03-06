﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using StructureMap;

namespace IGS.SharedComponents.Container
{
	/// <summary>
	/// Mechanism for bootstrap StructureMap
	/// </summary>
	public static class Ioc
	{
		/// <summary>
		/// The profile name for extensions
		/// </summary>
		public const string ExtensionProfileName = "extensions";

		private static readonly object SyncRoot = new object();
		private static IContainer profileContainer;
		private static IContainer applicationContainer;

		/// <summary>
		/// Provide access to the root <see cref="IContainer"/> for the application. Any instance created by
		/// this container will be a singleton and live for the life of the application. This property is used for
		/// the application frameworks and should not be used outside of bootstrapping.
		/// </summary>
		internal static IContainer ApplicaitonContainer
		{
			get
			{
				lock (SyncRoot)
				{
					return profileContainer;
				}
			}
		}

		/// <summary>
		/// Configure StructureMap
		/// </summary>
		/// <param name="expresssionAction">The StructureMap Configuration Expression to initialize the application <see cref="IContainer"/></param>
		public static void Configure(Action<ConfigurationExpression> expresssionAction)
		{
			Configure(new StructureMap.Container(expresssionAction));
		}

		/// <summary>
		/// Configure StructureMap using existing Root <see cref="IContainer"/>. *Available for unit tests.
		/// </summary>
		/// <param name="container">An existing root <see cref="IContainer"/></param>
		public static void Configure(IContainer container)
		{
			lock (SyncRoot)
			{
				if (applicationContainer != null)
				{
					throw new InvalidOperationException("A root container has already been registered. Call Reset() to dispose the existing container before registering another to ensure proper cleanup and initialization.");
				}
			}

			applicationContainer = container;
			profileContainer = applicationContainer.GetProfile(ExtensionProfileName);
		}

		/// <summary>
		/// Check the validity of the StructureMap container
		/// </summary>
		public static void AssertConfigurationIsValid()
		{
			using (var container = ApplicaitonContainer.GetNestedContainer())
			{
				if (Debugger.IsAttached)
				{
					Trace.WriteLine(container.WhatDoIHave());
				}	

				container.AssertConfigurationIsValid();
			}
		}

		/// <summary>
		/// Create a new <see cref="IContainer"/>.  This container will be a nested container of the root container.  All instances created in 
		/// this container will be disposed when the container is disposed.  This method is only used when you need to ensure you never enroll 
		/// into an existing container.  In all other cases use <see cref="ContainerScope"/>
		/// </summary>
		/// <returns>A new IGS Container</returns>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1616:ElementReturnValueDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here.")]
		public static IContainer GetDisposableContainer()
		{
			return ApplicaitonContainer.GetNestedContainer();
		}
	}
}
