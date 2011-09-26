using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;

namespace Art.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public class ShapeSet
	{
		/// <summary>
		/// 
		/// </summary>
		private IList<IShape> shapes;
		/// <summary>
		/// 
		/// </summary>
		private double sumArea;
		/// <summary>
		/// 
		/// </summary>
		private IList<double> areas;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		public ShapeSet (IShape s)
		{
			this.shapes = new List<IShape> ();
			var todo = new Stack<IShape> ();
			todo.Push (s);

			while (todo.Count > 0)
			{
				var sh = todo.Pop ();
				if (sh.CanIntersect)
					shapes.Add (sh);
				else
					sh.Refine (todo.ToList ());
			}

			this.sumArea = this.shapes.Sum (x => { var area = x.Area;  this.areas.Add (area); return area; });
		}

		/// <summary>
		/// 
		/// </summary>
		public double Area { get { return this.sumArea; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="ls"></param>
		/// <param name="Ns"></param>
		/// <returns></returns>
		public Point Sample (Point p, LightSample ls, Normal Ns)
		{
			int sn = 0;
			var pt = this.shapes[sn].Sample (p, ls.uPos[0], ls.uPos[1], Ns);
			var r = new Ray (p, pt - p, 1e-3, double.PositiveInfinity);
			var tHit = 1.0;
			var rayEpsilon = 0.0;
			var dg = new DifferentialGeometry ();

			var anyHit = this.shapes.Any (x => x.Intersect (r, out tHit, out rayEpsilon, dg));
			if (anyHit)
			{
				Ns.x = dg.nn.x;
				Ns.y = dg.nn.y;
				Ns.z = dg.nn.z;
			}
			return r.Apply (tHit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="ls"></param>
		/// <param name="Ns"></param>
		/// <returns></returns>
		public async Task<Point> SampleAsync (Point p, LightSample ls, Normal Ns)
		{
			int sn = 0;
			var pt = await this.shapes[sn].SampleAsync (p, ls.uPos[0], ls.uPos[1], Ns);
			var r = new Ray (p, pt - p, 1e-3, double.PositiveInfinity);
			var tHit = 1.0;
			var rayEpsilon = 0.0;
			var dg = new DifferentialGeometry ();

			var anyHit = this.shapes.Any (x => x.Intersect (r, out tHit, out rayEpsilon, dg));
			if (anyHit)
			{
				Ns.x = dg.nn.x;
				Ns.y = dg.nn.y;
				Ns.z = dg.nn.z;
			}
			return r.Apply (tHit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ls"></param>
		/// <param name="Ns"></param>
		/// <returns></returns>
		public Point Sample (LightSample ls, Normal Ns)
		{
			int sn = 0;
			return this.shapes[sn].Sample (ls.uPos[0], ls.uPos[1], Ns);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ls"></param>
		/// <param name="Ns"></param>
		/// <returns></returns>
		public async Task<Point> SampleAsync (LightSample ls, Normal Ns)
		{
			int sn = 0;
			return await this.shapes[sn].SampleAsync (ls.uPos[0], ls.uPos[1], Ns);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="wi"></param>
		/// <returns></returns>
		public double Pdf (Point p, Vector wi)
		{
			var pdf = 0.0;
			for (var i = 0; i < this.shapes.Count; ++i)
				pdf += this.areas[i] * this.shapes[i].Pdf (p, wi);
			return pdf / this.sumArea;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public double Pdf (Point p)
		{
			var pdf = 0.0;
			for (var i = 0; i < this.shapes.Count; ++i)
				pdf += this.areas[i] * this.shapes[i].Pdf (p);
			return pdf / this.sumArea;
		}
	}
}

