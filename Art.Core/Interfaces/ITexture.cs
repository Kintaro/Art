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
	/// <typeparam name="T"></typeparam>
	public interface ITexture<T>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dg"></param>
		/// <returns></returns>
		T Evaluate (DifferentialGeometry dg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dg"></param>
		/// <returns></returns>
		Task<T> EvaluateAsync (DifferentialGeometry dg);
	}
}
