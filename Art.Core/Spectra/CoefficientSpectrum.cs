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
    public class CoefficientSpectrum
    {
		/// <summary>
		/// 
		/// </summary>
		protected double[] c;
		/// <summary>
		/// 
		/// </summary>
		protected int nSamples;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nSamples"></param>
		protected CoefficientSpectrum (int nSamples)
		{
			this.nSamples = nSamples;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nSamples"></param>
		/// <param name="v"></param>
		public CoefficientSpectrum (int nSamples, double v = 0.0) 
			: this (nSamples)
		{
			this.c = new double[nSamples];
			for (var i = 0; i < nSamples; ++i)
				c[i] = v;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nSamples"></param>
		/// <param name="s"></param>
		public CoefficientSpectrum (int nSamples, CoefficientSpectrum s)
			: this (nSamples)
		{
			this.c = new double[nSamples];
			s.c.CopyTo (c, 0);
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsBlack
		{
			get
			{
				for (var i = 0; i < this.nSamples; ++i)
					if (this.c[i] != 0.0)
						return false;
				return true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		public static CoefficientSpectrum operator + (CoefficientSpectrum s1, CoefficientSpectrum s2) 
		{
			var ret = new CoefficientSpectrum(s1.nSamples);
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
		public static CoefficientSpectrum operator * (CoefficientSpectrum s, double f)
		{
			var result = new CoefficientSpectrum (s.nSamples);
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
		public static CoefficientSpectrum operator / (CoefficientSpectrum s, double f)
		{
			var result = new CoefficientSpectrum (s.nSamples);
			for (var i = 0; i < s.nSamples; ++i)
				result.c[i] = s.c[i] / f;
			return s;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xyz"></param>
		/// <param name="rgb"></param>
		public static void XYZToRGB(double[] xyz, double[] rgb) 
		{
			rgb[0] =  3.240479 * xyz[0] - 1.537150 * xyz[1] - 0.498535 * xyz[2];
			rgb[1] = -0.969256 * xyz[0] + 1.875991 * xyz[1] + 0.041556 * xyz[2];
			rgb[2] =  0.055648 * xyz[0] - 0.204043 * xyz[1] + 1.057311 * xyz[2];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rgb"></param>
		/// <param name="xyz"></param>
		public static void RGBToXYZ(double[] rgb, double[] xyz) 
		{
			xyz[0] = 0.412453 * rgb[0] + 0.357580 * rgb[1] + 0.180423 * rgb[2];
			xyz[1] = 0.212671 * rgb[0] + 0.715160 * rgb[1] + 0.072169 * rgb[2];
			xyz[2] = 0.019334 * rgb[0] + 0.119193 * rgb[1] + 0.950227 * rgb[2];
		}
	}
}
