﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.Core.Geometry
{
	/// <summary>
	/// 
	/// </summary>
	public class MonteCarlo
	{
		/// <summary>
		/// 
		/// </summary>
		public const double OneMinusEpsilon = 1.0 - double.Epsilon;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <returns></returns>
		public static Vector UniformSampleHemisphere (double u1, double u2)
		{
			var z = u1;
			var r = Math.Sqrt (Math.Max (0.0, 1.0 - z * z));
			var phi = 2 * Math.PI * u2;
			var x = r * Math.Cos (phi);
			var y = r * Math.Sin (phi);

			return new Vector (x, y, z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <returns></returns>
		public static Vector CosineSampleHemisphere (double u1, double u2)
		{
			var ret = new Vector ();
			ConcentricSampleDisk (u1, u2, ref ret.x, ref ret.y);
			ret.z = Math.Sqrt (Math.Max (0.0, 1.0 - ret.x * ret.x - ret.y * ret.y));
			return ret;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		public static void ConcentricSampleDisk (double u1, double u2, ref double dx, ref double dy)
		{
			var r = 0.0;
			var theta = 0.0;
			
			var sx = 2 * u1 - 1;
			var sy = 2 * u2 - 1;

			if (sx == 0.0 && sy == 0.0)
			{
				dx = 0.0;
				dy = 0.0;
				return;
			}

			if (sx >= -sy)
			{
				if (sx > sy)
				{
					// Handle first region of disk
					r = sx;
					if (sy > 0.0) theta = sy / r;
					else theta = 8.0f + sy / r;
				}
				else
				{
					// Handle second region of disk
					r = sy;
					theta = 2.0f - sx / r;
				}
			}
			else
			{
				if (sx <= sy)
				{
					// Handle third region of disk
					r = -sx;
					theta = 4.0f - sy / r;
				}
				else
				{
					// Handle fourth region of disk
					r = -sy;
					theta = 6.0f + sx / r;
				}
			}
			theta *= Math.PI / 4.0;
			dx = r * Math.Cos (theta);
			dy = r * Math.Sin (theta);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n"></param>
		/// <param name="scramble"></param>
		/// <returns></returns>
		public static double VanDerCorput(int n, int scramble) 
		{
			n = (n << 16) | (n >> 16);
			n = ((int)(n & 0x00ff00ff) << 8) | ((int)(n & 0xff00ff00) >> 8);
			n = ((int)(n & 0x0f0f0f0f) << 4) | ((int)(n & 0xf0f0f0f0) >> 4);
			n = ((int)(n & 0x33333333) << 2) | ((int)(n & 0xcccccccc) >> 2);
			n = ((int)(n & 0x55555555) << 1) | ((int)(n & 0xaaaaaaaa) >> 1);
			n ^= scramble;
			return Math.Min(((n>>8) & 0xffffff) / (double)(1 << 24), OneMinusEpsilon);
		}
	}
}
