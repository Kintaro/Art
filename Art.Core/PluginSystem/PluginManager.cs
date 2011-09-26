using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;
using Art.Core.Interfaces;
using Art.Core.Spectra;
using Art.Core.Tools;

namespace Art.Core.PluginSystem
{
	/// <summary>
	/// 
	/// </summary>
	public static class PluginManager
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="objectToWorld"></param>
		/// <param name="worldToObject"></param>
		/// <param name="reverseOrientation"></param>
		/// <param name="parameters"></param>
		/// <param name="floatTextures"></param>
		/// <param name="spectrumTextures"></param>
		/// <returns></returns>
		public static IShape CreateShape (string name, Transform objectToWorld, Transform worldToObject, bool reverseOrientation, ParameterSet parameters, Dictionary<string, ITexture<double>> floatTextures, Dictionary<string, ITexture<Spectrum>> spectrumTextures)
		{
			var plugin = new ShapePlugin (name);
			return plugin.CreateShape (objectToWorld, worldToObject, reverseOrientation, parameters, floatTextures, spectrumTextures);
		} 
	}
}
