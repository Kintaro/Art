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
	}
}
