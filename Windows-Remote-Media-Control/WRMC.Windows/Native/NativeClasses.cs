using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WRMC.Windows.Native {
	public static class NativeClasses {
		public static class ComObjectFactory {
			public static object CreateInstance(Guid guid) {
				return Activator.CreateInstance(Type.GetTypeFromCLSID(guid));
			}
		}

		[ComImport, Guid(NativeGuids.APPLICATION_ACTIVATION_MANAGER)]
		class ApplicationActivationManager : NativeInterfaces.IApplicationActivationManager {
			[PreserveSig]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern NativeEnums.HRESULT ActivateApplication(
				[In, MarshalAs(UnmanagedType.LPWStr)] string appUserModelId,
				[In, MarshalAs(UnmanagedType.LPWStr)] string arguments,
				[In] NativeEnums.ActivateOptions options,
				[Out, MarshalAs(UnmanagedType.U4)] out uint processId
			);

			[PreserveSig]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern NativeEnums.HRESULT ActivateForFile(
				[In, MarshalAs(UnmanagedType.LPWStr)] string appUserModelId,
				[In] IntPtr itemArray,
				[In, MarshalAs(UnmanagedType.LPWStr)] string verb,
				[Out, MarshalAs(UnmanagedType.U4)] out uint processId
			);

			[PreserveSig]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern NativeEnums.HRESULT ActivateForProtocol(
				[In, MarshalAs(UnmanagedType.LPWStr)] string appUserModelId,
				[In] IntPtr itemArray,
				[Out, MarshalAs(UnmanagedType.U4)] out uint processId
			);
		}
	}
}