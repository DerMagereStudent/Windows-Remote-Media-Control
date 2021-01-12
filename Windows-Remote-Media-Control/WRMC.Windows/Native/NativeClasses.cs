using System;
using System.Runtime.InteropServices;

namespace WRMC.Windows.Native {
	public static class NativeClasses {
		public static class ComObjectFactory {
			public static object CreateInstance(Guid guid) {
				return Activator.CreateInstance(Type.GetTypeFromCLSID(guid));
			}
		}
	}
}