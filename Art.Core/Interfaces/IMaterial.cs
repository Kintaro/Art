using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;
using Art.Core.Reflection;

namespace Art.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class IMaterial
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dgGeom"></param>
		/// <param name="dgShading"></param>
		/// <returns></returns>
		public abstract BSDF GetBSDF (DifferentialGeometry dgGeom, DifferentialGeometry dgShading);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dgGeom"></param>
		/// <param name="dgShading"></param>
		/// <returns></returns>
		public abstract Task<BSDF> GetBSDFAsync (DifferentialGeometry dgGeom, DifferentialGeometry dgShading);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <param name="dgGeom"></param>
		/// <param name="dgShading"></param>
		/// <param name="dgBump"></param>
		public static DifferentialGeometry Bump (ITexture<double> d, DifferentialGeometry dgGeom, DifferentialGeometry dgShading)
		{
			var dgBump = new DifferentialGeometry ();
			var dgEval = new DifferentialGeometry (dgShading);

			var du = .5 * (Math.Abs (dgShading.dudx) + Math.Abs (dgShading.dudy));
			if (du == 0.0)
				du = .01;

			dgEval.p = dgShading.p + du * dgShading.dpdu;
			dgEval.u = dgShading.u + du;
			dgEval.nn = null;// TODO

			var uDisplace = d.Evaluate (dgEval);

			var dv = .5 * (Math.Abs (dgShading.dvdx) + Math.Abs (dgShading.dvdy));
			//

			var vDisplace = d.Evaluate (dgEval);
			var displace = d.Evaluate (dgShading);

			dgBump = new DifferentialGeometry (dgShading);
			dgBump.dpdu = dgShading.dpdu + (uDisplace - displace) / du * new Vector (dgShading.nn) + displace * new Vector (dgShading.dndu);
			dgBump.dpdv = dgShading.dpdv + (vDisplace - displace) / dv * new Vector (dgShading.nn) + displace * new Vector (dgShading.dndv);

			if (dgShading.Shape.ReverseOrientation ^ dgShading.Shape.TransformSwapsHandedness)
				dgBump.nn *= -1.0;

			dgBump.nn = Util.FaceForward (dgBump.nn, dgGeom.nn);

			return dgBump;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <param name="dgGeom"></param>
		/// <param name="dgShading"></param>
		/// <param name="dgBump"></param>
		/// <returns></returns>
		public static async Task<DifferentialGeometry> BumpAsync (ITexture<double> d, DifferentialGeometry dgGeom, DifferentialGeometry dgShading)
		{
			var dgBump = new DifferentialGeometry ();
			var dgEval = new DifferentialGeometry (dgShading);

			var du = .5 * (Math.Abs (dgShading.dudx) + Math.Abs (dgShading.dudy));
			if (du == 0.0)
				du = .01;

			dgEval.p = dgShading.p + du * dgShading.dpdu;
			dgEval.u = dgShading.u + du;
			dgEval.nn = null;// TODO

			var uDisplace = await d.EvaluateAsync (dgEval);

			var dv = .5 * (Math.Abs (dgShading.dvdx) + Math.Abs (dgShading.dvdy));
			//

			var vDisplace = await d.EvaluateAsync (dgEval);
			var displace = await d.EvaluateAsync (dgShading);

			dgBump = new DifferentialGeometry (dgShading);
			dgBump.dpdu = dgShading.dpdu + (uDisplace - displace) / du * new Vector (dgShading.nn) + displace * new Vector (dgShading.dndu);
			dgBump.dpdv = dgShading.dpdv + (vDisplace - displace) / dv * new Vector (dgShading.nn) + displace * new Vector (dgShading.dndv);

			if (dgShading.Shape.ReverseOrientation ^ dgShading.Shape.TransformSwapsHandedness)
				dgBump.nn *= -1.0;

			dgBump.nn = Util.FaceForward (dgBump.nn, dgGeom.nn);

			return dgBump;
		}
	}
}
