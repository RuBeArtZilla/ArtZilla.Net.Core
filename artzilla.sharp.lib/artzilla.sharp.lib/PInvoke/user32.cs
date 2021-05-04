using System;
using System.Runtime.InteropServices;

namespace ArtZilla.Net.Core.PInvoke {
	/// <summary>
	/// Second argument for function <see cref="User32.ShowWindow(IntPtr,ShowWindowCommand)"/>
	/// and <see cref="User32.ShowWindowAsync(IntPtr,ShowWindowCommand)"/>
	/// </summary>
	public enum ShowWindowCommand {
		/// <summary>
		/// Minimizes a window, even if the thread that owns the window is not responding.
		/// This flag should only be used when minimizing windows from a different thread.
		/// </summary>
		ForceMinimize = 11,

		/// <summary>
		/// Hides the window and activates another window.
		/// </summary>
		Hide = 0,

		/// <summary>
		/// Maximizes the specified window.
		/// </summary>
		Maximize = 3,

		/// <summary>
		/// Minimizes the specified window and activates the next top-level window in the Z order.
		/// </summary>
		Minimize = 2,

		/// <summary>
		/// Activates and displays the window. If the window is minimized or maximized,
		/// the system restores it to its original size and position.
		/// An application should specify this flag when restoring a minimized window.
		/// </summary>
		Restore = 9,

		/// <summary>
		/// Activates the window and displays it in its current size and position.
		/// </summary>
		Show = 5,

		/// <summary>
		/// Sets the show state based on the SW_ value specified in the STARTUPINFO structure
		/// passed to the CreateProcess function by the program that started the application.
		/// </summary>
		ShowDefault = 10,

		/// <summary>
		/// Activates the window and displays it as a maximized window.
		/// </summary>
		ShowMaximized = 3,

		/// <summary>
		/// Activates the window and displays it as a minimized window.
		/// </summary>
		ShowMinimized = 2,

		/// <summary>
		/// Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED,
		/// except the window is not activated.
		/// </summary>
		ShowMinNoActive = 7,

		/// <summary>
		/// Displays the window in its current size and position. This value is similar to SW_SHOW,
		/// except that the window is not activated.
		/// </summary>
		ShowNa = 8,

		/// <summary>
		/// Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL,
		/// except that the window is not activated.
		/// </summary>
		ShowNoActivate = 4,

		/// <summary>
		/// Activates and displays a window. If the window is minimized or maximized,
		/// the system restores it to its original size and position.
		/// An application should specify this flag when displaying the window for the first time.
		/// </summary>
		ShowNormal = 1,
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

	/// <summary>
	/// USER32.DLL implements the Windows USER component that creates and manipulates the standard elements
	/// of the Windows user interface, such as the desktop, windows, and menus. It thus enables programs
	/// to implement a graphical user interface (GUI) that matches the Windows look and feel.
	/// Programs call functions from Windows USER to perform operations such as creating and managing windows,
	/// receiving window messages (which are mostly user input such as mouse and keyboard events,
	/// but also notifications from the operating system), displaying text in a window, and displaying message boxes.
	/// Many of the functions in USER32.DLL call upon GDI functions exported by GDI32.DLL to do the actual rendering
	/// of the various elements of the user interface. Some types of programs will also call GDI functions directly
	/// to perform lower-level drawing operations within a window previously created via USER32 functions.
	/// <para>https://en.wikipedia.org/wiki/Microsoft_Windows_library_files#USER32.DLL"</para>
	/// </summary>
	public static class User32 {
		/// <summary> The name of the DLL that contains the unmanaged method. </summary>
		public const string DllName = "user32.dll";

		[DllImport(DllName)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		/// <summary>
		/// Sets the specified window's show state.
		/// </summary>
		/// <param name="hWnd">A handle to the window.</param>
		/// <param name="nCmdShow">Controls how the window is to be shown.
		/// This parameter is ignored the first time an application calls ShowWindow,
		/// if the program that launched the application provides a STARTUPINFO structure.
		/// Otherwise, the first time ShowWindow is called, the value should be the value obtained by the WinMain
		/// function in its nCmdShow parameter. In subsequent calls, this parameter can be one of the following values.
		/// </param>
		[DllImport(DllName)]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		/// <summary>
		/// Sets the specified window's show state.
		/// </summary>
		/// <param name="hWnd">A handle to the window.</param>
		/// <param name="nCmdShow">Controls how the window is to be shown.
		/// This parameter is ignored the first time an application calls ShowWindow,
		/// if the program that launched the application provides a STARTUPINFO structure.
		/// Otherwise, the first time ShowWindow is called, the value should be the value obtained by the WinMain
		/// function in its nCmdShow parameter. In subsequent calls, this parameter can be one of the following values.
		/// </param>
		public static void ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow)
			=> ShowWindow(hWnd, (int) nCmdShow);

		/// <summary>
		/// Sets the show state of a window without waiting for the operation to complete.
		/// </summary>
		/// <param name="hWnd">A handle to the window.</param>
		/// <param name="nCmdShow">Controls how the window is to be shown.
		/// This parameter is ignored the first time an application calls ShowWindow,
		/// if the program that launched the application provides a STARTUPINFO structure.
		/// Otherwise, the first time ShowWindow is called, the value should be the value obtained by the WinMain
		/// function in its nCmdShow parameter. In subsequent calls, this parameter can be one of the following values.
		/// </param>
		[DllImport(DllName)]
		public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

		/// <summary>
		/// Sets the show state of a window without waiting for the operation to complete.
		/// </summary>
		/// <param name="hWnd">A handle to the window.</param>
		/// <param name="nCmdShow">Controls how the window is to be shown.
		/// This parameter is ignored the first time an application calls ShowWindow,
		/// if the program that launched the application provides a STARTUPINFO structure.
		/// Otherwise, the first time ShowWindow is called, the value should be the value obtained by the WinMain
		/// function in its nCmdShow parameter. In subsequent calls, this parameter can be one of the following values.
		/// </param>
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