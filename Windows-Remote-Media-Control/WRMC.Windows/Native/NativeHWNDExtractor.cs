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

		public static List<IntPtr> GetHwndsForPids(List<int> pids) {
			List<IntPtr> hwnds = new List<IntPtr>();
			NativeMethods.EnumWindows((hwnd, lParam) => CheckWindow(hwnd, lParam, hwnds, pids), IntPtr.Zero);
			return hwnds;
		}

		public static IntPtr GetHwndsForUWPApp(string aumid) {
			NativeHWNDExtractor.aumid = aumid;
			ENUMWINDOWPARAM ep = new ENUMWINDOWPARAM();
			IntPtr eplParam = Marshal.AllocHGlobal(Marshal.SizeOf(ep));
			Marshal.StructureToPtr(ep, eplParam, false);

			NativeMethods.EnumWindows(new NativeMethods.WNDENUMPROCR(CheckWindowUWP), ref eplParam);

			object obj = Marshal.PtrToStructure(eplParam, typeof(ENUMWINDOWPARAM));
			ENUMWINDOWPARAM newep = (ENUMWINDOWPARAM)obj;
			Marshal.FreeHGlobal(eplParam);

			return new IntPtr(newep.ihWnd);
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

		private static bool CheckWindowUWP(IntPtr hwnd, ref IntPtr lParam) {
			ENUMWINDOWPARAM ep = (ENUMWINDOWPARAM)Marshal.PtrToStructure(lParam, typeof(ENUMWINDOWPARAM));
			
			StringBuilder sbClassName = new StringBuilder(256);
			NativeMethods.GetClassName(hwnd, sbClassName, sbClassName.Capacity);
			string className = sbClassName.ToString();

			if (className.Equals("ApplicationFrameWindow")) {
				// get real hWnd
				NativeInterfaces.IPropertyStore pPropertyStore;
				Guid guid = new Guid("{886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99}");
				NativeEnums.HRESULT hr = NativeMethods.SHGetPropertyStoreForWindow(hwnd, ref guid, out pPropertyStore);
				if (hr == NativeEnums.HRESULT.S_OK) {
					NativeStructs.PROPVARIANT propVar = new NativeStructs.PROPVARIANT();
					hr = pPropertyStore.GetValue(ref NativeDefinitions.PKEY_AppUserModel_ID, out propVar);
					string sAUMID = Marshal.PtrToStringUni(propVar.pwszVal);
					if (sAUMID != null && sAUMID.Equals(aumid)) {
						ep.ihWnd = hwnd.ToInt32();
						Marshal.StructureToPtr(ep, lParam, false);
						Marshal.ReleaseComObject(pPropertyStore);
						return false;
					}
					Marshal.ReleaseComObject(pPropertyStore);
				}
			}

			return true;
		}

		private static bool IsHwndOfProcesses(IntPtr hwnd, List<int> pids) {
			int pid = 0;
			uint threadId = NativeMethods.GetWindowThreadProcessId(hwnd, out pid);

			return pids.Contains(pid);
		}
	}
}