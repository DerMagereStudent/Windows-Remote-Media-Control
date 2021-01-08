using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WRMC.Windows {
	public static class ProcessExtensions {
		private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern bool IsWindowVisible(IntPtr hwnd);

		public static IEnumerable<IntPtr> GetWindowHandles(this Process process) {
			var handles = new List<IntPtr>();

			foreach (ProcessThread thread in Process.GetProcessById(process.Id).Threads)
				EnumThreadWindows(thread.Id, (hWnd, lParam) => {
					if (!handles.Contains(hWnd) && IsWindowVisible(hWnd))
						handles.Add(hWnd);

					return true;
				}, IntPtr.Zero);

			return handles;
		}
	}
}