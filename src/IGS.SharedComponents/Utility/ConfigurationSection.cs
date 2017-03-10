using System.Configuration;

namespace IGS.SharedComponents.Utility
{
	/// <summary>
	/// Wrapper Class for ConfigurationSection
	/// </summary>
	public class ConfigurationSection : System.Configuration.ConfigurationSection 
	{
		/// <summary>
		/// Create a new instance of  <see cref="ConfigurationSection"/>
		/// </summary>
		// ReSharper disable once EmptyConstructor
		public ConfigurationSection()
		{
			// Default instance generation 
		}

		/// <summary>
		/// Gets the name of the section
		/// </summary>
		/// <value>The name of the section</value>
		protected virtual string SectionName
		{
			get
			{
				return GetType().Assembly.GetName().Name;
			}
		}

		/// <summary>
		/// Connection string value
		/// </summary>
		/// <param name="connectionStringName"></param>
		/// <returns>The connection string based on connectionStringName</returns>
		/// <exception cref="System.Configuration.ConfigurationErrorsException">Thrown when the connection string is null or empty for the name provided</exception>
		protected string ConnectionString(string connectionStringName)
		{
			var connection = ConfigurationManager.ConnectionStrings[connectionStringName];

			if (connection == null || string.IsNullOrWhiteSpace(connection.ConnectionString))
			{
				throw new ConfigurationErrorsException(string.Format("Null or empty connection string for connection string name {0}", connectionStringName));
			}

			return connection.ConnectionString;
		}
	}		
}
