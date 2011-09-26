using System;
using System.Threading;

namespace Art.Core.Tools
{
	/// <summary>
	///
	/// </summary>
	public class ProgressReporter
	{
		/// <summary>
		/// 
		/// </summary>
		private double frequency;
		/// <summary>
		/// 
		/// </summary>
		private double count;
		/// <summary>
		/// 
		/// </summary>
		private int plussesPrinted;
		/// <summary>
		/// 
		/// </summary>
		private int totalPlusses;
		/// <summary>
		/// 
		/// </summary>
		private int left;
		/// <summary>
		/// 
		/// </summary>
		private int right;
		/// <summary>
		/// 
		/// </summary>
		private Timer timer;
		/// <summary>
		/// 
		/// </summary>
		private Mutex MutexLock = new Mutex ();

		/// <summary>
		///
		/// </summary>
		/// <param name="totalWork">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="title">
		/// A <see cref="System.String"/>
		/// </param>
		public ProgressReporter (int totalWork, string title)
			: this (totalWork, title, 50)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="totalWork">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="title">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="barLength">
		/// A <see cref="System.Int32"/>
		/// </param>
		public ProgressReporter (int totalWork, string title, int barLength)
		{
			plussesPrinted = 0;
			totalPlusses = barLength - title.Length;
			frequency = (double)totalWork / (double)totalPlusses;
			count = frequency;
			timer = new Timer ();
			timer.Start ();

			Console.Write ("  > " + title + ": [");
			left = Console.CursorLeft;
			right = left + totalPlusses - 1;
			Console.CursorLeft = right;
			Console.Write ("]");
			Console.CursorLeft = left;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="num">
		/// A <see cref="System.Int32"/>
		/// </param>
		public void Update (int num = 1)
		{
			lock (MutexLock)
			{
				count -= num;

				Console.CursorLeft = left;
				while (count <= 0)
				{
					count += frequency;
					if (plussesPrinted++ < totalPlusses - 1)
						Console.Write ("+");
					++left;
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		public void Done ()
		{
			lock (MutexLock)
			{
				Console.WriteLine ();
			}
		}
	}
}