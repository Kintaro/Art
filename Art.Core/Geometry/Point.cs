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
	public sealed class Point
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
		public Point ()
		{
			this.x = this.y = this.z = 0.0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Point (double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public Point (Point p)
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
		public static Vector operator - (Point a, Point b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Point contains Nans");
			return new Vector (a.x - b.x, a.y - b.y, a.z - b.z);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point operator + (Point a, Vector b)
        {
            Debug.Assert(!a.HasNans && !b.HasNans, @"Point or vector contains Nans");
            return new Point (a.x + b.x, a.y + b.y, a.z + b.z);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Point operator + (Point a, Point b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Point contains Nans");
			return new Point (a.x + b.x, a.y + b.y, a.z + b.z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Point operator - (Point a, Vector b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Point or vector contains Nans");
			return new Point (a.x + b.x, a.y + b.y, a.z + b.z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="f"></param>
		/// <returns></returns>
		public static Point operator * (Point a, double f)
		{
			Debug.Assert (!a.HasNans, @"Point contains Nans");
			Debug.Assert (!double.IsNaN (f), @"Factor is NaN");
			return new Point (a.x * f, a.y * f, a.z * f);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="f"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Point operator * (double f, Point a)
		{
			Debug.Assert (!a.HasNans, @"Point contains Nans");
			Debug.Assert (!double.IsNaN (f), @"Factor is NaN");
			return new Point (a.x * f, a.y * f, a.z * f);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator == (Point a, Point b)
        {
            Debug.Assert (!a.HasNans && !b.HasNans, @"Point contains Nans");
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator != (Point a, Point b)
        {
            Debug.Assert (!a.HasNans && !b.HasNans, @"Point contains Nans");
            return !(a == b);
        }
	}
}
