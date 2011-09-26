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
		/// <summary>
		/// 
		/// </summary>
		private const int sampledLambdaStart = 400;
		/// <summary>
		/// 
		/// </summary>
		private const int sampledLambdaEnd = 700;
		/// <summary>
		/// 
		/// </summary>
		private const int nRGB2SpectSamples = 32;
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum X = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum Y = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum Z = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbRefl2SpectWhite = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbRefl2SpectCyan = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbRefl2SpectMagenta = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbRefl2SpectYellow = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbRefl2SpectRed = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbRefl2SpectGreen = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbRefl2SpectBlue = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbIllum2SpectWhite = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbIllum2SpectCyan = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbIllum2SpectMagenta = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbIllum2SpectYellow = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbIllum2SpectRed = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbIllum2SpectGreen = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static SampledSpectrum rgbIllum2SpectBlue = new SampledSpectrum ();
		/// <summary>
		/// 
		/// </summary>
		public static double yint;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		public SampledSpectrum (double v = 0.0)
			: base (NumberOfSpectralSamples, v)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Init ()
		{
			Parallel.For (0, NumberOfSpectralSamples, new ParallelOptions { MaxDegreeOfParallelism = Api.NumberOfCores }, i =>
			{
				var wl0 = Util.Lerp ((double)i / (double)(NumberOfSpectralSamples),
								 sampledLambdaStart, sampledLambdaEnd);
				var wl1 = Util.Lerp ((double)(i + 1) / (double)(NumberOfSpectralSamples),
								 sampledLambdaStart, sampledLambdaEnd);
				X.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.CIE_Lambda, SpectrumCIE.CIE_X, SpectrumCIE.nCIESamples, wl0, wl1);
				Y.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.CIE_Lambda, SpectrumCIE.CIE_Y, SpectrumCIE.nCIESamples, wl0, wl1);
				Z.c[i] = Spectrum.AverageSpectrumSamples (SpectrumCIE.CIE_Lambda, SpectrumCIE.CIE_Z, SpectrumCIE.nCIESamples, wl0, wl1);
				yint += Y.c[i];
			});
			Parallel.For (0, NumberOfSpectralSamples, new ParallelOptions { MaxDegreeOfParallelism = Api.NumberOfCores }, i =>
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
			});
		}

		/// <summary>
		/// 
		/// </summary>
		public double y
		{
			get
			{
				var yy = 0.0;
				for (var i = 0; i < NumberOfSpectralSamples; ++i)
					yy += Y.c[i] * c[i];
				return yy / yint;
			}
		}
	}
}
