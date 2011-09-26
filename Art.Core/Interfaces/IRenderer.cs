using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;
using Art.Core.Spectra;

namespace Art.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public interface IRenderer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		void Render (Scene scene);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="ray"></param>
		/// <param name="sample"></param>
		/// <param name="isect"></param>
		/// <param name="T"></param>
		/// <returns></returns>
		Spectrum Li (Scene scene, RayDifferential ray, Sample sample, Intersection isect = null, Spectrum T = null);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="ray"></param>
		/// <param name="sample"></param>
		/// <returns></returns>
		Spectrum Transmittance (Scene scene, RayDifferential ray, Sample sample);
	}
}
