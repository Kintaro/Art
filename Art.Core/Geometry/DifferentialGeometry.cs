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
	public class DifferentialGeometry
	{
		/// <summary>
		/// 
		/// </summary>
		public Point p;
		/// <summary>
		/// 
		/// </summary>
		public Normal nn;
		/// <summary>
		/// 
		/// </summary>
		public double u;
		/// <summary>
		/// 
		/// </summary>
		public double v;
		/// <summary>
		/// 
		/// </summary>
		public Vector dpdu;
		/// <summary>
		/// 
		/// </summary>
		public Vector dpdv;
		/// <summary>
		/// 
		/// </summary>
		public Vector dpdx;
		/// <summary>
		/// 
		/// </summary>
		public Vector dpdy;
		/// <summary>
		/// 
		/// </summary>
		public double dudx;
		/// <summary>
		/// 
		/// </summary>
		public double dvdx;
		/// <summary>
		/// 
		/// </summary>
		public double dudy;
		/// <summary>
		/// 
		/// </summary>
		public double dvdy;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		public void ComputeDifferentials (RayDifferential ray)
		{
			if (!ray.HasDifferentials)
			{
				dudx = dvdx = 0.0;
				dudy = dvdy = 0.0;
				dpdx = dpdy = new Vector (0, 0, 0);
				return;
			}

			var d = -Util.Dot (nn, new Vector (p.x, p.y, p.z));
			var rxv = new Vector (ray.rxOrigin.x, ray.rxOrigin.y, ray.rxOrigin.z);
			var tx = -(Util.Dot (nn, rxv) + d) / Util.Dot (nn, ray.rxDirection);

			if (double.IsNaN (tx))
			{
				dudx = dvdx = 0.0;
				dudy = dvdy = 0.0;
				dpdx = dpdy = new Vector (0, 0, 0);
				return;
			}

			var px = ray.rxOrigin + tx * ray.rxDirection;
			var ryv = new Vector (ray.ryOrigin.x, ray.ryOrigin.y, ray.ryOrigin.z);
			var ty = -(Util.Dot (nn, ryv) + d) / Util.Dot (nn, ray.ryDirection);

			if (double.IsNaN (ty))
			{
				dudx = dvdx = 0.0;
				dudy = dvdy = 0.0;
				dpdx = dpdy = new Vector (0, 0, 0);
				return;
			}

			var py = ray.ryOrigin + ty * ray.ryDirection;
			dpdx = px - p;
			dpdy = py - p;

			var A = new double[2][] { new double[2], new double[2] };
			var Bx = new double[2];
			var By = new double[2];
			var axes = new int[2];

			if (Math.Abs (nn.x) > Math.Abs (nn.y) && Math.Abs (nn.x) > Math.Abs (nn.z))
			{
				axes[0] = 1; axes[1] = 2;
			}
			else if (Math.Abs (nn.y) > Math.Abs (nn.z))
			{
				axes[0] = 0; axes[1] = 2;
			}
			else
			{
				axes[0] = 0; axes[1] = 1;
			}

			A[0][0] = dpdu[axes[0]];
			A[0][1] = dpdv[axes[0]];
			A[1][0] = dpdu[axes[1]];
			A[1][1] = dpdv[axes[1]];

			Bx[0] = px[axes[0]] - p[axes[0]];
			Bx[1] = px[axes[1]] - p[axes[1]];
			By[0] = py[axes[0]] - p[axes[0]];
			By[1] = py[axes[1]] - p[axes[1]];

			if (!Util.SolveLinearSystem2x2 (A, Bx, ref dudx, ref dvdx))
			{
				dudx = 0.0; dvdx = 0.0;
			}
			if (!Util.SolveLinearSystem2x2 (A, By, ref dudy, ref dvdy))
			{
				dudy = 0.0; dvdy = 0.0;
			}
		}
	}
}
