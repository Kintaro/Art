using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;

namespace Art.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ISampler
	{
		/// <summary>
		/// 
		/// </summary>
		public readonly int xPixelStart;
		/// <summary>
		/// 
		/// </summary>
		public readonly int xPixelEnd;
		/// <summary>
		/// 
		/// </summary>
		public readonly int yPixelStart;
		/// <summary>
		/// 
		/// </summary>
		public readonly int yPixelEnd;
		/// <summary>
		/// 
		/// </summary>
		public readonly int SamplesPerPixel;
		/// <summary>
		/// 
		/// </summary>
		public readonly double ShutterOpen;
		/// <summary>
		/// 
		/// </summary>
		public readonly double ShutterClose;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sample"></param>
		/// <returns></returns>
		public abstract int GetMoreSamples (Sample[] sample);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sample"></param>
		/// <returns></returns>
		public abstract Task<int> GetMoreSamplesAsync (Sample[] sample);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="num"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public abstract ISampler GetSubSampler (int num, int count);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public abstract int RoundSize (int size);
		/// <summary>
		/// 
		/// </summary>
		public abstract int MaximumSampleCount { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="num"></param>
		/// <param name="count"></param>
		/// <param name="xstart"></param>
		/// <param name="xend"></param>
		/// <param name="ystart"></param>
		/// <param name="yend"></param>
		protected void ComputeSubWindow (int num, int count, ref int xstart, ref int xend, ref int ystart, ref int yend)
		{
			var dx = xPixelEnd - xPixelStart;
			var dy = yPixelEnd - yPixelStart;
			var nx = count;
			var ny = 1;

			while ((nx & 0x1) == 0 && 2 * dx * ny < dy * nx)
			{
				nx >>= 1;
				ny <<= 1;
			}

			Debug.Assert (nx * ny == count);

			var xo = num % nx;
			var yo = num / nx;
			var tx0 = (double) xo      / (double)nx;
			var tx1 = (double)(xo + 1) / (double)nx;
			var ty0 = (double) yo      / (double)ny;
			var ty1 = (double)(yo + 1) / (double)ny;

			xstart = Util.Floor2Int (Util.Lerp (tx0, xPixelStart, xPixelEnd));
			xend   = Util.Floor2Int (Util.Lerp (tx1, xPixelStart, xPixelEnd));
			ystart = Util.Floor2Int (Util.Lerp (ty0, yPixelStart, yPixelEnd));
			yend   = Util.Floor2Int (Util.Lerp (ty1, yPixelStart, yPixelEnd));
		}
	}
}
