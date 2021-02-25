using System;

namespace WRMC.Windows.Native {
	public static class NativeDefinitions {
		public static NativeStructs.PROPERTYKEY PKEY_AppUserModel_ID = new NativeStructs.PROPERTYKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5);

		public const int PROCESS_TERMINATE = 0x0001;
		public const int PROCESS_CREATE_THREAD = 0x0002;
		public const int PROCESS_SET_SESSIONID = 0x0004;
		public const int PROCESS_VM_OPERATION = 0x0008;
		public const int PROCESS_VM_READ = 0x0010;
		public const int PROCESS_VM_WRITE = 0x0020;
		public const int PROCESS_DUP_HANDLE = 0x0040;
		public const int PROCESS_CREATE_PROCESS = 0x0080;
		public const int PROCESS_SET_QUOTA = 0x0100;
		public const int PROCESS_SET_INFORMATION = 0x0200;
		public const int PROCESS_QUERY_INFORMATION = 0x0400;
		public const int PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
		public const int STANDARD_RIGHTS_REQUIRED = 0x000F0000;
		public const int SYNCHRONIZE = 0x00100000;
		public const int PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF;

		public const int SPI_GETWORKAREA = 0x0030;

		public const int SW_HIDE = 0;
		public const int SW_SHOWNORMAL = 1;
		public const int SW_NORMAL = 1;
		public const int SW_SHOWMINIMIZED = 2;
		public const int SW_SHOWMAXIMIZED = 3;
		public const int SW_MAXIMIZE = 3;
		public const int SW_SHOWNOACTIVATE = 4;
		public const int SW_SHOW = 5;
		public const int SW_MINIMIZE = 6;
		public const int SW_SHOWMINNOACTIVE = 7;
		public const int SW_SHOWNA = 8;
		public const int SW_RESTORE = 9;
		public const int SW_SHOWDEFAULT = 10;
		public const int SW_FORCEMINIMIZE = 11;
		public const int SW_MAX = 11;

		public const int SWP_NOSIZE = 0x0001;
		public const int SWP_NOMOVE = 0x0002;
		public const int SWP_NOZORDER = 0x0004;
		public const int SWP_NOREDRAW = 0x0008;
		public const int SWP_NOACTIVATE = 0x0010;
		public const int SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
		public const int SWP_SHOWWINDOW = 0x0040;
		public const int SWP_HIDEWINDOW = 0x0080;
		public const int SWP_NOCOPYBITS = 0x0100;
		public const int SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
		public const int SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */
		public const int SWP_DRAWFRAME = SWP_FRAMECHANGED;
		public const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;
		public const int SWP_DEFERERASE = 0x2000;
		public const int SWP_ASYNCWINDOWPOS = 0x4000;

		public const int GWL_EXSTYLE = -20;
		public const int GWL_HINSTANCE = -6;
		public const int GWL_ID = -12;
		public const int GWL_STYLE = -16;
		public const int GWL_USERDATA = -21;
		public const int GWL_WNDPROC = -4;

		public const long WS_BORDER = 0x00800000L;
		public const long WS_CAPTION = 0x00C00000L;
		public const long WS_CHILD = 0x40000000L;
		public const long WS_CHILDWINDOW = 0x40000000L;
		public const long WS_CLIPCHILDREN = 0x02000000L;
		public const long WS_CLIPSIBLINGS = 0x04000000L;
		public const long WS_DISABLED = 0x08000000L;
		public const long WS_DLGFRAME = 0x00400000L;
		public const long WS_GROUP = 0x00020000L;
		public const long WS_HSCROLL = 0x00100000L;
		public const long WS_ICONIC = 0x20000000L;
		public const long WS_MAXIMIZE = 0x01000000L;
		public const long WS_MAXIMIZEBOX = 0x00010000L;
		public const long WS_MINIMIZE = 0x20000000L;
		public const long WS_MINIMIZEBOX = 0x00020000L;
		public const long WS_OVERLAPPED = 0x00000000L;
		public const long WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
		public const long WS_POPUP = 0x80000000L;
		public const long WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU);
		public const long WS_SIZEBOX = 0x00040000L;
		public const long WS_SYSMENU = 0x00080000L;
		public const long WS_TABSTOP = 0x00010000L;
		public const long WS_THICKFRAME = 0x00040000L;
		public const long WS_TILED = 0x00000000L;
		public const long WS_TILEDWINDOW = 0x10000000L;
		public const long WS_VSCROLL = 0x00200000L;
	}
}