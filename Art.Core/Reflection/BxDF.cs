using System;
using Art.Core.Geometry;
using Art.Core.Spectra;

namespace Art.Core.Reflection
{
	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum BxDFType : byte
	{
		BSDF_REFLECTION = 1 << 0,
		BSDF_TRANSMISSION = 1 << 1,
		BSDF_DIFFUSE = 1 << 2,
		BSDF_GLOSSY = 1 << 3,
		BSDF_SPECULAR = 1 << 4,
		BSDF_ALL_TYPES = BSDF_DIFFUSE | BSDF_GLOSSY | BSDF_SPECULAR,
		BSDF_ALL_REFLECTION = BSDF_REFLECTION | BSDF_ALL_TYPES,
		BSDF_ALL_TRANSMISSION = BSDF_TRANSMISSION | BSDF_ALL_TYPES,
		BSDF_ALL = BSDF_ALL_REFLECTION | BSDF_ALL_TRANSMISSION
	}

	/// <summary>
	/// 
	/// </summary>
	public abstract class BxDF
	{
		/// <summary>
		/// 
		/// </summary>
		public readonly BxDFType Type;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public BxDF (BxDFType type)
		{
			this.Type = type;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="flags"></param>
		/// <returns></returns>
		public bool MatchesFlags (BxDFType flags)
		{
			return (this.Type & flags) == this.Type;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="wo"></param>
		/// <param name="wi"></param>
		/// <returns></returns>
		public abstract Spectrum F (Vector wo, Vector wi);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="wo"></param>
		/// <param name="wi"></param>
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <param name="pdf"></param>
		/// <returns></returns>
		public virtual Spectrum SampleF (Vector wo, ref Vector wi, double u1, double u2, ref double pdf)
		{
			wi = MonteCarlo.CosineSampleHemisphere (u1, u2);
			if (wo.z < 0.0)
				wi.z *= -1.0;
			pdf = Pdf (wo, wi);

			return F (wo, wi);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="wo"></param>
		/// <param name="nSamples"></param>
		/// <param name="samples"></param>
		/// <returns></returns>
		public virtual Spectrum Rho (Vector wo, int nSamples, double[] samples)
		{
			var r = new Spectrum ();
			for (var i = 0; i < nSamples; ++i)
			{
				var wi = new Vector ();
				var pdf = 0.0;
				var f = SampleF (wo, ref wi, samples[2 * i], samples[2 * i + 1], ref pdf);
				if (pdf > 0.0)
					r += f * Util.AbsCosTheta(wi) / pdf;
			}
			return (Spectrum)(r / (double)nSamples);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nSamples"></param>
		/// <param name="samples1"></param>
		/// <param name="samples2"></param>
		/// <returns></returns>
		public virtual Spectrum Rho (int nSamples, double[] samples1, double[] samples2)
		{
			var r = new Spectrum ();
			for (var i = 0; i < nSamples; ++i)
			{
				var wo = MonteCarlo.UniformSampleHemisphere (samples1[2 * i], samples1[2 * i + 1]);
				var wi = new Vector ();
				var pdf_o = Util.InvTwoPI;
				var pdf_i = 0.0;
				var f = SampleF (wo, ref wi, samples2[2 * i], samples2[2 * i + 1], ref pdf_i);

				if (pdf_i > 0.0)
					r += f * Util.AbsCosTheta (wi) * Util.AbsCosTheta (wo) / (pdf_o * pdf_i);
			}
			return r / (Math.PI * nSamples);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="wi"></param>
		/// <param name="wo"></param>
		/// <returns></returns>
		public virtual double Pdf (Vector wi, Vector wo)
		{
			return Util.SameHemisphere (wo, wi) ? Util.AbsCosTheta (wi) * Util.InvPI : 0.0;
		}
	}
}