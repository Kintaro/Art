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
	public abstract class IFilter
	{
		/// <summary>
		/// 
		/// </summary>
		public readonly double xWidth;
		/// <summary>
		/// 
		/// </summary>
		public readonly double yWidth;
		/// <summary>
		/// 
		/// </summary>
		public readonly double InvXWidth;
		/// <summary>
		/// 
		/// </summary>
		public readonly double InvYWidth;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xw"></param>
		/// <param name="yw"></param>
		protected IFilter (double xw, double yw)
		{
			this.xWidth = xw;
			this.yWidth = yw;
			this.InvXWidth = 1.0 / xw;
			this.InvYWidth = 1.0 / yw;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public abstract double Evaluate (double x, double y);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public abstract Task<double> EvaluateAsync (double x, double y);
	}
}
