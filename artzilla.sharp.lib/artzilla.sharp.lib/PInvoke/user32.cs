using System;
using System.Runtime.InteropServices;

namespace ArtZilla.Net.Core.PInvoke {
	/// <summary> Second argument for function <see cref="User32.ShowWindowAsync"/> </summary>
	public enum ShowWindowCommand {
		Hide = 0,
		Normal = 1,
		Minimized = 2,
		Maximized = 3,
		NoActivate = 4,
		Restore = 9,
		Default = 10,
	}

	public static class WM {
		public const int Close = 0x0010;
		public const int Quit = 0x0012;
		public const int ShowWindow = 0x0018;
	}

	public enum PostMessages {
		WM_CLOSE = WM.Close,
		WM_QUIT = WM.Quit,
		WM_SHOWWINDOW = WM.ShowWindow,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT {
		public int X;
		public int Y;

		public POINT(int x, int y) {
			X = x;
			Y = y;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MSG {
		public IntPtr hWnd;
		public uint message;
		public IntPtr wParam;
		public IntPtr lParam;
		public uint time;
		public POINT pt;
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


		/// <summary>
		/// The GetMessage function retrieves a message from the calling thread's 
		/// message queue. The function dispatches incoming sent messages until a 
		/// posted message is available for retrieval. 
		/// </summary>
		/// <param name="lpMsg">
		/// Pointer to an MSG structure that receives message information from 
		/// the thread's message queue.
		/// </param>
		/// <param name="hWnd">
		/// Handle to the window whose messages are to be retrieved.
		/// </param>
		/// <param name="wMsgFilterMin">
		/// Specifies the integer value of the lowest message value to be 
		/// retrieved. 
		/// </param>
		/// <param name="wMsgFilterMax">
		/// Specifies the integer value of the highest message value to be 
		/// retrieved.
		/// </param>
		/// <returns></returns>
		[DllImport(DllName)]
		public static extern bool GetMessage(
			out MSG lpMsg,
			IntPtr hWnd,
			uint wMsgFilterMin,
			uint wMsgFilterMax);

		/// <summary>
		/// The TranslateMessage function translates virtual-key messages into 
		/// character messages. The character messages are posted to the calling 
		/// thread's message queue, to be read the next time the thread calls the 
		/// GetMessage or PeekMessage function.
		/// </summary>
		/// <param name="lpMsg"></param>
		/// <returns></returns>
		[DllImport(DllName)]
		public static extern bool TranslateMessage([In] ref MSG lpMsg);

		/// <summary>
		/// The DispatchMessage function dispatches a message to a window 
		/// procedure. It is typically used to dispatch a message retrieved by 
		/// the GetMessage function.
		/// </summary>
		/// <param name="lpMsg"></param>
		/// <returns></returns>
		[DllImport(DllName)]
		public static extern IntPtr DispatchMessage([In] ref MSG lpMsg);

		/// <summary>
		/// The PostThreadMessage function posts a message to the message queue 
		/// of the specified thread. It returns without waiting for the thread to 
		/// process the message.
		/// </summary>
		/// <param name="idThread">
		/// Identifier of the thread to which the message is to be posted.
		/// </param>
		/// <param name="msg">Specifies the type of message to be posted.</param>
		/// <param name="wParam">
		/// Specifies additional message-specific information.
		/// </param>
		/// <param name="lParam">
		/// Specifies additional message-specific information.
		/// </param>
		/// <returns></returns>
		[DllImport(DllName)]
		public static extern bool PostThreadMessage(
			uint idThread,
			uint msg,
			UIntPtr wParam,
			IntPtr lParam);

		[StructLayout(LayoutKind.Sequential)]
		public struct Rect {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public int Width => Right - Left;

			public int Height => Bottom - Top;

			/// <inheritdoc />
			public override string ToString()
				=> $"({Left}, {Top}, {Right}, {Bottom}) W={Width}, H={Height}";
		}

		[DllImport(DllName)]
		public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
	}
}