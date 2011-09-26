using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;
using Art.Core.Reflection;

namespace Art.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class IMaterial
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dgGeom"></param>
		/// <param name="dgShading"></param>
		/// <returns></returns>
		public abstract BSDF GetBSDF (DifferentialGeometry dgGeom, DifferentialGeometry dgShading);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dgGeom"></param>
		/// <param name="dgShading"></param>
		/// <returns></returns>
		public abstract Task<BSDF> GetBSDFAsync (DifferentialGeometry dgGeom, DifferentialGeometry dgShading);
	}
}
