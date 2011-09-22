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
	public static class Util
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double Dot (Vector a, Vector b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Vector contains Nans");
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double AbsDot (Vector a, Vector b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Vector contains Nans");
			return Math.Abs (a.x * b.x + a.y * b.y + a.z * b.z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double Dot (Normal a, Vector b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Vector contains Nans");
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double AbsDot (Normal a, Vector b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Vector contains Nans");
			return Math.Abs (a.x * b.x + a.y * b.y + a.z * b.z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double Dot (Vector a, Normal b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Vector contains Nans");
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double AbsDot (Vector a, Normal b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Vector contains Nans");
			return Math.Abs (a.x * b.x + a.y * b.y + a.z * b.z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double Dot (Normal a, Normal b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Normal contains Nans");
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double AbsDot (Normal a, Normal b)
		{
			Debug.Assert (!a.HasNans && !b.HasNans, @"Normal contains Nans");
			return Math.Abs (a.x * b.x + a.y * b.y + a.z * b.z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double DistanceSquared (Point a, Point b)
		{
			return (a - b).LengthSquared;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double Distance (Point a, Point b)
		{
			return (a - b).Length;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Normal FaceForward (Normal a, Vector b)
		{
			return Dot (a, b) < 0.0 ? -a : a;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Normal FaceForward (Normal a, Normal b)
		{
			return Dot (a, b) < 0.0 ? -a : a;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector FaceForward (Vector a, Vector b)
		{
			return Dot (a, b) < 0.0 ? -a : a;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector FaceForward (Vector a, Normal b)
		{
			return Dot (a, b) < 0.0 ? -a : a;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		public static double Lerp (double t, double v1, double v2)
		{
			return (1.0 - t) * v1 + t * v2;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <param name="low"></param>
		/// <param name="high"></param>
		/// <returns></returns>
		public static double Clamp(double val, double low, double high) 
		{
			if (val < low) return low;
			else if (val > high) return high;
			return val;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <param name="low"></param>
		/// <param name="high"></param>
		/// <returns></returns>
		public static int Clamp (int val, int low, int high)
		{
			if (val < low) return low;
			else if (val > high) return high;
			return val;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static double SphericalTheta (Vector v)
		{
			return Math.Acos (Clamp (v.z, -1.0, 1.0));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static double SphericalPhi (Vector v)
		{
			var p = Math.Atan2 (v.y, v.x);
			return p < 0.0 ? p + 2.0 + Math.PI : p;
		}
	}
}
