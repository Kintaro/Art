using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;
using Art.Core.Spectra;

namespace Art.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class AreaLight : ILight
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="l2w"></param>
		/// <param name="ns"></param>
		public AreaLight (Transform l2w, int ns)
			: base (l2w, ns)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="n"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public abstract Spectrum L (Point p, Normal n, Vector w);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="n"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public abstract Task<Spectrum> LAsync (Point p, Normal n, Vector w);
	}
}
