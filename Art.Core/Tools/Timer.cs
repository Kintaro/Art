using System;

namespace Art.Core.Tools
{
	/// <summary>
	/// 
	/// </summary>
	public class Timer
	{
		/// <summary>
		/// 
		/// </summary>
		private double time0, elapsed;
		/// <summary>
		/// 
		/// </summary>
		private bool running;

		/// <summary>
		/// 
		/// </summary>
		public Timer ()
		{
			time0 = elapsed = 0.0;
			running = false;
		}

		/// <summary>
		///		Start the timer
		/// </summary>
		public void Start ()
		{
			running = true;
			time0 = GetTime ();
		}

		/// <summary>
		///		Stop the timer
		/// </summary>
		public void Stop ()
		{
			running = false;
			elapsed += GetTime () - time0;
		}

		/// <summary>
		///		Reset the timer
		/// </summary>
		public void Reset ()
		{
			running = false;
			elapsed = 0;
		}

		/// <summary>
		///		Get the elapsed time
		/// </summary>
		public double Time
		{
			get
			{
				if (running)
				{
					Stop ();
					Start ();
				}
				return elapsed;
			}
		}

		/// <summary>
		///		Get the time
		/// </summary>
		/// <returns></returns>
		private double GetTime ()
		{
			return DateTime.Now.Second + DateTime.Now.Millisecond / 1000000.0;
		}
	}
}