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
	public interface IVolumeIntegrator : IIntegrator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="renderer"></param>
		/// <param name="ray"></param>
		/// <param name="sample"></param>
		/// <param name="transmittance"></param>
		/// <returns></returns>
		Spectrum Li (Scene scene, IRenderer renderer, RayDifferential ray, Sample sample, Spectrum transmittance);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="renderer"></param>
		/// <param name="ray"></param>
		/// <param name="sample"></param>
		/// <param name="transmittance"></param>
		/// <returns></returns>
		Task<Spectrum> LiAsync (Scene scene, IRenderer renderer, RayDifferential ray, Sample sample, Spectrum transmittance);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="renderer"></param>
		/// <param name="ray"></param>
		/// <param name="sample"></param>
		/// <returns></returns>
		Spectrum Transmittance (Scene scene, IRenderer renderer, RayDifferential ray, Sample sample);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="renderer"></param>
		/// <param name="ray"></param>
		/// <param name="sample"></param>
		/// <returns></returns>
		Task<Spectrum> TransmittanceAsync (Scene scene, IRenderer renderer, RayDifferential ray, Sample sample);
	}
}
