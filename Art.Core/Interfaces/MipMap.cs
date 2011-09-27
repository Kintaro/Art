using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;
using Art.Core.Spectra;
using Art.Core.Tools;

namespace Art.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MipMap<T> where T : new ()
	{
		/// <summary>
		/// 
		/// </summary>
		internal class ResampleWeight
		{
			/// <summary>
			/// 
			/// </summary>
			public int FirstTexel;
			/// <summary>
			/// 
			/// </summary>
			public double[] Weight = new double[4];
		}

		/// <summary>
		/// 
		/// </summary>
		public enum ImageWrap
		{
			/// <summary>
			/// 
			/// </summary>
			Repeat,
			/// <summary>
			/// 
			/// </summary>
			Black,
			/// <summary>
			/// 
			/// </summary>
			Clamp
		}

		/// <summary>
		/// 
		/// </summary>
		public const int WeightLutSize = 128;
		/// <summary>
		/// 
		/// </summary>
		public int Width { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int Height { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int Levels { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		private bool doTrilinear;
		/// <summary>
		/// 
		/// </summary>
		private double maxAnisotropy;
		/// <summary>
		/// 
		/// </summary>
		private ImageWrap wrapMode;
		/// <summary>
		/// 
		/// </summary>
		private BlockedArray<T>[] pyramid;
		/// <summary>
		/// 
		/// </summary>
		private static double[] weightLut = new double[WeightLutSize];

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sres"></param>
		/// <param name="tres"></param>
		/// <param name="img"></param>
		/// <param name="doTri"></param>
		/// <param name="maxAniso"></param>
		/// <param name="wm"></param>
		public MipMap (int sres, int tres, T[] img, bool doTri, double maxAniso, ImageWrap wm)
		{
			this.doTrilinear = doTri;
			this.maxAnisotropy = maxAniso;
			this.wrapMode = wm;
			T[] resampledImage = null;
			if (!Util.IsPowerOf2 (sres) || !Util.IsPowerOf2 (tres))
			{
				// Resample image to power-of-two resolution
				var sPow2 = Util.RoundUpPow2 (sres);
				var tPow2 = Util.RoundUpPow2 (tres);

				// Resample image in $s$ direction
				var sWeights = this.ResampleWeights (sres, sPow2);
				resampledImage = new T[sPow2 * tPow2];

				// Apply _sWeights_ to zoom in $s$ direction
				var cores = Math.Max (1, Api.NumberOfCores >> 1);
				Parallel.For (0, tres, new ParallelOptions { MaxDegreeOfParallelism = cores }, t =>
				{
					Parallel.For (0, sPow2, new ParallelOptions { MaxDegreeOfParallelism = cores }, s =>
					{
						// Compute texel $(s,t)$ in $s$-zoomed image
						resampledImage[t * sPow2 + s] = default (T);
						for (int j = 0; j < 4; ++j)
						{
							int origS = sWeights[s].FirstTexel + j;
							if (wrapMode == ImageWrap.Repeat)
								origS = Util.Mod (origS, sres);
							else if (wrapMode == ImageWrap.Clamp)
								origS = Util.Clamp (origS, 0, sres - 1);
							if (origS >= 0 && origS < (int)sres)
							{
								dynamic a = sWeights[s].Weight[j];
								dynamic b = img[t * sres + origS];
								dynamic val = a * b;
								resampledImage[t * sPow2 + s] += val;
							}
						}
					});
				});

				// Resample image in $t$ direction
				var tWeights = this.ResampleWeights (tres, tPow2);
				var workData = new T[tPow2];
				for (var s = 0; s < sPow2; ++s)
				{
					for (var t = 0; t < tPow2; ++t)
					{
						workData[t] = default (T);
						for (var j = 0; j < 4; ++j)
						{
							int offset = tWeights[t].FirstTexel + j;
							if (wrapMode == ImageWrap.Repeat) offset = Util.Mod (offset, tres);
							else if (wrapMode == ImageWrap.Clamp) offset = Util.Clamp (offset, 0, tres - 1);
							if (offset >= 0 && offset < (int)tres)
							{
								dynamic a = tWeights[t].Weight[j];
								dynamic b = resampledImage[offset * sPow2 + s];
								workData[t] += a * b;
							}
						}
					}
					for (var t = 0; t < tPow2; ++t)
					{
						dynamic v = workData[t];
						dynamic val = this.Clamp (v);
						resampledImage[t * sPow2 + s] = val;
					}
				}

				img = resampledImage;
				sres = sPow2;
				tres = tPow2;
			}
			this.Width = sres;
			this.Height = tres;
			// Initialize levels of MIPMap from image
			this.Levels = 1 + Util.Log2Int (Math.Max (sres, tres));
			pyramid = new BlockedArray<T>[this.Levels];

			// Initialize most detailed level of MIPMap
			pyramid[0] = new BlockedArray<T> (sres, tres, img);
			for (var i = 1; i < this.Levels; ++i)
			{
				// Initialize $i$th MIPMap level from $i-1$st level
				var sRes = (int)Math.Max (1u, pyramid[i - 1].uSize / 2);
				var tRes = (int)Math.Max (1u, pyramid[i - 1].vSize / 2);
				pyramid[i] = new BlockedArray<T> (sRes, tRes);

				// Filter four texels from finer level of pyramid
				for (var t = 0; t < tRes; ++t)
				{
					for (var s = 0; s < sRes; ++s)
					{
						dynamic a = this.Texel (i - 1, 2 * s, 2 * t);
						dynamic b = this.Texel (i - 1, 2 * s + 1, 2 * t);
						dynamic c = this.Texel (i - 1, 2 * s, 2 * t + 1);
						dynamic d = this.Texel (i - 1, 2 * s + 1, 2 * t + 1);
						dynamic val = .25 * a + b + c + d;
						pyramid[i][s, t] = val;
					}
				}
			}

			// Initialize EWA filter weights if needed
			if (weightLut == null)
			{
				weightLut = new double[WeightLutSize];
				for (int i = 0; i < WeightLutSize; ++i)
				{
					var alpha = 2;
					var r2 = (double)(i) / (double)(WeightLutSize - 1);
					weightLut[i] = Math.Exp (-alpha * r2) - Math.Exp (-alpha);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		private double Clamp (double t)
		{
			return Util.Clamp (t, 0, double.PositiveInfinity);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		private SampledSpectrum Clamp (SampledSpectrum v)
		{
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="level"></param>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public T Texel (int level, int s, int t)
		{
			var l = this.pyramid[level];

			switch (this.wrapMode)
			{
				case ImageWrap.Repeat:
					s = Util.Mod (s, l.uSize);
					t = Util.Mod (t, l.vSize);
					break;
				case ImageWrap.Clamp:
					s = Util.Clamp (s, 0, l.uSize - 1);
					t = Util.Clamp (t, 0, l.vSize - 1);
					break;
				case ImageWrap.Black:
					dynamic black = 0.0;
					if (s < 0 || s >= l.uSize || t < 0 || t >= l.vSize)
						return black;
					break;
			}

			return l[s, t];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="level"></param>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public Task<T> TexelAsync (int level, int s, int t)
		{
			return new Task<T> (() => Texel (level, s, t));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		public T Lookup (double s, double t, double width = 0.0)
		{
			var level = this.Levels - 1 + Util.Log2 (Math.Max (width, 1e-8));

			if (level < 0)
				return this.Triangle (0, s, t);
			else if (level >= this.Levels - 1)
				return this.Texel (this.Levels - 1, 0, 0);
			else
			{
				var iLevel = Util.Floor2Int (level);
				dynamic delta = level - iLevel;
				return (1.0 - delta) * this.Triangle (iLevel, s, t) + delta * this.Triangle (iLevel + 1, s, t);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		public async Task<T> LookupAsync (double s, double t, double width = 0.0)
		{
			var level = this.Levels - 1 + Util.Log2 (Math.Max (width, 1e-8));

			if (level < 0)
				return await this.TriangleAsync (0, s, t);
			else if (level >= this.Levels - 1)
				return await this.TexelAsync (this.Levels - 1, 0, 0);
			else
			{
				var iLevel = Util.Floor2Int (level);
				dynamic delta = level - iLevel;
				return (1.0 - delta) * await this.TriangleAsync (iLevel, s, t) + delta * await this.TriangleAsync (iLevel + 1, s, t);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <param name="ds0"></param>
		/// <param name="dt0"></param>
		/// <param name="ds1"></param>
		/// <param name="dt1"></param>
		/// <returns></returns>
		public T Lookup (double s, double t, double ds0, double dt0, double ds1, double dt1)
		{
			if (this.doTrilinear)
			{
				var val = this.Lookup (s, t, 2.0 * Math.Max (Math.Max (Math.Abs (ds0), Math.Abs (dt0)), Math.Max (Math.Abs (ds1), Math.Abs (dt1))));
				return val;
			}

			if (ds0 * ds0 + dt0 * dt0 < ds1 * ds1 + dt1 * dt1)
			{
				var temp = ds0;
				ds0 = ds1;
				ds1 = temp;
				temp = dt0;
				dt0 = dt1;
				dt1 = temp;
			}

			var majorLength = Math.Sqrt (ds0 * ds0 + dt0 * dt0);
			var minorLength = Math.Sqrt (ds1 * ds1 + dt1 * dt1);

			if (minorLength * this.maxAnisotropy < majorLength && minorLength > 0.0)
			{
				var scale = majorLength / (minorLength * this.maxAnisotropy);
				ds1 *= scale;
				dt1 *= scale;
				minorLength *= scale;
			}

			if (minorLength == 0.0)
			{
				return this.Triangle (0, s, t);
			}

			var lod = Math.Max (0.0, this.Levels - 1.0 + Util.Log2 (minorLength));
			var ilod = Util.Floor2Int (lod);
			dynamic d = lod - ilod;

			dynamic result = (1.0 - d) * this.Ewa (ilod, s, t, ds0, dt0, ds1, dt1) + d * this.Ewa (ilod + 1, s, t, ds0, dt0, ds1, dt1);

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <param name="ds0"></param>
		/// <param name="dt0"></param>
		/// <param name="ds1"></param>
		/// <param name="dt1"></param>
		/// <returns></returns>
		public async Task<T> LookupAsync (double s, double t, double ds0, double dt0, double ds1, double dt1)
		{
			if (this.doTrilinear)
			{
				var val = this.Lookup (s, t, 2.0 * Math.Max (Math.Max (Math.Abs (ds0), Math.Abs (dt0)), Math.Max (Math.Abs (ds1), Math.Abs (dt1))));
				return val;
			}

			if (ds0 * ds0 + dt0 * dt0 < ds1 * ds1 + dt1 * dt1)
			{
				var temp = ds0;
				ds0 = ds1;
				ds1 = temp;
				temp = dt0;
				dt0 = dt1;
				dt1 = temp;
			}

			var majorLength = Math.Sqrt (ds0 * ds0 + dt0 * dt0);
			var minorLength = Math.Sqrt (ds1 * ds1 + dt1 * dt1);

			if (minorLength * this.maxAnisotropy < majorLength && minorLength > 0.0)
			{
				var scale = majorLength / (minorLength * this.maxAnisotropy);
				ds1 *= scale;
				dt1 *= scale;
				minorLength *= scale;
			}

			if (minorLength == 0.0)
			{
				return await this.TriangleAsync (0, s, t);
			}

			var lod = Math.Max (0.0, this.Levels - 1.0 + Util.Log2 (minorLength));
			var ilod = Util.Floor2Int (lod);
			dynamic d = lod - ilod;

			dynamic result = (1.0 - d) * await this.EwaAsync (ilod, s, t, ds0, dt0, ds1, dt1) + d * await this.EwaAsync (ilod + 1, s, t, ds0, dt0, ds1, dt1);

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="level"></param>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		private T Triangle (int level, double s, double t)
		{
			level = Util.Clamp (level, 0, this.Levels - 1);
			s = s * this.pyramid[level].uSize - .5;
			t = t * this.pyramid[level].vSize - .5;
			dynamic s0 = Util.Floor2Int (s);
			dynamic t0 = Util.Floor2Int (t);
			dynamic ds = s - s0;
			dynamic dt = t - t0;

			return (1.0 - ds) * (1.0 - dt) * this.Texel (level, s0, t0) +
				(1.0 - ds) * dt * this.Texel (level, s0, t0 + 1) +
				ds * (1.0 - dt) * this.Texel (level, s0 + 1, t0) +
				ds * dt * this.Texel (level, s0 + 1, t0 + 1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="level"></param>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		private async Task<T> TriangleAsync (int level, double s, double t)
		{
			level = Util.Clamp (level, 0, this.Levels - 1);
			s = s * this.pyramid[level].uSize - .5;
			t = t * this.pyramid[level].vSize - .5;
			dynamic s0 = Util.Floor2Int (s);
			dynamic t0 = Util.Floor2Int (t);
			dynamic ds = s - s0;
			dynamic dt = t - t0;

			return (1.0 - ds) * (1.0 - dt) * await this.TexelAsync (level, s0, t0) +
				(1.0 - ds) * dt * await this.TexelAsync (level, s0, t0 + 1) +
				ds * (1.0 - dt) * await this.TexelAsync (level, s0 + 1, t0) +
				ds * dt * await this.TexelAsync (level, s0 + 1, t0 + 1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oldres"></param>
		/// <param name="newres"></param>
		/// <returns></returns>
		private ResampleWeight[] ResampleWeights (int oldres, int newres)
		{
			var wt = new ResampleWeight[newres];
			var filterWidth = 2.0;

			Parallel.For (0, newres, new ParallelOptions { MaxDegreeOfParallelism = Api.NumberOfCores }, i =>
			{
				var center = (i + .5) * oldres / newres;
				if (wt[i] == null)
					wt[i] = new ResampleWeight ();
				wt[i].FirstTexel = Util.Floor2Int ((center - filterWidth) + .5);
				for (var j = 0; j < 4; ++j)
				{
					var pos = wt[i].FirstTexel + j + .5;
					wt[i].Weight[j] = Util.Lanczos ((pos - center) / filterWidth);
				}

				var invSumWts = 1.0 / wt[i].Weight.Sum ();
				wt[i].Weight = wt[i].Weight.Select (x => x * invSumWts).ToArray ();
			});

			return wt;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="level"></param>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <param name="ds0"></param>
		/// <param name="dt0"></param>
		/// <param name="ds1"></param>
		/// <param name="dt1"></param>
		/// <returns></returns>
		private T Ewa (int level, double s, double t, double ds0, double dt0, double ds1, double dt1)
		{
			if (level >= this.Levels) return Texel (this.Levels - 1, 0, 0);
			// Convert EWA coordinates to appropriate scale for level
			s = s * this.pyramid[level].uSize - 0.5;
			t = t * this.pyramid[level].vSize - 0.5;
			ds0 *= this.pyramid[level].uSize;
			dt0 *= this.pyramid[level].vSize;
			ds1 *= this.pyramid[level].uSize;
			dt1 *= this.pyramid[level].vSize;

			// Compute ellipse coefficients to bound EWA filter region
			var A = dt0 * dt0 + dt1 * dt1 + 1;
			var B = -2.0 * (ds0 * dt0 + ds1 * dt1);
			var C = ds0 * ds0 + ds1 * ds1 + 1;
			var invF = 1.0 / (A * C - B * B * 0.25f);
			A *= invF;
			B *= invF;
			C *= invF;

			// Compute the ellipse's $(s,t)$ bounding box in texture space
			var det = -B * B + 4.0 * A * C;
			var invDet = 1.0 / det;
			var uSqrt = Math.Sqrt (det * C);
			var vSqrt = Math.Sqrt (A * det);
			var s0 = Util.Ceil2Int (s - 2.0 * invDet * uSqrt);
			var s1 = Util.Floor2Int (s + 2.0 * invDet * uSqrt);
			var t0 = Util.Ceil2Int (t - 2.0 * invDet * vSqrt);
			var t1 = Util.Floor2Int (t + 2.0 * invDet * vSqrt);

			// Scan over ellipse bound and compute quadratic equation
			dynamic sum = 0.0;
			var sumWts = 0.0;
			for (int it = t0; it <= t1; ++it)
			{
				var tt = it - t;
				for (int iss = s0; iss <= s1; ++iss)
				{
					var ss = iss - s;
					// Compute squared radius and filter texel if inside ellipse
					var r2 = A * ss * ss + B * ss * tt + C * tt * tt;
					if (r2 < 1.0)
					{
						dynamic weight = weightLut[Math.Min (Util.Double2Int (r2 * WeightLutSize),
											 WeightLutSize - 1)];
						sum += this.Texel (level, iss, it) * weight;
						sumWts += weight;
					}
				}
			}
			return sum / sumWts;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="level"></param>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <param name="ds0"></param>
		/// <param name="dt0"></param>
		/// <param name="ds1"></param>
		/// <param name="dt1"></param>
		/// <returns></returns>
		private async Task<T> EwaAsync (int level, double s, double t, double ds0, double dt0, double ds1, double dt1)
		{
			if (level >= this.Levels) return await this.TexelAsync (this.Levels - 1, 0, 0);
			// Convert EWA coordinates to appropriate scale for level
			s = s * this.pyramid[level].uSize - 0.5;
			t = t * this.pyramid[level].vSize - 0.5;
			ds0 *= this.pyramid[level].uSize;
			dt0 *= this.pyramid[level].vSize;
			ds1 *= this.pyramid[level].uSize;
			dt1 *= this.pyramid[level].vSize;

			// Compute ellipse coefficients to bound EWA filter region
			var A = dt0 * dt0 + dt1 * dt1 + 1;
			var B = -2.0 * (ds0 * dt0 + ds1 * dt1);
			var C = ds0 * ds0 + ds1 * ds1 + 1;
			var invF = 1.0 / (A * C - B * B * 0.25f);
			A *= invF;
			B *= invF;
			C *= invF;

			// Compute the ellipse's $(s,t)$ bounding box in texture space
			var det = -B * B + 4.0 * A * C;
			var invDet = 1.0 / det;
			var uSqrt = Math.Sqrt (det * C);
			var vSqrt = Math.Sqrt (A * det);
			var s0 = Util.Ceil2Int (s - 2.0 * invDet * uSqrt);
			var s1 = Util.Floor2Int (s + 2.0 * invDet * uSqrt);
			var t0 = Util.Ceil2Int (t - 2.0 * invDet * vSqrt);
			var t1 = Util.Floor2Int (t + 2.0 * invDet * vSqrt);

			// Scan over ellipse bound and compute quadratic equation
			dynamic sum = 0.0;
			var sumWts = 0.0;
			for (int it = t0; it <= t1; ++it)
			{
				var tt = it - t;
				for (int iss = s0; iss <= s1; ++iss)
				{
					var ss = iss - s;
					// Compute squared radius and filter texel if inside ellipse
					var r2 = A * ss * ss + B * ss * tt + C * tt * tt;
					if (r2 < 1.0)
					{
						dynamic weight = weightLut[Math.Min (Util.Double2Int (r2 * WeightLutSize),
											 WeightLutSize - 1)];
						sum += await this.TexelAsync (level, iss, it) * weight;
						sumWts += weight;
					}
				}
			}
			return sum / sumWts;
		}
	}
}
