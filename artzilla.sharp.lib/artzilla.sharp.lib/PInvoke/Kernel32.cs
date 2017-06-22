using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ArtZilla.Net.Core.PInvoke {
	public static class Kernel32 {
		public const String DllName = "kernel32.dll";

		[DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
		static extern UInt32 GetShortPathName(
			[MarshalAs(UnmanagedType.LPTStr)]String lpszLongPath,
			[MarshalAs(UnmanagedType.LPTStr)]StringBuilder lpszShortPath,
			UInt32 cchBuffer);

		[DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
		static extern UInt32 GetShortPathName(String lpszLongPath, Char[] lpszShortPath, Int32 cchBuffer);

		[DllImport(DllName)]
		public static extern Boolean AllocConsole();

		[DllImport(DllName)]
		public static extern Boolean FreeConsole();

		[DllImport(DllName)]
		public static extern IntPtr GetConsoleWindow();

		[DllImport(DllName)]
		public static extern Int32 GetConsoleOutputCP();

		public static String GetShortPathName(String longpath) {
			var buffer = new Char[256];
			GetShortPathName(longpath, buffer, buffer.Length);
			return new String(buffer);
		}
	}
}