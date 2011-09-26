using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Spectra;

namespace Art.Core.Reflection
{
	/// <summary>
	/// 
	/// </summary>
	public class BSSRDF
	{
		/// <summary>
		/// 
		/// </summary>
		private double e;
		/// <summary>
		/// 
		/// </summary>
		private Spectrum sigA;
		/// <summary>
		/// 
		/// </summary>
		private Spectrum sigpS;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sa"></param>
		/// <param name="sps"></param>
		/// <param name="et"></param>
		public BSSRDF (Spectrum sa, Spectrum sps, double et)
		{
			this.e = et;
			this.sigA = sa;
			this.sigpS = sps;
		}

		/// <summary>
		/// 
		/// </summary>
		public double Eta { get { return this.e; } }
		/// <summary>
		/// 
		/// </summary>
		public Spectrum SigmaA { get { return this.sigA; } }

		/// <summary>
		/// 
		/// </summary>
		public Spectrum SigmaPrimeS { get { return this.sigpS; } }
	}
}
