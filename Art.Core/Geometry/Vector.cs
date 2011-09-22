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
    public sealed class Vector
    {
		/// <summary>
		/// 
		/// </summary>
        public double x;
		/// <summary>
		/// 
		/// </summary>
        public double y;
		/// <summary>
		/// 
		/// </summary>
        public double z;

		/// <summary>
		/// 
		/// </summary>
		public Vector ()
		{
			this.x = this.y = this.z = 0.0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vector (double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		public Vector (Point p)
		{
			this.x = p.x;
			this.y = p.y;
			this.z = p.z;
		}

		/// <summary>
		/// 
		/// </summary>
        public bool HasNans
        {
            get { return double.IsNaN (x) || double.IsNaN (y) || double.IsNaN (y); }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public double this[int index]
		{
			get
			{
				Debug.Assert (x >= 0 && x <= 2, @"Index out of range");
				if (index == 0)
					return this.x;
				if (index == 1)
					return this.y;
				return this.z;
			}
			set
			{
				Debug.Assert (x >= 0 && x <= 2, @"Index out of range");
				if (index == 0)
					this.x = value;
				if (index == 1)
					this.y = value;
				if (index == 2)
					this.z = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
        public static Vector operator + (Vector a, Vector b)
        {
            Debug.Assert(!a.HasNans && !b.HasNans, @"Vector contains Nans");
            return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
        public static Vector operator - (Vector a, Vector b)
        {
            Debug.Assert(!a.HasNans && !b.HasNans, @"Vector contains Nans");
            return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="f"></param>
		/// <returns></returns>
        public static Vector operator * (Vector a, double f)
        {
			Debug.Assert (!a.HasNans, @"Vector contains Nans");
			Debug.Assert (!double.IsNaN(f), @"Factor is NaN");
            return new Vector(a.x * f, a.y * f, a.z * f);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="f"></param>
		/// <param name="a"></param>
		/// <returns></returns>
        public static Vector operator * (double f, Vector a)
        {
			Debug.Assert (!a.HasNans, @"Vector contains Nans");
			Debug.Assert (!double.IsNaN (f), @"Factor is NaN");
            return new Vector(a.x * f, a.y * f, a.z * f);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="f"></param>
		/// <returns></returns>
		public static Vector operator / (Vector a, double f)
		{
			Debug.Assert (!a.HasNans, @"Vector contains Nans");
			Debug.Assert (!double.IsNaN (f), @"Factor is NaN");
			Debug.Assert (f != 0.0, @"Factor cannot be zero");
			var inv = 1.0 / f;
			return a * inv;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
        public static Vector operator - (Vector a)
        {
	    	Debug.Assert (!a.HasNans, @"Vector contains Nans");
            return new Vector(-a.x, -a.y, -a.z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator == (Vector a, Vector b)
        {
            Debug.Assert (!a.HasNans && !b.HasNans, @"Vector contains Nans");
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator != (Vector a, Vector b)
        {
            Debug.Assert (!a.HasNans && !b.HasNans, @"Vector contains Nans");
            return !(a == b);
        }

		/// <summary>
		/// 
		/// </summary>
        public double LengthSquared { get { return this.x * this.x + this.y * this.y + this.z * this.z; } }

		/// <summary>
		/// 
		/// </summary>
        public double Length { get { return Math.Sqrt (this.LengthSquared); } }

		/// <summary>
		/// 
		/// </summary>
		public Vector Normalized { get { return this / Length; } }
    }
}
