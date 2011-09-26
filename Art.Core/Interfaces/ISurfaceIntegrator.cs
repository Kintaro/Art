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
	public interface ISurfaceIntegrator : IIntegrator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="renderer"></param>
		/// <param name="ray"></param>
		/// <param name="isect"></param>
		/// <param name="sample"></param>
		/// <returns></returns>
		Spectrum Li (Scene scene, IRenderer renderer, RayDifferential ray, Intersection isect, Sample sample);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="renderer"></param>
		/// <param name="ray"></param>
		/// <param name="isect"></param>
		/// <param name="sample"></param>
		/// <returns></returns>
		Task<Spectrum> LiAsync (Scene scene, IRenderer renderer, RayDifferential ray, Intersection isect, Sample sample);
	}
}
