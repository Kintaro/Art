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
	public abstract class IPrimitive
	{
		/// <summary>
		/// 
		/// </summary>
		protected static int NextPrimitiveID;
		/// <summary>
		/// 
		/// </summary>
		public readonly int PrimitiveID;

		/// <summary>
		/// 
		/// </summary>
		protected IPrimitive ()
		{
			this.PrimitiveID = NextPrimitiveID++;
		}

		/// <summary>
		/// 
		/// </summary>
		public abstract BoundingBox WorldBound { get; }
		/// <summary>
		/// 
		/// </summary>
		public abstract bool CanIntersect { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <param name="intersection"></param>
		/// <returns></returns>
		public abstract bool Intersect (Ray ray, Intersection intersection);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <param name="intersection"></param>
		/// <returns></returns>
		public abstract Task<bool> IntersectAsync (Ray ray, Intersection intersection);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public abstract bool IntersectP (Ray ray);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public abstract Task<bool> IntersectPAsync (Ray ray);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="refined"></param>
		public abstract void Refine (IList<IPrimitive> refined);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="refined"></param>
		public abstract Task RefineAsync (IList<IPrimitive> refined);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dg"></param>
		/// <param name="objectToWorld"></param>
		/// <returns></returns>
		public abstract BSDF GetBSDF (DifferentialGeometry dg, Transform objectToWorld);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dg"></param>
		/// <param name="objectToWorld"></param>
		/// <returns></returns>
		public abstract Task<BSDF> GetBSDFAsync (DifferentialGeometry dg, Transform objectToWorld);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="refined"></param>
		public void FullyRefine (IList<IPrimitive> refined)
		{
			var todo = new Stack<IPrimitive> ();
			todo.Push (this);
			while (todo.Count > 0)
			{
				var primitive = todo.Pop ();
				if (primitive.CanIntersect)
					refined.Add (primitive);
				else
					primitive.Refine (refined);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="refined"></param>
		public async void FullyRefineAsync (IList<IPrimitive> refined)
		{
			var todo = new Stack<IPrimitive> ();
			todo.Push (this);
			while (todo.Count > 0)
			{
				var primitive = todo.Pop ();
				if (primitive.CanIntersect)
					refined.Add (primitive);
				else
					await primitive.RefineAsync (refined);
			}
		}
	}
}
