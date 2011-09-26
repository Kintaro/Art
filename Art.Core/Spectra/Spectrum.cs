using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;

namespace Art.Core.Spectra
{
	/// <summary>
	/// 
	/// </summary>
	public class Spectrum : CoefficientSpectrum
	{
		/// <summary>
		/// 
		/// </summary>
		public Spectrum ()
			: base (3)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		public static Spectrum operator + (Spectrum s1, Spectrum s2)
		{
			var ret = new Spectrum ();
			for (var i = 0; i < s1.nSamples; ++i)
				ret.c[i] = s1.c[i] + s2.c[i];
			return ret;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="f"></param>
		/// <returns></returns>
		public static Spectrum operator * (Spectrum s, double f)
		{
			var result = new Spectrum ();
			for (var i = 0; i < s.nSamples; ++i)
				result.c[i] = s.c[i] * f;
			return s;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="f"></param>
		/// <returns></returns>
		public static Spectrum operator / (Spectrum s, double f)
		{
			var result = new Spectrum ();
			for (var i = 0; i < s.nSamples; ++i)
				result.c[i] = s.c[i] / f;
			return s;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lambda"></param>
		/// <param name="vals"></param>
		/// <param name="n"></param>
		/// <param name="lamdaStart"></param>
		/// <param name="lambdaEnd"></param>
		/// <returns></returns>
		public static double AverageSpectrumSamples (double[] lambda, double[] vals, int n, double lambdaStart, double lambdaEnd)
		{
			// Handle cases with out-of-bounds range or single sample only
			if (lambdaEnd <= lambda[0]) return vals[0];
			if (lambdaStart >= lambda[n - 1]) return vals[n - 1];
			if (n == 1) return vals[0];
			var sum = 0.0;
			// Add contributions of constant segments before/after samples
			if (lambdaStart < lambda[0])
				sum += vals[0] * (lambda[0] - lambdaStart);
			if (lambdaEnd > lambda[n - 1])
				sum += vals[n - 1] * (lambdaEnd - lambda[n - 1]);

			// Advance to first relevant wavelength segment
			var i = 0;
			while (lambdaStart > lambda[i + 1]) ++i;

			// Loop over wavelength sample segments and add contributions
			for (; i + 1 < n && lambdaEnd >= lambda[i]; ++i)
			{
				var segStart = Math.Max (lambdaStart, lambda[i]);
				var segEnd = Math.Min (lambdaEnd, lambda[i + 1]);
				sum += SegAverage (segStart, segEnd, i, lambda, vals) * (segEnd - segStart);
			}
			return sum / (lambdaEnd - lambdaStart);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="wl0"></param>
		/// <param name="wl1"></param>
		/// <param name="i"></param>
		/// <param name="lambda"></param>
		/// <param name="vals"></param>
		/// <returns></returns>
		private static double SegAverage (double wl0, double wl1, int i, double[] lambda, double[] vals)
		{
			return 0.5 * (Interpolate (wl0, i, lambda, vals) + Interpolate (wl1, i, lambda, vals));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="w"></param>
		/// <param name="i"></param>
		/// <param name="lambda"></param>
		/// <param name="vals"></param>
		/// <returns></returns>
		private static double Interpolate (double w, int i, double[] lambda, double[] vals)
		{
			return Util.Lerp (((w) - lambda[i]) / (lambda[(i) + 1] - lambda[i]), vals[i], vals[(i) + 1]);
		}
	}
}
