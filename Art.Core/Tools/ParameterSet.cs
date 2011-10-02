using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.Core.Tools
{
	/// <summary>
	/// 
	/// </summary>
	public class ParameterSet
	{
		/// <summary>
		///
		/// </summary>
		private List<ParameterSetItem<double>> doubles = new List<ParameterSetItem<double>> ();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public double FindOneDouble (string name, double def)
		{
			return LookupOne<double> (name, doubles, def);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="list"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		private T LookupOne<T> (string name, List<ParameterSetItem<T>> list, T def)
		{
			foreach (var parameter in list)
			{
				if (parameter.Name == name && parameter.NumberOfItems == 1)
				{
					parameter.LookedUp = true;
					return parameter.Data[0];
				}
			}
			return def;
		}
	}
}
