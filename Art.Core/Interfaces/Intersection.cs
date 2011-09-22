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
	public class Intersection
	{
		/// <summary>
		/// 
		/// </summary>
		public DifferentialGeometry dg;
		/// <summary>
		/// 
		/// </summary>
		public int ShapeID;
		/// <summary>
		/// 
		/// </summary>
		public int PrimitiveID;
		/// <summary>
		/// 
		/// </summary>
		public double RayEpsilon;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="wo"></param>
		/// <returns></returns>
		public Spectrum Le (Vector wo)
		{
			return null;
		}
	}
}
