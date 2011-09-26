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
		public LightSample () { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sample"></param>
		/// <param name="offsets"></param>
		/// <param name="num"></param>
		public LightSample (Sample sample, LightSampleOffsets offsets, int num)
		{
		}
	}
}
