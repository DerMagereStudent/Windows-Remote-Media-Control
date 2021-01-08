using System;
using System.Runtime.InteropServices;

namespace WRMC.Windows.Native {
	public static class NativeStructs {
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
	}
}