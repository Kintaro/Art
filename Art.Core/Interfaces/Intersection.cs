using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;
using Art.Core.Reflection;
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
		public IPrimitive Primitive;
		/// <summary>
		/// 
		/// </summary>
		public Transform WorldToObject;
		/// <summary>
		/// 
		/// </summary>
		public Transform ObjectToWorld;
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
		/// <param name="ray"></param>
		/// <returns></returns>
		public BSDF GetBSDF (RayDifferential ray)
		{
			this.dg.ComputeDifferentials (ray);
			return this.Primitive.GetBSDF (dg, this.ObjectToWorld);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public async Task<BSDF> GetBSDFAsync (RayDifferential ray)
		{
			this.dg.ComputeDifferentials (ray);
			return await this.Primitive.GetBSDFAsync (dg, this.ObjectToWorld);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public BSSRDF GetBSSRDF (RayDifferential ray)
		{
			this.dg.ComputeDifferentials (ray);
			return this.Primitive.GetBSSRDF (this.dg, this.ObjectToWorld);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public async Task<BSSRDF> GetBSSRDFAsync (RayDifferential ray)
		{
			this.dg.ComputeDifferentials (ray);
			return await this.Primitive.GetBSSRDFAsync (this.dg, this.ObjectToWorld);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="wo"></param>
		/// <returns></returns>
		public Spectrum Le (Vector wo)
		{
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="wo"></param>
		/// <returns></returns>
		public async Task<Spectrum> LeAsync (Vector wo)
		{
			return null;
		}
	}
}
