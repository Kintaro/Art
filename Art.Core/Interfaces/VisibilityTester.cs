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
	public class VisibilityTester
	{
		/// <summary>
		/// 
		/// </summary>
		public Ray Ray;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="renderer"></param>
		/// <param name="sample"></param>
		/// <returns></returns>
		public Spectrum Transmittance (Scene scene, IRenderer renderer, Sample sample)
		{
			return renderer.Transmittance (scene, new RayDifferential (this.Ray), sample);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="renderer"></param>
		/// <param name="sample"></param>
		/// <returns></returns>
		public async Task<Spectrum> TransmittanceAsync (Scene scene, IRenderer renderer, Sample sample)
		{
			return await renderer.TransmittanceAsync (scene, new RayDifferential (this.Ray), sample);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public bool Unoccluded (Scene scene)
		{
			return !scene.IntersectP (this.Ray);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public async Task<bool> UnoccludedAsync (Scene scene)
		{
			return !await scene.IntersectPAsync (this.Ray);
		}
	}
}
