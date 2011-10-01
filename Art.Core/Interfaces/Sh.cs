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
	public static class Sh
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lmax"></param>
		/// <returns></returns>
		public static int SHTerms (int lmax)
		{
			return (lmax + 1) * (lmax + 1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="l"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		public static int SHIndex (int l, int m)
		{
			return l * l + l + m;
		}
	}
}
