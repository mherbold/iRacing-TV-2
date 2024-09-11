
using System;
using System.Runtime.InteropServices;

public static class WinApi
{
	public const int GWL_WNDPROC = -4;
	public const int GWL_HINSTANCE = -6;
	public const int GWL_HWNDPARENT = -8;
	public const int GWL_ID = -12;
	public const int GWL_STYLE = -16;
	public const int GWL_EXSTYLE = -20;
	public const int GWL_USERDATA = -21;

	public const uint WS_BORDER = 0x800000;
	public const uint WS_CAPTION = 0xc00000;
	public const uint WS_CHILD = 0x40000000;
	public const uint WS_CLIPCHILDREN = 0x2000000;
	public const uint WS_CLIPSIBLINGS = 0x4000000;
	public const uint WS_DISABLED = 0x8000000;
	public const uint WS_DLGFRAME = 0x400000;
	public const uint WS_GROUP = 0x20000;
	public const uint WS_HSCROLL = 0x100000;
	public const uint WS_MAXIMIZE = 0x1000000;
	public const uint WS_MAXIMIZEBOX = 0x10000;
	public const uint WS_MINIMIZE = 0x20000000;
	public const uint WS_MINIMIZEBOX = 0x20000;
	public const uint WS_OVERLAPPED = 0x0;
	public const uint WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
	public const uint WS_POPUP = 0x80000000;
	public const uint WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU;
	public const uint WS_SIZEFRAME = 0x40000;
	public const uint WS_SYSMENU = 0x80000;
	public const uint WS_TABSTOP = 0x10000;
	public const uint WS_VISIBLE = 0x10000000;
	public const uint WS_VSCROLL = 0x200000;

	public const uint WS_EX_ACCEPTFILES = 0x00000010;
	public const uint WS_EX_APPWINDOW = 0x00040000;
	public const uint WS_EX_CLIENTEDGE = 0x00000200;
	public const uint WS_EX_COMPOSITED = 0x02000000;
	public const uint WS_EX_CONTEXTHELP = 0x00000400;
	public const uint WS_EX_CONTROLPARENT = 0x00010000;
	public const uint WS_EX_DLGMODALFRAME = 0x00000001;
	public const uint WS_EX_LAYERED = 0x00080000;
	public const uint WS_EX_LAYOUTRTL = 0x00400000;
	public const uint WS_EX_LEFT = 0x00000000;
	public const uint WS_EX_LEFTSCROLLBAR = 0x00004000;
	public const uint WS_EX_LTRREADING = 0x00000000;
	public const uint WS_EX_MDICHILD = 0x00000040;
	public const uint WS_EX_NOACTIVATE = 0x08000000;
	public const uint WS_EX_NOINHERITLAYOUT = 0x00100000;
	public const uint WS_EX_NOPARENTNOTIFY = 0x00000004;
	public const uint WS_EX_NOREDIRECTIONBITMAP = 0x00200000;
	public const uint WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE;
	public const uint WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST;
	public const uint WS_EX_RIGHT = 0x00001000;
	public const uint WS_EX_RIGHTSCROLLBAR = 0x00000000;
	public const uint WS_EX_RTLREADING = 0x00002000;
	public const uint WS_EX_STATICEDGE = 0x00020000;
	public const uint WS_EX_TOOLWINDOW = 0x00000080;
	public const uint WS_EX_TOPMOST = 0x00000008;
	public const uint WS_EX_TRANSPARENT = 0x00000020;
	public const uint WS_EX_WINDOWEDGE = 0x00000100;

	public static readonly IntPtr HWND_TOP = new( 0 );
	public static readonly IntPtr HWND_BOTTOM = new( 1 );
	public static readonly IntPtr HWND_TOPMOST = new( -1 );
	public static readonly IntPtr HWND_NOTOPMOST = new( -2 );

	public const int SWP_ASYNCWINDOWPOS = 0x4000;
	public const int SWP_DEFERERASE = 0x2000;
	public const int SWP_DRAWFRAME = 0x0020;
	public const int SWP_FRAMECHANGED = 0x0020;
	public const int SWP_HIDEWINDOW = 0x0080;
	public const int SWP_NOACTIVATE = 0x0010;
	public const int SWP_NOCOPYBITS = 0x0100;
	public const int SWP_NOMOVE = 0x0002;
	public const int SWP_NOOWNERZORDER = 0x0200;
	public const int SWP_NOREDRAW = 0x0008;
	public const int SWP_NOREPOSITION = 0x0200;
	public const int SWP_NOSENDCHANGING = 0x0400;
	public const int SWP_NOSIZE = 0x0001;
	public const int SWP_NOZORDER = 0x0004;
	public const int SWP_SHOWWINDOW = 0x0040;

	public const int KEY_PRESSED = 0x8000;

	[StructLayout( LayoutKind.Sequential )]
	public struct MARGINS
	{
		public int Left;
		public int Right;
		public int Top;
		public int Bottom;
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct POINT
	{
		public int X;
		public int Y;

		public POINT( int X, int Y )
		{
			this.X = X;
			this.Y = Y;
		}
	}

	public enum VirtualKeyStates : int
	{
		VK_BACK = 0x08,
		VK_TAB = 0x09,

		VK_CLEAR = 0x0C,
		VK_RETURN = 0x0D,

		VK_SHIFT = 0x10,
		VK_CONTROL = 0x11,
		VK_MENU = 0x12,
		VK_PAUSE = 0x13,
		VK_CAPITAL = 0x14,

		VK_ESCAPE = 0x1B,
		VK_SPACE = 0x20,
		VK_PRIOR = 0x21,
		VK_NEXT = 0x22,
		VK_END = 0x23,
		VK_HOME = 0x24,
		VK_LEFT = 0x25,
		VK_UP = 0x26,
		VK_RIGHT = 0x27,
		VK_DOWN = 0x28,
		VK_INSERT = 0x2D,
		VK_DELETE = 0x2E,

		VK_LWIN = 0x5B,
		VK_RWIN = 0x5C,

		VK_SLEEP = 0x5F,

		VK_F1 = 0x70,
		VK_F2 = 0x71,
		VK_F3 = 0x72,
		VK_F4 = 0x73,
		VK_F5 = 0x74,
		VK_F6 = 0x75,
		VK_F7 = 0x76,
		VK_F8 = 0x77,
		VK_F9 = 0x78,
		VK_F10 = 0x79,
		VK_F11 = 0x7A,
		VK_F12 = 0x7B,

		VK_NUMLOCK = 0x90,
		VK_SCROLL = 0x91,

		VK_LSHIFT = 0xA0,
		VK_RSHIFT = 0xA1,
		VK_LCONTROL = 0xA2,
		VK_RCONTROL = 0xA3,
		VK_LMENU = 0xA4,
		VK_RMENU = 0xA5,
	}

	[DllImport( "user32.dll", SetLastError = true )]
	public static extern IntPtr FindWindow( string lpClassName, string lpWindowName );

	[DllImport( "user32.dll" )]
	public static extern uint SetWindowLong( IntPtr hWnd, int nIndex, uint dwNewLong );

	[DllImport( "user32.dll", SetLastError = true )]
	public static extern bool SetWindowPos( IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags );

	[DllImport( "user32.dll", SetLastError = true )]
	public static extern bool GetWindowRect( IntPtr hwnd, out RECT lpRect );

	[DllImport( "user32.dll" )]
	public static extern bool GetClientRect( IntPtr hWnd, out RECT lpRect );

	[DllImport( "user32.dll" )]
	public static extern bool ClientToScreen( IntPtr hWnd, ref POINT lpPoint );

	[DllImport( "dwmapi.dll" )]
	public static extern int DwmExtendFrameIntoClientArea( IntPtr hwnd, ref MARGINS margins );

	[DllImport( "user32.dll" )]
	public static extern short GetKeyState( VirtualKeyStates nVirtKey );
}
