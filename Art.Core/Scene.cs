using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;
using Art.Core.Interfaces;

namespace Art.Core
{
	/// <summary>
	/// 
	/// </summary>
	public class Scene
	{
		/// <summary>
		/// 
		/// </summary>
		public IPrimitive Aggregate;
		/// <summary>
		/// 
		/// </summary>
		public BoundingBox Bound;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <param name="isect"></param>
		/// <returns></returns>
		public bool Intersect (Ray ray, Intersection isect)
		{
			return this.Aggregate.Intersect (ray, isect);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <param name="isect"></param>
		/// <returns></returns>
		public async Task<bool> IntersectAsync (Ray ray, Intersection isect)
		{
			return await this.Aggregate.IntersectAsync (ray, isect);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public bool IntersectP (Ray ray)
		{
			return this.Aggregate.IntersectP (ray);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public async Task<bool> IntersectPAsync (Ray ray)
		{
			return await this.Aggregate.IntersectPAsync (ray);
		}
	}
}
