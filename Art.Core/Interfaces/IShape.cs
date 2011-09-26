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
	public abstract class IShape
	{
		/// <summary>
		/// 
		/// </summary>
		public static int NextShapeID;
		/// <summary>
		/// 
		/// </summary>
		public readonly Transform ObjectToWorld;
		/// <summary>
		/// 
		/// </summary>
		public readonly Transform WorldToObject;
		/// <summary>
		/// 
		/// </summary>
		public readonly bool ReverseOrientation;
		/// <summary>
		/// 
		/// </summary>
		public readonly bool TransformSwapsHandedness;
		/// <summary>
		/// 
		/// </summary>
		public readonly int ShapeID;

		/// <summary>
		/// 
		/// </summary>
		public bool CanIntersect { get { throw new NotImplementedException (); } }
		/// <summary>
		/// 
		/// </summary>
		public double Area { get { throw new NotImplementedException (); } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <param name="tHit"></param>
		/// <param name="rayEpsilon"></param>
		/// <param name="dg"></param>
		/// <returns></returns>
		public bool Intersect (Ray ray, out double tHit, out double rayEpsilon, DifferentialGeometry dg)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <param name="tHit"></param>
		/// <param name="rayEpsilon"></param>
		/// <param name="dg"></param>
		/// <returns></returns>
		public Task<bool> IntersectAsync (Ray ray, out double tHit, out double rayEpsilon, DifferentialGeometry dg)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="wi"></param>
		/// <returns></returns>
		public double Pdf (Point p, Vector wi)
		{
			var dgLight = new DifferentialGeometry ();
			var ray = new Ray (p, wi, 1e-3);
			ray.Depth = -1;
			double thit, rayEpsilon;

			if (!this.Intersect (ray, out thit, out rayEpsilon, dgLight))
				return 0.0;
			var pdf = Util.DistanceSquared (p, ray.Apply (thit)) / (Util.AbsDot (dgLight.nn, -wi) * this.Area);
			if (double.IsInfinity (pdf))
				pdf = 0.0;
			return pdf;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="wi"></param>
		/// <returns></returns>
		public async Task<double> PdfAsync (Point p, Vector wi)
		{
			var dgLight = new DifferentialGeometry ();
			var ray = new Ray (p, wi, 1e-3);
			ray.Depth = -1;
			double thit, rayEpsilon;

			if (!await this.IntersectAsync (ray, out thit, out rayEpsilon, dgLight))
				return 0.0;
			var pdf = Util.DistanceSquared (p, ray.Apply (thit)) / (Util.AbsDot (dgLight.nn, -wi) * this.Area);
			if (double.IsInfinity (pdf))
				pdf = 0.0;
			return pdf;
		}

		/// <summary>
		/// 
		/// </summary>
		public abstract BoundingBox ObjectBound { get; }
	}
}
