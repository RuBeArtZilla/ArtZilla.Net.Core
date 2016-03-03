using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ArtZilla.Sharp.Lib.PInvoke {
	public static class Kernel32 {
		public const string DllName = "kernel32.dll";

		[DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
		static extern uint GetShortPathName(
			[MarshalAs(UnmanagedType.LPTStr)]string lpszLongPath,
			[MarshalAs(UnmanagedType.LPTStr)]StringBuilder lpszShortPath,
			uint cchBuffer);

		[DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
		static extern uint GetShortPathName(string lpszLongPath, char[] lpszShortPath, int cchBuffer);

		[DllImport(DllName)]
		public static extern bool AllocConsole();

		[DllImport(DllName)]
		public static extern bool FreeConsole();

		[DllImport(DllName)]
		public static extern IntPtr GetConsoleWindow();

		[DllImport(DllName)]
		public static extern int GetConsoleOutputCP();

		public static string GetShortPathName(string longpath) {
			var buffer = new char[256];
			GetShortPathName(longpath, buffer, buffer.Length);
			return new string(buffer);
		}
	}
}