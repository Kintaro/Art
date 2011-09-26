using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
