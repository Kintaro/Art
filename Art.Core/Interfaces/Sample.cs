using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public class Sample : CameraSample
    {
		/// <summary>
		/// 
		/// </summary>
		public List<int> n1D = new List<int> ();
		/// <summary>
		/// 
		/// </summary>
		public List<int> n2D = new List<int> ();
		/// <summary>
		/// 
		/// </summary>
		public int oneD;
		/// <summary>
		/// 
		/// </summary>
		public int twoD;
		/// <summary>
		/// 
		/// </summary>
		public double[][] samples;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		public Sample (ISampler sampler, ISurfaceIntegrator surfaceIntegrator, IVolumeIntegrator volumeIntegrator, Scene scene)
		{
			if (surfaceIntegrator != null)
				surfaceIntegrator.RequestSamples (sampler, this, scene);
			if (volumeIntegrator != null)
				volumeIntegrator.RequestSamples (sampler, this, scene);
			this.AllocateSampleMemory ();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public int Add1D (int num)
		{
			this.n1D.Add (num);
			return this.n1D.Count - 1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public int Add2D (int num)
		{
			this.n2D.Add (num);
			return this.n2D.Count - 1;
		}

		/// <summary>
		/// 
		/// </summary>
		private void AllocateSampleMemory ()
		{
			var nPtrs = n1D.Count + n2D.Count;

			if (nPtrs == 0)
			{
				samples = null;
				return;
			}

			samples = new double[nPtrs][];
			oneD = 0;
			twoD = oneD + n1D.Count;

			var totSamples = 0;
			for (var i = 0; i < n1D.Count; ++i)
				totSamples += n1D[i];
			for (var i = 0; i < n2D.Count; ++i)
				totSamples += n2D[i];

			for (var i = 0; i < n1D.Count; ++i)
			{
				samples[oneD + i] = new double[n1D[i]];
			}

			for (var i = 0; i < n2D.Count; ++i)
			{
				samples[twoD + i] = new double[2 * n2D[i]];
			}
		}
    }
}
