using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.Core.Geometry
{
	/// <summary>
	/// 
	/// </summary>
    public class BoundingBox
    {
		/// <summary>
		/// 
		/// </summary>
        public Point pMin = new Point ();
		/// <summary>
		/// 
		/// </summary>
        public Point pMax = new Point ();

		/// <summary>
		/// 
		/// </summary>
        public BoundingBox ()
        {
            this.pMin = new Point (double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
            this.pMax = new Point (double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
        public BoundingBox (Point p)
        {
            this.pMin = new Point (p);
            this.pMax = new Point (p);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
        public BoundingBox (Point a, Point b)
        {
            this.pMin = new Point (Math.Min (a.x, b.x), Math.Min (a.y, b.y), Math.Min (a.z, b.z));
            this.pMax = new Point (Math.Max (a.x, b.x), Math.Max (a.y, b.y), Math.Max (a.z, b.z));
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="box"></param>
		public BoundingBox (BoundingBox box) : this(box.pMin, box.pMax)
		{
		}

		/// <summary>
		/// 
		/// </summary>
        public double SurfaceArea
        {
            get
            {
                var d = this.pMax - this.pMin;
                return 2.0 * (d.x * d.y + d.x * d.z + d.y * d.z);
            }
        }

		/// <summary>
		/// 
		/// </summary>
        public double Volume
        {
            get
            {
                var d = this.pMax - this.pMin;
                return d.x * d.y * d.z;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		public int MaximumExtent
		{
			get
			{
				var diag = this.pMax - this.pMin;
				if (diag.x > diag.y && diag.x > diag.z)
					return 0;
				else if (diag.y > diag.z)
					return 1;
				else
					return 2; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Point this[int index]
		{
			get
			{
				Debug.Assert (index == 0 || index == 1);
				return index == 0 ? this.pMin : this.pMax;
			}
			set
			{
				Debug.Assert (index == 0 || index == 1);
				if (index == 0)
					this.pMin = value;
				else
					this.pMax = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="b"></param>
		/// <param name="p"></param>
		/// <returns></returns>
		public static BoundingBox Union (BoundingBox b, Point p)
		{
			var ret = new BoundingBox(b);

			ret.pMin.x = Math.Min (b.pMin.x, p.x);
			ret.pMin.y = Math.Min (b.pMin.y, p.y);
			ret.pMin.z = Math.Min (b.pMin.z, p.z);
			ret.pMax.x = Math.Max (b.pMax.x, p.x);
			ret.pMax.y = Math.Max (b.pMax.y, p.y);
			ret.pMax.z = Math.Max (b.pMax.z, p.z);

			return ret;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="b"></param>
		/// <param name="b2"></param>
		/// <returns></returns>
		public static BoundingBox Union (BoundingBox b, BoundingBox b2)
		{
			var ret = new BoundingBox(b);
			ret.pMin.x = Math.Min (b.pMin.x, b2.pMin.x);
			ret.pMin.y = Math.Min (b.pMin.y, b2.pMin.y);
			ret.pMin.z = Math.Min (b.pMin.z, b2.pMin.z);
			ret.pMax.x = Math.Max (b.pMax.x, b2.pMax.x);
			ret.pMax.y = Math.Max (b.pMax.y, b2.pMax.y);
			ret.pMax.z = Math.Max (b.pMax.z, b2.pMax.z);
			return ret;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tx"></param>
		/// <param name="ty"></param>
		/// <param name="tz"></param>
		/// <returns></returns>
        public Point Lerp (double tx, double ty, double tz)
        {
            return new Point (Util.Lerp (tx, this.pMin.x, this.pMax.x), 
                Util.Lerp (ty, this.pMin.y, this.pMax.y), 
                Util.Lerp (tz, this.pMin.z, this.pMax.z));
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public bool Inside (Point pt)
		{
			return (pt.x >= this.pMin.x && pt.x <= this.pMax.x &&
				pt.y >= this.pMin.y && pt.y <= this.pMax.y &&
				pt.z >= this.pMin.z && pt.z <= this.pMax.z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public bool Overlaps (BoundingBox b)
		{
			var x = (this.pMax.x >= b.pMin.x) && (this.pMin.x <= b.pMax.x);
			var y = (this.pMax.y >= b.pMin.y) && (this.pMin.y <= b.pMax.y);
			var z = (this.pMax.z >= b.pMin.z) && (this.pMin.z <= b.pMax.z);

			return (x && y && z); 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="delta"></param>
		public void Expand (double delta)
		{
			this.pMin -= new Vector (delta, delta, delta);
			this.pMax += new Vector (delta, delta, delta);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		public void BoundingSphere (out Point center, out double radius)
		{
			center = .5 * this.pMin + .5 * this.pMax;
			radius = this.Inside (center) ? Util.Distance (center, this.pMax) : 0.0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		/// <param name="hitt0"></param>
		/// <param name="hitt1"></param>
		/// <returns></returns>
		public bool IntersectP (Ray ray, ref double hitt0, ref double hitt1)
		{
			var t0 = ray.MinT;
			var t1 = ray.MaxT;

			for (var i = 0; i < 3; ++i)
			{
				var invRayDir = 1.0 / ray.Direction[i];
				var tNear = (this.pMin[i] - ray.Origin[i]) * invRayDir;
				var tFar  = (this.pMax[i] - ray.Origin[i]) * invRayDir;

				if (tNear > tFar)
				{
					var temp = tNear;
					tNear = tFar;
					tFar = temp;
				}

				t0 = tNear > t0 ? tNear : t0;
				t1 = tFar  < t1 ? tFar : t1;

				if (t0 > t1)
					return false;
			}

			if (hitt0 != null)
				hitt0 = t0;
			if (hitt1 != null)
				hitt1 = t1;

			return true;
		}
    }
}
