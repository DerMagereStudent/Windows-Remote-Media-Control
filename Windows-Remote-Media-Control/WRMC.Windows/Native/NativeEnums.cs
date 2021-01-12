using System;

namespace WRMC.Windows.Native {
	public class NativeEnums {
		public enum HRESULT : int {
			S_OK = 0,
			S_FALSE = 1,
			E_NOTIMPL = unchecked((int)0x80004001),
			E_NOINTERFACE = unchecked((int)0x80004002),
			E_POINTER = unchecked((int)0x80004003),
			E_ABORT = unchecked((int)0x80004004),
			E_FAIL = unchecked((int)0x80004005),
			E_UNEXPECTED = unchecked((int)0x8000FFFF),
			E_ACCESSDENIED = unchecked((int)0x80070005),
			E_HANDLE = unchecked((int)0x80070006),
			E_OUTOFMEMORY = unchecked((int)0x8007000E),
			E_INVALIDARG = unchecked((int)0x80070057)
		}

		[Flags]
		public enum EDataFlow {
			eRender,
			eCapture,
			eAll,
		}

		[Flags]
		public enum ERole : uint {
			eConsole,
			eMultimedia,
			eCommunications
		}

		public enum AudioSessionState {
			Inactive = 0,
			Active = 1,
			Expired = 2
		}

		public enum AudioSessionDisconnectReason {
			DisconnectReasonDeviceRemoval = 0,
			DisconnectReasonServerShutdown = 1,
			DisconnectReasonFormatChanged = 2,
			DisconnectReasonSessionLogoff = 3,
			DisconnectReasonSessionDisconnected = 4,
			DisconnectReasonExclusiveModeOverride = 5
		}
	}
}