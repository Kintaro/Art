using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.Core.Geometry
{
	public sealed class Normal
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
		public Normal ()
		{
			this.x = this.y = this.z = 0.0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Normal (double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n"></param>
		public Normal (Normal n)
			: this (n.x, n.y, n.z)
		{
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
		/// <param name="a"></param>
		/// <returns></returns>
		public static Normal operator - (Normal a)
		{
			return new Normal (-a.x, -a.y, -a.z);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Normal operator * (Normal a, double f)
        {
            Debug.Assert (!a.HasNans, @"Vector contains Nans");
            Debug.Assert (!double.IsNaN (f), @"Factor is NaN");
            return new Normal (a.x * f, a.y * f, a.z * f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Normal operator * (double f, Normal a)
        {
            Debug.Assert (!a.HasNans, @"Vector contains Nans");
            Debug.Assert (!double.IsNaN (f), @"Factor is NaN");
            return new Normal (a.x * f, a.y * f, a.z * f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Normal operator / (Normal a, double f)
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
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator == (Normal a, Normal b)
        {
            Debug.Assert (!a.HasNans && !b.HasNans, @"Normal contains Nans");
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator != (Normal a, Normal b)
        {
            Debug.Assert (!a.HasNans && !b.HasNans, @"Normal contains Nans");
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
        public Normal Normalized { get { return this / Length; } }
	}
}
