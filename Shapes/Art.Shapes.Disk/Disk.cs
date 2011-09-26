using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;
using Art.Core.Interfaces;

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

		public override bool Intersect (Ray ray, out double tHit, out double rayEpsilon, DifferentialGeometry dg)
		{
			return base.Intersect (ray, out tHit, out rayEpsilon, dg);
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
			var ns = new Normal (Ns);
			// TODO
			if (this.ReverseOrientation)
				ns *= -1.0;
			Ns.x = ns.x;
			Ns.y = ns.y;
			Ns.z = ns.z;

			return null;
		}

		public override async Task<Point> SampleAsync (double u1, double u2, Normal Ns)
		{
			var p = new Point ();
			MonteCarlo.ConcentricSampleDisk (u1, u2, ref p.x, ref p.y);
			p.x *= radius;
			p.y *= radius;
			var ns = new Normal (Ns);
			// TODO
			if (this.ReverseOrientation)
				ns *= -1.0;
			Ns.x = ns.x;
			Ns.y = ns.y;
			Ns.z = ns.z;

			return null;
		}
	}
}
