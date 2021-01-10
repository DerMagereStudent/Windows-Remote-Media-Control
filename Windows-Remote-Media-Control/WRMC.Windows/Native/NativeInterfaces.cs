using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WRMC.Windows.Native {
	public static class NativeInterfaces {
		[ComImport, Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPropertyStore {
			NativeEnums.HRESULT GetCount([Out] out uint propertyCount);
			NativeEnums.HRESULT GetAt([In] uint propertyIndex, [Out, MarshalAs(UnmanagedType.Struct)] out NativeStructs.PROPERTYKEY key);
			NativeEnums.HRESULT GetValue([In, MarshalAs(UnmanagedType.Struct)] ref NativeStructs.PROPERTYKEY key, [Out, MarshalAs(UnmanagedType.Struct)] out NativeStructs.PROPVARIANT pv);
			NativeEnums.HRESULT SetValue([In, MarshalAs(UnmanagedType.Struct)] ref NativeStructs.PROPERTYKEY key, [In, MarshalAs(UnmanagedType.Struct)] ref NativeStructs.PROPVARIANT pv);
			NativeEnums.HRESULT Commit();
		}
	}
}