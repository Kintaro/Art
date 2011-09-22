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
	public struct Matrix
	{
		/// <summary>
		/// 
		/// </summary>
		public double[] m;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		public Matrix (double[] v)
		{
			this.m = new double[16];
			v.CopyTo (m, 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="matrix"></param>
		public Matrix (Matrix matrix)
			: this (matrix.m)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t00"></param>
		/// <param name="t01"></param>
		/// <param name="t02"></param>
		/// <param name="t03"></param>
		/// <param name="t10"></param>
		/// <param name="t11"></param>
		/// <param name="t12"></param>
		/// <param name="t13"></param>
		/// <param name="t20"></param>
		/// <param name="t21"></param>
		/// <param name="t22"></param>
		/// <param name="t23"></param>
		/// <param name="t30"></param>
		/// <param name="t31"></param>
		/// <param name="t32"></param>
		/// <param name="t33"></param>
		public Matrix (double t00, double t01, double t02, double t03, double t10, double t11, double t12, double t13, double t20, double t21,
		double t22, double t23, double t30, double t31, double t32, double t33)
		{
			this.m = new double[16];
			m[0] = t00;
			m[1] = t01;
			m[2] = t02;
			m[3] = t03;
			m[4] = t10;
			m[5] = t11;
			m[6] = t12;
			m[7] = t13;
			m[8] = t20;
			m[9] = t21;
			m[10] = t22;
			m[11] = t23;
			m[12] = t30;
			m[13] = t31;
			m[14] = t32;
			m[15] = t33;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		public static Matrix operator * (Matrix m1, Matrix m2)
		{
			Debug.Assert (m1.m.Length == 16 && m2.m.Length == 16);
			var result = new Matrix ();

			for (var i = 0; i < 4; ++i)
			{
				for (var j = 0; j < 4; ++j)
				{
					result.m[i * 4 + j] =
						m1.m[i * 4 + 0] * m2.m[0 * 4 + j] +
						m1.m[i * 4 + 1] * m2.m[1 * 4 + j] +
						m1.m[i * 4 + 2] * m2.m[2 * 4 + j] +
						m1.m[i * 4 + 3] * m2.m[3 * 4 + j];
				}
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		public Matrix Transposed
		{
			get
			{
				return new Matrix (m[0], m[4], m[8], m[12],
								   m[1], m[5], m[9], m[13],
								   m[2], m[6], m[10], m[14],
								   m[3], m[7], m[11], m[15]);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Matrix Inverse
		{
			get
			{
				int[] indxc = new int[4], indxr = new int[4];
				int[] ipiv = new int[] { 0, 0, 0, 0 };
				double[] minv = new double[16];
				m.CopyTo (minv, 0);

				for (int i = 0; i < 4; i++)
				{
					int irow = -1, icol = -1;
					double big = 0.0;
					// Choose pivot
					for (int j = 0; j < 4; j++)
					{
						if (ipiv[j] != 1)
						{
							for (int k = 0; k < 4; k++)
							{
								if (ipiv[k] == 0)
								{
									if (Math.Abs (minv[j * 4 + k]) >= big)
									{
										big = Math.Abs (minv[j * 4 + k]);
										irow = j;
										icol = k;
									}
								}
								else if (ipiv[k] > 1)
									throw new Exception ("Singular matrix in MatrixInvert");
							}
						}
					}
					++ipiv[icol];
					// Swap rows _irow_ and _icol_ for pivot
					if (irow != icol)
					{
						for (int k = 0; k < 4; ++k)
						{
							double t = minv[irow * 4 + k];
							minv[irow * 4 + k] = minv[icol * 4 + k];
							minv[icol * 4 + k] = t;
						}
					}
					indxr[i] = irow;
					indxc[i] = icol;
					if (minv[icol * 4 + icol] == 0.0)
						throw new Exception ("Singular matrix in MatrixInvert");

					// Set $m[icol][icol]$ to one by scaling row _icol_ appropriately
					double pivinv = 1.0 / minv[icol * 4 + icol];
					minv[icol * 4 + icol] = 1.0;
					for (int j = 0; j < 4; j++)
						minv[icol * 4 + j] *= pivinv;

					// Subtract this row from others to zero out their columns
					for (int j = 0; j < 4; j++)
					{
						if (j != icol)
						{
							double save = minv[j * 4 + icol];
							minv[j * 4 + icol] = 0;
							for (int k = 0; k < 4; k++)
								minv[j * 4 + k] -= minv[icol * 4 + k] * save;
						}
					}
				}
				// Swap columns to reflect permutation
				for (int j = 3; j >= 0; j--)
				{
					if (indxr[j] != indxc[j])
					{
						for (int k = 0; k < 4; k++)
						{
							double t = minv[k * 4 + indxr[j]];
							minv[k * 4 + indxr[j]] = minv[k * 4 + indxc[j]];
							minv[k * 4 + indxc[j]] = t;
						}
					}
				}
				return new Matrix (minv);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		public static bool operator == (Matrix m1, Matrix m2)
		{
			for (int i = 0; i < 16; ++i)
			{
				if (m1.m[i] != m2.m[i])
					return false;
			}
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		public static bool operator != (Matrix m1, Matrix m2)
		{
			for (int i = 0; i < 16; ++i)
			{
				if (m1.m[i] != m2.m[i])
					return true;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals (object obj)
		{
			return this == (Matrix)obj;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		} 
	}
}
