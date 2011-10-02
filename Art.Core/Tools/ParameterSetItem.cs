using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Art.Core.Tools
{
	public class ParameterSetItem<T>
	{
		/// <summary>
		///
		/// </summary>
		public T[] Data;
		/// <summary>
		///
		/// </summary>
		public string Name;
		/// <summary>
		///
		/// </summary>
		public int NumberOfItems;
		/// <summary>
		///
		/// </summary>
		public bool LookedUp;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="values"></param>
		public ParameterSetItem (string name, T[] values)
		{
			this.Name = name;
			this.NumberOfItems = values.Length;
			this.Data = new T[NumberOfItems];
			values.CopyTo (this.Data, 0);
			this.LookedUp = false;
		}
	}
}
