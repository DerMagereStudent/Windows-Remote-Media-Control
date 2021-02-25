using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WRMC.Windows.Native {
	public static class NativeHWNDExtractor {
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct ENUMWINDOWPARAM {
			public int ihWnd;
		}

		private static string aumid = "";
		private static List<IntPtr> currentUWPHwnds;

		public static List<IntPtr> GetHwndsForPids(List<int> pids) {
			List<IntPtr> hwnds = new List<IntPtr>();
			NativeMethods.EnumWindows((hwnd, lParam) => CheckWindow(hwnd, lParam, hwnds, pids), IntPtr.Zero);
			return hwnds;
		}

		public static List<IntPtr> GetHwndsForUWPApp(string aumid) {
			NativeHWNDExtractor.aumid = aumid;
			NativeHWNDExtractor.currentUWPHwnds = new List<IntPtr>();

			//NativeMethods.EnumWindows(new NativeMethods.WNDENUMPROC(CheckWindowUWP), IntPtr.Zero);

			IntPtr hWndChild = IntPtr.Zero;
			hWndChild = NativeMethods.FindWindowEx(IntPtr.Zero, hWndChild, null, null);
			while (hWndChild != IntPtr.Zero) {
				NativeEnums.HRESULT hr = NativeMethods.SHGetPropertyStoreForWindow(hWndChild, new Guid(NativeGuids.I_PROPERTY_STORE), out NativeInterfaces.IPropertyStore pPropertyStore);
				string sAUMID = null;
				if (hr == NativeEnums.HRESULT.S_OK) {
					hr = pPropertyStore.GetValue(ref NativeDefinitions.PKEY_AppUserModel_ID, out NativeStructs.PROPVARIANT propVar);
					sAUMID = Marshal.PtrToStringUni(propVar.pwszVal);
					Marshal.ReleaseComObject(pPropertyStore);
				}

				if (sAUMID != null && sAUMID.Equals(aumid))
					NativeHWNDExtractor.currentUWPHwnds.Add(hWndChild);

				hWndChild = NativeMethods.FindWindowEx(IntPtr.Zero, hWndChild, null, null);
			}

			return NativeHWNDExtractor.currentUWPHwnds;
		}

		public static IntPtr GetHwndsForUWPFSApp(string aumid) {
			IntPtr hwnd = NativeMethods.GetForegroundWindow();

			StringBuilder sbClassName = new StringBuilder(256);
			NativeMethods.GetClassName(hwnd, sbClassName, sbClassName.Capacity);
			string className = sbClassName.ToString();

			if (className.Equals("ApplicationFrameWindow"))
				return hwnd;

			return IntPtr.Zero;
		}

		private static bool CheckWindow(IntPtr hwnd, IntPtr lParam, List<IntPtr> hwds, List<int> pids) {
			if (IsHwndOfProcesses(hwnd, pids))
				hwds.Add(hwnd);

			return true;
		}

		private static bool CheckWindowUWP(IntPtr hwnd, IntPtr lParam) {
			StringBuilder sbClassName = new StringBuilder(256);
			NativeMethods.GetClassName(hwnd, sbClassName, sbClassName.Capacity);
			string className = sbClassName.ToString();

			//if (className.Equals("ApplicationFrameWindow")) {
				NativeEnums.HRESULT hr = NativeMethods.SHGetPropertyStoreForWindow(
					hwnd,
					new Guid(NativeGuids.I_PROPERTY_STORE),
					out NativeInterfaces.IPropertyStore  pPropertyStore
				);

				if (hr == NativeEnums.HRESULT.S_OK) {
					hr = pPropertyStore.GetValue(ref NativeDefinitions.PKEY_AppUserModel_ID, out NativeStructs.PROPVARIANT propVar);
					string sAUMID = Marshal.PtrToStringUni(propVar.pwszVal);

					if (sAUMID != null && sAUMID.Equals(aumid)) {
						NativeHWNDExtractor.currentUWPHwnds.Add(hwnd);
						return true;
					}
				}

				Marshal.ReleaseComObject(pPropertyStore);
			//}

			return true;
		}

		private static bool IsHwndOfProcesses(IntPtr hwnd, List<int> pids) {
			int pid = 0;
			uint threadId = NativeMethods.GetWindowThreadProcessId(hwnd, out pid);

			return pids.Contains(pid);
		}
	}
}