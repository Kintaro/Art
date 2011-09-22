using System;
using System.IO;
using System.Reflection;

namespace Art.Core.PluginSystem
{
	/// <summary>
	///     This is the basic Plugin class. So far, a plugin holds track
	///     of it's name and a library handle. To use a plugin, the pluginclass
	///     has to inherit from Plugin and provide a Creation Method. Afterwards it
	///     can be used like this:
	///
	/// <code>
	///     ICamera cam = PluginManager.CreateCamera ("PerspectiveCamera");
	/// </code>
	///
	///     Therefore your class must contain this (again, the camera as an example):
	///
	/// <code>
	///     public class CameraPlugin : Plugin
	///     {
	///         delegate ICamera CreateCameraDelegate ();
	///         public CreateCameraDelegate CreateCamera;
	///     }
	/// </code>
	/// </summary>
	public class Plugin
	{
		/// <summary>
		///     The plugin's name
		/// </summary>
		private string name;

		/// <summary>
		///     The plugin's assembly
		/// </summary>
		private Assembly assembly;

		/// <summary>
		///     Initialize the plugin and it's name
		/// </summary>
		/// <param name="name">
		///     The plugin's name
		/// </param>
		public Plugin (string category, string name)
		{
			category = category.Trim ();
			name = name.Trim ();
			this.name = name;

			var currentDirectory = Directory.GetCurrentDirectory ();
			var files = Directory.GetFiles (currentDirectory);
			var assemblyPath = string.Empty;

			foreach (var file in files)
			{
				if (file.Contains (category + "." + name + ".dll"))
				{
					assemblyPath = file;
					break;
				}
			}

			this.assembly = Assembly.LoadFile (assemblyPath);
		}

		/// <summary>
		///     Retrieves a pointer to a method in the plugin's assembly
		/// </summary>
		/// <param name="methodName">
		///     The method's name
		/// </param>
		/// <returns>
		///     The method info
		/// </returns>
		public MethodInfo GetMethod (string methodName)
		{
			Type[] types = null;
			try
			{
				types = assembly.GetTypes ();
			}
			catch (ReflectionTypeLoadException e)
			{
				types = e.Types;
			}

			foreach (var type in types)
			{
				var typename = type.Name;
				if (typename.Contains ("`"))
					typename = type.Name.Substring (0, type.Name.IndexOf ("`"));
				if (typename == name)
				{
					return type.GetMethod (methodName);
				}
			}

			return null;
		}
	}
}