using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.Core.Geometry
{
	/// <summary>
	/// 
	/// </summary>
	public class Transform
	{
		/// <summary>
		/// 
		/// </summary>
		private Matrix m = new Matrix ();
		/// <summary>
		/// 
		/// </summary>
		private Matrix mInv = new Matrix ();

		/// <summary>
		/// 
		/// </summary>
		public Transform ()
			: this (new Matrix (), new Matrix ())
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mat"></param>
		public Transform (Matrix mat)
		{
			this.m = new Matrix (mat);
			this.mInv = new Matrix (mat.Inverse);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mat"></param>
		/// <param name="inverse"></param>
		public Transform (Matrix mat, Matrix inverse)
		{
			this.m = new Matrix (mat);
			this.mInv = new Matrix (inverse);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="transform"></param>
		public Transform (Transform transform)
			: this (transform.Matrix, transform.InverseMatrix)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public Transform Transposed
		{
			get { return new Transform (this.m.Transposed, this.mInv.Transposed); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Transform Inverse
		{
			get { return new Transform (this.mInv, this.m); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Matrix Matrix { get { return this.m; } }

		/// <summary>
		/// 
		/// </summary>
		public Matrix InverseMatrix { get { return this.mInv; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="delta"></param>
		/// <returns></returns>
		public Transform Translate (Vector delta)
		{
			var m = new Matrix (1, 0, 0, delta.x,
				0, 1, 0, delta.y,
				0, 0, 1, delta.z,
				0, 0, 0, 1);
			var minv = new Matrix (1, 0, 0, -delta.x,
						   0, 1, 0, -delta.y,
						   0, 0, 1, -delta.z,
						   0, 0, 0, 1);
			return new Transform (m, minv);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public Transform Scale (double x, double y, double z)
		{
			var m = new Matrix (x, 0, 0, 0,
				0, y, 0, 0,
				0, 0, z, 0,
				0, 0, 0, 1);
			var minv = new Matrix (1.0 / x, 0, 0, 0,
						   0, 1.0 / y, 0, 0,
						   0, 0, 1.0 / z, 0,
						   0, 0, 0, 1);
			return new Transform (m, minv);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public Point Apply (Point pt)
		{
			var x = pt.x;
			var y = pt.y;
			var z = pt.z;

			var xp = this.m[0, 0] * x + this.m[0, 1] * y + this.m[0, 2] * z + this.m[0, 3];
			var yp = this.m[1, 0] * x + this.m[1, 1] * y + this.m[1, 2] * z + this.m[1, 3];
			var zp = this.m[2, 0] * x + this.m[2, 1] * y + this.m[2, 2] * z + this.m[2, 3];
			var wp = this.m[3, 0] * x + this.m[3, 1] * y + this.m[3, 2] * z + this.m[3, 3];

			if (wp == 1.0) return new Point (xp, yp, zp);
			else return new Point (xp, yp, zp) * (1.0 / wp);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="pt"></param>
		public void Apply (Point p, ref Point pt)
		{
			var x = pt.x;
			var y = pt.y;
			var z = pt.z;

			pt.x = this.m[0, 0] * x + this.m[0, 1] * y + this.m[0, 2] * z + this.m[0, 3];
			pt.y = this.m[1, 0] * x + this.m[1, 1] * y + this.m[1, 2] * z + this.m[1, 3];
			pt.z = this.m[2, 0] * x + this.m[2, 1] * y + this.m[2, 2] * z + this.m[2, 3];
			var w = this.m[3, 0] * x + this.m[3, 1] * y + this.m[3, 2] * z + this.m[3, 3];

			if (w != 1.0)
				pt *= 1.0 / w;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <param name="vt"></param>
		public void Apply (Vector v, ref Vector vt)
		{
			var x = v.x;
			var y = v.y;
			var z = v.z;

			vt.x = this.m[0, 0] * x + this.m[0, 1] * y + this.m[0, 2] * z;
			vt.y = this.m[1, 0] * x + this.m[1, 1] * y + this.m[1, 2] * z;
			vt.z = this.m[2, 0] * x + this.m[2, 1] * y + this.m[2, 2] * z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public Normal Apply (Normal n)
		{
			var x = n.x;
			var y = n.y;
			var z = n.z;
			return new Normal (this.mInv[0, 0] * x + this.mInv[1, 0] * y + this.mInv[2, 0] * z,
						  this.mInv[0, 1] * x + this.mInv[1, 1] * y + this.mInv[2, 1] * z,
						  this.mInv[0, 2] * x + this.mInv[1, 2] * y + this.mInv[2, 2] * z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="r"></param>
		/// <param name="rt"></param>
		public void Apply (Ray r, ref Ray rt)
		{
			this.Apply (r.Origin, ref rt.Origin);
			this.Apply (r.Direction, ref rt.Direction);
			if (rt != r)
			{
				rt.MinT = r.MinT;
				rt.MaxT = r.MaxT;
				rt.Time = r.Time;
				rt.Depth = r.Depth;
			}
		}
	}
}
