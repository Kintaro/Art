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
	public abstract class ILight
	{
		/// <summary>
		/// 
		/// </summary>
		protected Transform LightToWorld;
		/// <summary>
		/// 
		/// </summary>
		protected Transform WorldToLight;
		/// <summary>
		/// 
		/// </summary>
		public readonly int NumberOfSamples;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="l2w"></param>
		/// <param name="ns"></param>
		protected ILight (Transform l2w, int ns = 1)
		{
			this.NumberOfSamples = ns;
			this.LightToWorld = new Transform (l2w);
			this.WorldToLight = new Transform (l2w.Inverse);
		}

		/// <summary>
		/// 
		/// </summary>
		public abstract bool IsDeltaLight { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public abstract Spectrum Power (Scene scene);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="wi"></param>
		/// <returns></returns>
		public abstract double Pdf (Point p, Vector wi);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="wi"></param>
		/// <returns></returns>
		public abstract Task<double> PdfAsync (Point p, Vector wi);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="pEpsilon"></param>
		/// <param name="ls"></param>
		/// <param name="time"></param>
		/// <param name="wi"></param>
		/// <param name="pdf"></param>
		/// <param name="vis"></param>
		/// <returns></returns>
		public abstract Spectrum SampleL (Point p, double pEpsilon, LightSample ls, double time, Vector wi, out double pdf, VisibilityTester vis);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="pEpsilon"></param>
		/// <param name="ls"></param>
		/// <param name="time"></param>
		/// <param name="wi"></param>
		/// <param name="pdf"></param>
		/// <param name="vis"></param>
		/// <returns></returns>
		public abstract Task<Spectrum> SampleLAsync (Point p, double pEpsilon, LightSample ls, double time, Vector wi, out double pdf, VisibilityTester vis);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="ls"></param>
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <param name="time"></param>
		/// <param name="ray"></param>
		/// <param name="Ns"></param>
		/// <param name="pdf"></param>
		/// <returns></returns>
		public abstract Spectrum SampleL (Scene scene, LightSample ls, double u1, double u2, double time, Ray ray, Normal Ns, out double pdf);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="ls"></param>
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <param name="time"></param>
		/// <param name="ray"></param>
		/// <param name="Ns"></param>
		/// <param name="pdf"></param>
		/// <returns></returns>
		public abstract Task<Spectrum> SampleLAsync (Scene scene, LightSample ls, double u1, double u2, double time, Ray ray, Normal Ns, out double pdf);
	}
}
