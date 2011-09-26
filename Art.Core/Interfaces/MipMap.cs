using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;

namespace Art.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MipMap<T>
	{
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
		/// <param name="level"></param>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public T Texel (int level, int s, int t)
		{
			return default (T);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="level"></param>
		/// <param name="s"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public async Task<T> TexelAsync (int level, int s, int t)
		{
			return default (T);
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
			return default (T);
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
			return default (T);
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
			return default (T);
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
			// TODO
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
			// TODO
			dynamic s0 = Util.Floor2Int (s);
			dynamic t0 = Util.Floor2Int (t);
			dynamic ds = s - s0;
			dynamic dt = t - t0;

			return (1.0 - ds) * (1.0 - dt) * await this.TexelAsync (level, s0, t0) +
				(1.0 - ds) * dt * await this.TexelAsync (level, s0, t0 + 1) +
				ds * (1.0 - dt) * await this.TexelAsync (level, s0 + 1, t0) +
				ds * dt * await this.TexelAsync (level, s0 + 1, t0 + 1);
		}
	}
}
