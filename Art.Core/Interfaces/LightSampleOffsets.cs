using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Art.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public class LightSampleOffsets
	{
		/// <summary>
		/// 
		/// </summary>
		public int NumberOfSamples;
		/// <summary>
		/// 
		/// </summary>
		public int ComponentOffset;
		/// <summary>
		/// 
		/// </summary>
		public int PosOffset;

		/// <summary>
		/// 
		/// </summary>
		public LightSampleOffsets ()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="count"></param>
		/// <param name="sample"></param>
		public LightSampleOffsets (int count, Sample sample)
		{
			this.NumberOfSamples = count;
			this.ComponentOffset = sample.Add1D (count);
			this.PosOffset = sample.Add2D (count);
		}
	}
}
