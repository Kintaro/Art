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
		///		The next shape's id when it is created
		/// </summary>
		public static int NextShapeID;
		/// <summary>
		///		Transformation from object coordinates to world coordinates
		/// </summary>
		public readonly Transform ObjectToWorld;
		/// <summary>
		///		Transformation from world coordinates to object coordinates
		/// </summary>
		public readonly Transform WorldToObject;
		/// <summary>
		///		True if the orientation is reversed
		/// </summary>
		public readonly bool ReverseOrientation;
		/// <summary>
		///		True if the transformation swaps handedness
		/// </summary>
		public readonly bool TransformSwapsHandedness;
		/// <summary>
		///		The shape's ID
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
			this.ShapeID = NextShapeID++;
		}

		/// <summary>
		///		Determines whether the shape is able to compute intersections
		/// </summary>
		public virtual bool CanIntersect { get { throw new NotImplementedException (); } }
		/// <summary>
		///		Gets the shape's area
		/// </summary>
		public virtual double Area { get { throw new NotImplementedException (); } }

		/// <summary>
		///		Checks if the shape intersects the ray
		/// </summary>
		/// <param name="ray">The ray to test against</param>
		/// <param name="tHit">If it intersects, the time where the ray was hit</param>
		/// <param name="rayEpsilon">The sharpness of the hit</param>
		/// <param name="dg">The differential geometry at the point of intersection</param>
		/// <returns>True if it intersects</returns>
		public virtual bool Intersect (Ray ray, out double tHit, out double rayEpsilon, DifferentialGeometry dg)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public virtual bool IntersectP (Ray ray)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		///		Computes the propability distribution function at the given point
		/// </summary>
		/// <param name="p">The point to check against</param>
		/// <returns>The propability distribution function</returns>
		public virtual double Pdf (Point p)
		{
			return 1.0 / this.Area;
		}

		/// <summary>
		///		Computes the propability distribution function at the given point
		/// </summary>
		/// <param name="p">The point to check against</param>
		/// <param name="wi">The out vector</param>
		/// <returns>The propability distribution function</returns>
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
		///		Computes the shape's bounding box
		/// </summary>
		public abstract BoundingBox ObjectBound { get; }
	}
}
