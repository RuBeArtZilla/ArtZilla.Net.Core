﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ArtZilla.Net.Core.PInvoke;

/// <summary>
/// KERNEL32.DLL exposes to applications most of the Win32 base APIs, such as memory management,
/// input/output (I/O) operations, process and thread creation, and synchronization functions.
/// Many of these are implemented within KERNEL32.DLL by calling corresponding functions in the native API,
/// exposed by NTDLL.DLL.
/// <para>https://en.wikipedia.org/wiki/Microsoft_Windows_library_files#KERNEL32.DLL</para>
/// </summary>
public static class Kernel32 {
	/// <summary> The name of the DLL that contains the unmanaged method. </summary>
	public const string DllName = "kernel32.dll";

	[DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
	public static extern uint GetShortPathName(
		[MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath,
		[MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszShortPath,
		uint cchBuffer
	);

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

	/// <summary> Use the console of the parent of the current process. </summary>
	public const uint ATTACH_PARENT_PROCESS = unchecked((uint) -1);

	/// <summary>
	/// Attaches the calling process to the console of the specified process as a client application.
	/// </summary>
	/// <param name="dwProcessId">
	///	<para>* <b>pid</b>: Use the console of the specified process.</para>
	/// <para>* <b>ATTACH_PARENT_PROCESS (DWORD)-1</b>: Use the console of the parent of the current process.</para>
	/// </param>
	/// <returns>
	/// <para>If the function succeeds, the return value is nonzero.</para>
	/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
	/// </returns>
	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool AttachConsole(uint dwProcessId);

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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="esFlags"></param>
	/// <remarks>
	/// There is no need to store the state you set, Windows remembers it for you.
	/// Just set it back to ES_CONTINUOUS when you don't want it anymore.
	/// Also note that this setting is per thread/application not global,
	/// so if you go to ES_CONTINUOUS and another app/thread is still setting ES_DISPLAY
	/// the display will be kept on.
	/// </remarks>
	/// <returns></returns>
	[DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
	public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

	public static string GetShortPathName(string longpath) {
		var buffer = new char[256];
		GetShortPathName(longpath, buffer, buffer.Length);
		return new string(buffer);
	}
}

[FlagsAttribute]
public enum EXECUTION_STATE : uint {
	/// <summary>
	/// Enables away mode. This value must be specified with ES_CONTINUOUS.
	/// Away mode should be used only by media-recording and media-distribution
	/// applications that must perform critical background processing on desktop
	/// computers while the computer appears to be sleeping. See Remarks.
	/// </summary>
	ES_AWAYMODE_REQUIRED = 0x00000040,
	/// <summary>
	/// Informs the system that the state being set should remain in effect until the next call
	/// that uses ES_CONTINUOUS and one of the other state flags is cleared.
	/// </summary>
	ES_CONTINUOUS = 0x80000000,

	/// <summary>
	/// Forces the display to be on by resetting the display idle timer.
	/// </summary>
	ES_DISPLAY_REQUIRED = 0x00000002,

	/// <summary>
	/// Forces the system to be in the working state by resetting the system idle timer.
	/// </summary>
	ES_SYSTEM_REQUIRED = 0x00000001

	// Legacy flag, should not be used.
	// ES_USER_PRESENT = 0x00000004
}
