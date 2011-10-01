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
	public class LightSample
	{
		/// <summary>
		/// 
		/// </summary>
		public double[] uPos = new double[2];
		/// <summary>
		/// 
		/// </summary>
		public double uComponent;

		/// <summary>
		/// 
		/// </summary>
		public LightSample () 
		{
			this.uPos[0] = Api.Random.NextDouble ();
			this.uPos[1] = Api.Random.NextDouble ();
			this.uComponent = Api.Random.NextDouble ();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sample"></param>
		/// <param name="offsets"></param>
		/// <param name="num"></param>
		public LightSample (Sample sample, LightSampleOffsets offsets, int num)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="up0"></param>
		/// <param name="up1"></param>
		/// <param name="ucomp"></param>
		public LightSample (double up0, double up1, double ucomp)
		{
			this.uPos[0] = up0;
			this.uPos[1] = up1;
			this.uComponent = ucomp;
		}
	}
}
