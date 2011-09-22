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
    public abstract class ICamera
    {
		/// <summary>
		/// 
		/// </summary>
		public double ShutterOpen;
		/// <summary>
		/// 
		/// </summary>
		public double ShutterClose;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sample"></param>
		/// <param name="ray"></param>
		/// <returns></returns>
		public abstract double GenerateRay (CameraSample sample, Ray ray);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sample"></param>
		/// <param name="ray"></param>
		/// <returns></returns>
		public abstract Task<double> GenerateRayAsync (CameraSample sample, Ray ray);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sample"></param>
		/// <param name="rd"></param>
		/// <returns></returns>
		public double GenerateRayDifferential (CameraSample sample, RayDifferential rd)
		{
			var wt = GenerateRay (sample, rd);
			var sshift = sample;
			++(sshift.ImageX);
			var rx = new Ray ();
			var wtx = GenerateRay (sshift, rx);
			rd.rxOrigin = rx.Origin;
			rd.rxDirection = rx.Direction;

			--(sshift.ImageX);
			++(sshift.ImageY);
			var ry = new Ray ();
			var wty = GenerateRay (sshift, ry);
			rd.ryOrigin = ry.Origin;
			rd.ryDirection = ry.Direction;
			if (wtx == 0.0 || wty == 0.0) return 0.0;
			rd.HasDifferentials = true;
			return wt;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sample"></param>
		/// <param name="rd"></param>
		/// <returns></returns>
		public async Task<double> GenerateRayDifferentialAsync (CameraSample sample, RayDifferential rd)
		{
			var wt = GenerateRayAsync (sample, rd);
			var sshift = sample;
			++(sshift.ImageX);
			var rx = new Ray ();
			var wtx = GenerateRayAsync (sshift, rx);
			rd.rxOrigin = rx.Origin;
			rd.rxDirection = rx.Direction;

			--(sshift.ImageX);
			++(sshift.ImageY);
			var ry = new Ray ();
			var wty = GenerateRayAsync (sshift, ry);
			rd.ryOrigin = ry.Origin;
			rd.ryDirection = ry.Direction;
			if (await wtx == 0.0 || await wty == 0.0) return 0.0;
			rd.HasDifferentials = true;
			return await wt;
		}
    }
}
