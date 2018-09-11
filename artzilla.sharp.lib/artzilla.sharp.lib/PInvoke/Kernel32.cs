using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ArtZilla.Net.Core.PInvoke {
	public static class Kernel32 {
		public const string DllName = "kernel32.dll";

		[DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint GetShortPathName(
			[MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath,
			[MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszShortPath,
			uint cchBuffer);

		[DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint GetShortPathName(string lpszLongPath, char[] lpszShortPath, int cchBuffer);

		// To ensure that paths are not limited to MAX_PATH, use this signature within .NET
		[DllImport(DllName, CharSet = CharSet.Unicode, EntryPoint = "GetShortPathNameW", SetLastError = true)]
		public static extern int GetShortPathName(string pathName, StringBuilder shortName, int cbShortName);

		/// <summary> Allocates a new console for the calling process. </summary>
		/// <returns>
		/// If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero.To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(DllName)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AllocConsole();

		/// <summary> Detaches the calling process from its console. </summary>
		/// <returns>
		/// If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero.To get extended error information, call GetLastError.
	  /// </returns>
		[DllImport(DllName, SetLastError = true, ExactSpelling = true)]
		public static extern bool FreeConsole();

		/// <summary> Retrieves the window handle used by the console associated with the calling process. </summary>
		/// <returns>
		/// The return value is a handle to the window used by the console associated with the calling
		/// process or NULL if there is no such associated console.
		/// </returns>
		[DllImport(DllName)]
		public static extern IntPtr GetConsoleWindow();

		/// <summary>
		/// Retrieves the output code page used by the console associated with the calling process.
		/// A console uses its output code page to translate the character values written by the
		/// various output functions into the images displayed in the console window.
		/// </summary>
		/// <returns>
		/// The return value is a code that identifies the code page.
		/// For a list of identifiers, see Code Page Identifiers.
		/// </returns>
		[DllImport(DllName)]
		public static extern int GetConsoleOutputCP();

		/// <summary> Retrieves the thread identifier of the calling thread. </summary>
		/// <remarks>Until the thread terminates, the thread identifier uniquely identifies the thread throughout the system.</remarks>
		/// <returns>The return value is the thread identifier of the calling thread.</returns>
		[DllImport(DllName)]
		public static extern uint GetCurrentThreadId();

		/// <summary> Retrieves the process identifier of the calling process. </summary>
		/// <remarks>Until the process terminates, the process identifier uniquely identifies the process throughout the system.</remarks>
		/// <returns>The return value is the process identifier of the calling process.</returns>
		[DllImport(DllName)]
		internal static extern uint GetCurrentProcessId();
		
		public static String GetShortPathName(String longpath) {
			var buffer = new Char[256];
			GetShortPathName(longpath, buffer, buffer.Length);
			return new String(buffer);
		}
	}
}