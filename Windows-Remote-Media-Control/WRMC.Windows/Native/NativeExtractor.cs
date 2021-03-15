using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WRMC.Windows.Native {
	public static class NativeExtractor {
		public static List<IntPtr> GetHwndsForAUMID(string aumid) {
			List<IntPtr> hwnds = new List<IntPtr>();

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
					hwnds.Add(hWndChild);

				hWndChild = NativeMethods.FindWindowEx(IntPtr.Zero, hWndChild, null, null);
			}

			return hwnds;
		}

		public static List<int> GetProcessIDsForAUMID(string aumid) {
			List<int> ids = new List<int>();

			foreach (Process p in Process.GetProcesses()) {
				try {
					string sAUMID = NativeExtractor.GetAUMIDForPid(p.Id);

					if (sAUMID != null && sAUMID.Equals(aumid) || p.MainModule.FileName.Contains(aumid))
						ids.Add(p.Id);
				}
				catch (Win32Exception) { }
				catch (InvalidOperationException) { }
			}

			return ids;
		}

		public static string GetAUMIDForPid(int pid) {
			IntPtr hProcess = NativeMethods.OpenProcess(NativeDefinitions.PROCESS_QUERY_LIMITED_INFORMATION, false, pid);

			uint sbc = 256;
			StringBuilder sbApplicationUserModelId = new StringBuilder((int)sbc);
			NativeMethods.GetApplicationUserModelId(hProcess, ref sbc, sbApplicationUserModelId);

			string s = sbApplicationUserModelId.ToString();
			NativeMethods.CloseHandle(hProcess);

			return s;
		}

		public static byte[] GetDirectoryItemIconBytes(string path) {
			uint rgfInOut = 0;
			if (NativeMethods.SHILCreateFromPath(path, out IntPtr ppIdl, ref rgfInOut) != NativeEnums.HRESULT.S_OK)
				return new byte[0];

			NativeStructs.SHFILEINFO sfi = new NativeStructs.SHFILEINFO();
			IntPtr hil = NativeMethods.SHGetFileInfo(ppIdl, 0, ref sfi, (uint)Marshal.SizeOf(sfi), NativeDefinitions.SHGFI_PIDL | NativeDefinitions.SHGFI_SYSICONINDEX);

			if (hil == IntPtr.Zero)
				return new byte[0];

			IntPtr hIcon = NativeMethods.ImageList_GetIcon(hil, sfi.iIcon.ToInt32(), NativeDefinitions.ILD_TRANSPARENT);

			try {
				Icon icon = Icon.FromHandle(hIcon);
				Bitmap iconBM = new Bitmap(icon.Width, icon.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				using (Graphics g = Graphics.FromImage(iconBM))
					g.DrawIcon(icon, 0, 0);

				byte[] bytes = new ImageConverter().ConvertTo(iconBM, typeof(byte[])) as byte[];
				NativeMethods.DestroyIcon(hIcon);
				return bytes;
			} catch (Exception) {
				return new byte[0];
			}
		}
	}
}