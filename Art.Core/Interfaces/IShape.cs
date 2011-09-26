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
		/// <param name="o2w"></param>
		/// <param name="w2o"></param>
		/// <param name="ro"></param>
		public IShape (Transform o2w, Transform w2o, bool ro)
		{
			this.ObjectToWorld = new Transform (o2w);
			this.WorldToObject = new Transform (w2o);
			this.ReverseOrientation = ro;
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool CanIntersect { get { throw new NotImplementedException (); } }
		/// <summary>
		/// 
		/// </summary>
		public virtual double Area { get { throw new NotImplementedException (); } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <param name="tHit"></param>
		/// <param name="rayEpsilon"></param>
		/// <param name="dg"></param>
		/// <returns></returns>
		public virtual bool Intersect (Ray ray, out double tHit, out double rayEpsilon, DifferentialGeometry dg)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public virtual double Pdf (Point p)
		{
			return 1.0 / this.Area;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="wi"></param>
		/// <returns></returns>
		public virtual double Pdf (Point p, Vector wi)
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
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <param name="Ns"></param>
		/// <returns></returns>
		public virtual Point Sample (double u1, double u2, Normal Ns)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <param name="Ns"></param>
		/// <returns></returns>
		public virtual async Task<Point> SampleAsync (double u1, double u2, Normal Ns)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="P"></param>
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <param name="Ns"></param>
		/// <returns></returns>
		public virtual Point Sample (Point P, double u1, double u2, Normal Ns)
		{
			return this.Sample (u1, u2, Ns);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="P"></param>
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <param name="Ns"></param>
		/// <returns></returns>
		public virtual async Task<Point> SampleAsync (Point P, double u1, double u2, Normal Ns)
		{
			return await this.SampleAsync (u1, u2, Ns);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="refined"></param>
		public virtual void Refine (IEnumerable<IShape> refined)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// 
		/// </summary>
		public abstract BoundingBox ObjectBound { get; }
	}
}
