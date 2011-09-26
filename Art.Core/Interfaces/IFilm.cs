using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Spectra;

namespace Art.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class IFilm
	{
		/// <summary>
		/// 
		/// </summary>
		public readonly int xResolution;
		/// <summary>
		/// 
		/// </summary>
		public readonly int yResolution;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xres"></param>
		/// <param name="yres"></param>
		protected IFilm (int xres, int yres)
		{
			this.xResolution = xres;
			this.yResolution = yres;
		}

		public abstract void AddSample (CameraSample sample, Spectrum L);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sample"></param>
		/// <param name="L"></param>
		public abstract void Splat (CameraSample sample, Spectrum L);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xstart"></param>
		/// <param name="xend"></param>
		/// <param name="ystart"></param>
		/// <param name="ystart"></param>
		public abstract void GetSampleExtent (ref int xstart, ref int xend, ref int ystart, ref int yend);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xstart"></param>
		/// <param name="xend"></param>
		/// <param name="ystart"></param>
		/// <param name="ystart"></param>
		public abstract void GetPixelExtent (ref int xstart, ref int xend, ref int ystart, ref int yend);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x0"></param>
		/// <param name="y0"></param>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="splatScale"></param>
		public abstract void UpdateDisplay (int x0, int y0, int x1, int y1, double splatScale = 1.0);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="splatScale"></param>
		public abstract void WriteImage (double splatScale = 1.0);
	}
}
