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
	public class SampledSpectrum : CoefficientSpectrum
	{
		/// <summary>
		/// 
		/// </summary>
		public const int NumberOfSpectralSamples = 30;
		private const int sampledLambdaStart = 400;
		private const int sampledLambdaEnd = 700;
		private const int nRGB2SpectSamples = 32;
		public static SampledSpectrum X = new SampledSpectrum ();
		public static SampledSpectrum Y = new SampledSpectrum ();
		public static SampledSpectrum Z = new SampledSpectrum ();
		public static SampledSpectrum rgbRefl2SpectWhite = new SampledSpectrum ();
		public static SampledSpectrum rgbRefl2SpectCyan = new SampledSpectrum ();
		public static SampledSpectrum rgbRefl2SpectMagenta = new SampledSpectrum ();
		public static SampledSpectrum rgbRefl2SpectYellow = new SampledSpectrum ();
		public static SampledSpectrum rgbRefl2SpectRed = new SampledSpectrum ();
		public static SampledSpectrum rgbRefl2SpectGreen = new SampledSpectrum ();
		public static SampledSpectrum rgbRefl2SpectBlue = new SampledSpectrum ();
		public static SampledSpectrum rgbIllum2SpectWhite = new SampledSpectrum ();
		public static SampledSpectrum rgbIllum2SpectCyan = new SampledSpectrum ();
		public static SampledSpectrum rgbIllum2SpectMagenta = new SampledSpectrum ();
		public static SampledSpectrum rgbIllum2SpectYellow = new SampledSpectrum ();
		public static SampledSpectrum rgbIllum2SpectRed = new SampledSpectrum ();
		public static SampledSpectrum rgbIllum2SpectGreen = new SampledSpectrum ();
		public static SampledSpectrum rgbIllum2SpectBlue = new SampledSpectrum ();
		public static double yint;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		public SampledSpectrum (double v = 0.0)
			: base (NumberOfSpectralSamples, v)
		{
		}

		public static void Init ()
		{
			for (var i = 0; i < NumberOfSpectralSamples; ++i)
			{
				var wl0 = Util.Lerp ((double)i / (double)(NumberOfSpectralSamples),
								 sampledLambdaStart, sampledLambdaEnd);
				var wl1 = Util.Lerp ((double)(i + 1) / (double)(NumberOfSpectralSamples),
								 sampledLambdaStart, sampledLambdaEnd);
				X.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.CIE_Lambda, SpectrumCIE.CIE_X, SpectrumCIE.nCIESamples, wl0, wl1);
				Y.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.CIE_Lambda, SpectrumCIE.CIE_Y, SpectrumCIE.nCIESamples, wl0, wl1);
				Z.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.CIE_Lambda, SpectrumCIE.CIE_Z, SpectrumCIE.nCIESamples, wl0, wl1);
				yint += Y.c[i];
			}
			for (var i = 0; i < NumberOfSpectralSamples; ++i)
			{
				var wl0 = Util.Lerp ((double)(i) / (double)(NumberOfSpectralSamples),
								 sampledLambdaStart, sampledLambdaEnd);
				var wl1 = Util.Lerp ((double)(i + 1) / (double)(NumberOfSpectralSamples),
								 sampledLambdaStart, sampledLambdaEnd);
				rgbRefl2SpectWhite.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBRefl2SpectWhite,
					nRGB2SpectSamples, wl0, wl1);
				rgbRefl2SpectCyan.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBRefl2SpectCyan,
					nRGB2SpectSamples, wl0, wl1);
				rgbRefl2SpectMagenta.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBRefl2SpectMagenta,
					nRGB2SpectSamples, wl0, wl1);
				rgbRefl2SpectYellow.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBRefl2SpectYellow,
					nRGB2SpectSamples, wl0, wl1);
				rgbRefl2SpectRed.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBRefl2SpectRed,
					nRGB2SpectSamples, wl0, wl1);
				rgbRefl2SpectGreen.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBRefl2SpectGreen,
					nRGB2SpectSamples, wl0, wl1);
				rgbRefl2SpectBlue.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBRefl2SpectBlue,
					nRGB2SpectSamples, wl0, wl1);

				rgbIllum2SpectWhite.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBIllum2SpectWhite,
					nRGB2SpectSamples, wl0, wl1);
				rgbIllum2SpectCyan.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBIllum2SpectCyan,
					nRGB2SpectSamples, wl0, wl1);
				rgbIllum2SpectMagenta.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBIllum2SpectMagenta,
					nRGB2SpectSamples, wl0, wl1);
				rgbIllum2SpectYellow.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBIllum2SpectYellow,
					nRGB2SpectSamples, wl0, wl1);
				rgbIllum2SpectRed.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBIllum2SpectRed,
					nRGB2SpectSamples, wl0, wl1);
				rgbIllum2SpectGreen.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBIllum2SpectGreen,
					nRGB2SpectSamples, wl0, wl1);
				rgbIllum2SpectBlue.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.RGB2SpectLambda, SpectrumCIE.RGBIllum2SpectBlue,
					nRGB2SpectSamples, wl0, wl1);
			}
		}
	}
}
