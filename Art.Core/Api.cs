using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Art.Core
{
	/// <summary>
	/// 
	/// </summary>
	public static class Api
	{
		/// <summary>
		/// 
		/// </summary>
		private struct _IMAGE_FILE_HEADER
		{
			public ushort Machine;
			public ushort NumberOfSections;
			public uint TimeDateStamp;
			public uint PointerToSymbolTable;
			public uint NumberOfSymbols;
			public ushort SizeOfOptionalHeader;
			public ushort Characteristics;
		};

		/// <summary>
		/// 
		/// </summary>
		public const int MajorVersion = 0;
		/// <summary>
		/// 
		/// </summary>
		public const int MinorVersion = 0;
		/// <summary>
		/// 
		/// </summary>
		public const int BuildVersion = 1;
		/// <summary>
		/// 
		/// </summary>
		public static string Version { get { return string.Format ("{0}.{1}.{2}", MajorVersion, MinorVersion, BuildVersion); } }
		/// <summary>
		/// 
		/// </summary>
		public static readonly DateTime BuildDate = GetBuildDateTime (Assembly.GetExecutingAssembly ());

		/// <summary>
		/// 
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static DateTime GetBuildDateTime (Assembly assembly)
		{
			if (File.Exists (assembly.Location))
			{
				var buffer = new byte[Math.Max (Marshal.SizeOf (typeof (_IMAGE_FILE_HEADER)), 4)];
				using (var fileStream = new FileStream (assembly.Location, FileMode.Open, FileAccess.Read))
				{
					fileStream.Position = 0x3C;
					fileStream.Read (buffer, 0, 4);
					fileStream.Position = BitConverter.ToUInt32 (buffer, 0); // COFF header offset
					fileStream.Read (buffer, 0, 4); // "PE\0\0"
					fileStream.Read (buffer, 0, buffer.Length);
				}
				var pinnedBuffer = GCHandle.Alloc (buffer, GCHandleType.Pinned);
				try
				{
					var coffHeader = (_IMAGE_FILE_HEADER)Marshal.PtrToStructure (pinnedBuffer.AddrOfPinnedObject (), typeof (_IMAGE_FILE_HEADER));
					return TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1) + new TimeSpan (coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
				}
				finally
				{
					pinnedBuffer.Free ();
				}
			}
			return new DateTime ();
		}
	}
}
