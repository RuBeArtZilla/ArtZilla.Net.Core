using System;
using System.Runtime.InteropServices;

namespace ArtZilla.Sharp.Lib.PInvoke {
	/// <summary>
	/// Second argument for function <see cref="User32.ShowWindowAsync"/> 
	/// </summary>
	public enum ShowWindowCommand {
		Hide = 0,
		Normal = 1,
		Minimized = 2,
		Maximized = 3,
		NoActivate = 4,
		Restore = 9,
		Default = 10,
	}

	public enum PostMessages {
		WM_CLOSE = 0x0010,
	}

	public static class User32 {
		public const string DllName = "user32.dll";

		[DllImport(DllName)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport(DllName)]
		public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

		public static void ShowWindowAsync(IntPtr hWnd, ShowWindowCommand nCmdShow)
			=> ShowWindowAsync(hWnd, (int) nCmdShow);

		[DllImport(DllName)]
		public static extern bool IsIconic(IntPtr hWnd);

		public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

		[DllImport(DllName)]
		public static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

		[DllImport(DllName, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		public static bool PostMessage(IntPtr hWnd, PostMessages msg, IntPtr wParam, IntPtr lParam)
			=> PostMessage(hWnd, (uint) msg, wParam, lParam);

	}
}