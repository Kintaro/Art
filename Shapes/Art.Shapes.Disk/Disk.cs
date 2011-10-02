using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;
using Art.Core.Interfaces;
using Art.Core.Tools;

namespace Art.Shapes.Disk
{
	/// <summary>
	/// 
	/// </summary>
	public class Disk : IShape
	{
		/// <summary>
		/// 
		/// </summary>
		private double height;
		/// <summary>
		/// 
		/// </summary>
		private double radius;
		/// <summary>
		/// 
		/// </summary>
		private double innerRadius;
		/// <summary>
		/// 
		/// </summary>
		private double phiMax;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="o2w"></param>
		/// <param name="w2o"></param>
		/// <param name="ro"></param>
		/// <param name="height"></param>
		/// <param name="radius"></param>
		/// <param name="innerRadius"></param>
		/// <param name="phiMax"></param>
		public Disk (Transform o2w, Transform w2o, bool ro, double height, double radius, double innerRadius, double phiMax)
			: base (o2w, w2o, ro)
		{
			this.height = height;
			this.radius = radius;
			this.innerRadius = innerRadius;
			this.phiMax = phiMax;
		}

		public override BoundingBox ObjectBound
		{
			get 
			{
				return new BoundingBox (new Point (-radius, -radius, -radius), new Point (radius, radius, radius));
			}
		}

		public override bool Intersect (Ray r, out double tHit, out double rayEpsilon, DifferentialGeometry dg)
		{
			tHit = 0.0;
			rayEpsilon = 0.0;

			var ray = new Ray ();
			this.WorldToObject.Apply (r, ref ray);

			if (Math.Abs (ray.Direction.z) < 1e-7) return false;
			var thit = (height - ray.Origin.z) / ray.Direction.z;
			if (thit < ray.MinT || thit > ray.MaxT)
				return false;

			var phit = ray.Apply (thit);
			var dist2 = phit.x * phit.x + phit.y * phit.y;
			if (dist2 > radius * radius || dist2 < innerRadius * innerRadius)
				return false;

			var phi = Math.Atan2 (phit.y, phit.x);
			if (phi < 0) phi += 2.0 * Math.PI;
			if (phi > phiMax)
				return false;

			var u = phi / phiMax;
			var oneMinusV = ((Math.Sqrt (dist2) - innerRadius) /
							   (radius - innerRadius));
			var invOneMinusV = (oneMinusV > 0.0) ? (1.0 / oneMinusV) : 0.0;
			var v = 1.0 - oneMinusV;
			var dpdu = new Vector (-phiMax * phit.y, phiMax * phit.x, 0.0);
			var dpdv = new Vector (-phit.x * invOneMinusV, -phit.y * invOneMinusV, 0.0);
			dpdu *= phiMax * Util.InvTwoPI;
			dpdv *= (radius - innerRadius) / radius;
			var dndu = new Normal (0, 0, 0);
			var dndv = new Normal (0, 0, 0);

			var o2w = new Transform (this.ObjectToWorld);
			dg = new DifferentialGeometry (o2w.Apply (phit), null, null, o2w.Apply (dndu), o2w.Apply (dndv), u, v, this);

			tHit = thit;
			rayEpsilon = 5e-4 * tHit;

			return true;
		}

		public override bool IntersectP (Ray r)
		{
			var ray = new Ray ();
			this.WorldToObject.Apply (r, ref ray);

			if (Math.Abs (ray.Direction.z) < 1e-7) return false;
			var thit = (height - ray.Origin.z) / ray.Direction.z;
			if (thit < ray.MinT || thit > ray.MaxT)
				return false;

			var phit = ray.Apply (thit);
			var dist2 = phit.x * phit.x + phit.y * phit.y;
			if (dist2 > radius * radius || dist2 < innerRadius * innerRadius)
				return false;

			var phi = Math.Atan2 (phit.y, phit.x);
			if (phi < 0) phi += 2.0 * Math.PI;
			if (phi > phiMax)
				return false;
			return true;
		}

		public override double Area
		{
			get
			{
				return phiMax * .5 * (radius * radius - innerRadius * innerRadius);
			}
		}

		public override Point Sample (double u1, double u2, Normal Ns)
		{
			var p = new Point ();
			MonteCarlo.ConcentricSampleDisk (u1, u2, ref p.x, ref p.y);
			p.x *= radius;
			p.y *= radius;

			var ns = this.ObjectToWorld.Apply (new Normal (0, 0, 1)).Normalized;
			
			if (this.ReverseOrientation)
				ns *= -1.0;
			Ns.x = ns.x;
			Ns.y = ns.y;
			Ns.z = ns.z;

			return this.ObjectToWorld.Apply (p);
		}

		public override Task<Point> SampleAsync (double u1, double u2, Normal Ns)
		{
			return new Task<Point>(() => this.Sample (u1, u2, Ns));
		}

		public static IShape CreateDiskShape (Transform o2w, Transform w2o, bool reverseOrientation, ParameterSet parameters)
		{
			var height = parameters.FindOneDouble ("height", 0.0);
			var radius = parameters.FindOneDouble ("radius", 1.0);
			var innerRadius = parameters.FindOneDouble ("innerradius", 0.0);
			var phimax = parameters.FindOneDouble ("phimax", 360.0);

			return new Disk (o2w, w2o, reverseOrientation, height, radius, innerRadius, phimax);
		}
	}
}
