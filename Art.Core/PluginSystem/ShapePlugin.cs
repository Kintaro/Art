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
	///		Plugin for all kinds of shapes
	/// </summary>
	public class ShapePlugin : Plugin
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="objectToWorld"></param>
		/// <param name="worldToObject"></param>
		/// <param name="reverse"></param>
		/// <param name="paramSet"></param>
		/// <param name="floatTextures"></param>
		/// <param name="spectrumTextures"></param>
		/// <returns></returns>
		public delegate IShape CreateShapeDelegate (Transform objectToWorld, Transform worldToObject, bool reverse, ParameterSet paramSet, Dictionary<string, ITexture<double>> floatTextures, Dictionary<string, ITexture<Spectrum>> spectrumTextures);
		/// <summary>
		/// 
		/// </summary>
		public CreateShapeDelegate CreateShape;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ShapePlugin (string name)
			: base ("Shapes", name)
		{
			var methodInfo = GetMethod ("CreateShape");
			this.CreateShape = Delegate.CreateDelegate (typeof (CreateShapeDelegate), methodInfo) as CreateShapeDelegate;
		}
	}
}
