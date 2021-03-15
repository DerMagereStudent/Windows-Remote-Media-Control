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

		[Flags]
		public enum AudioSessionState {
			Inactive = 0,
			Active = 1,
			Expired = 2
		}

		[Flags]
		public enum AudioSessionDisconnectReason {
			DisconnectReasonDeviceRemoval = 0,
			DisconnectReasonServerShutdown = 1,
			DisconnectReasonFormatChanged = 2,
			DisconnectReasonSessionLogoff = 3,
			DisconnectReasonSessionDisconnected = 4,
			DisconnectReasonExclusiveModeOverride = 5
		}

		[Flags]
		public enum ActivateOptions {
			None = 0x00000000,
			DesignMode = 0x00000001,
			NoErrorUI = 0x00000002,                            
			NoSplashScreen = 0x00000004,
		}

		[Flags]
		public enum SIGDN : int {
			SIGDN_NORMALDISPLAY					= unchecked((int)0x00000000),
			SIGDN_PARENTRELATIVEPARSING			= unchecked((int)0x80018001),
			SIGDN_DESKTOPABSOLUTEPARSING		= unchecked((int)0x80028000),
			SIGDN_PARENTRELATIVEEDITING			= unchecked((int)0x80031001),
			SIGDN_DESKTOPABSOLUTEEDITING		= unchecked((int)0x8004C000),
			SIGDN_FILESYSPATH					= unchecked((int)0x80058000),
			SIGDN_URL							= unchecked((int)0x80068000),
			SIGDN_PARENTRELATIVEFORADDRESSBAR	= unchecked((int)0x8007C001),
			SIGDN_PARENTRELATIVE				= unchecked((int)0x80080001),
			SIGDN_PARENTRELATIVEFORUI			= unchecked((int)0x80094001)
		}

		[Flags]
		public enum SICHINTF : int {
			SICHINT_DISPLAY							= unchecked((int)0x00000000),
			SICHINT_ALLFIELDS						= unchecked((int)0x80000000),
			SICHINT_CANONICAL						= unchecked((int)0x10000000),
			SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL	= unchecked((int)0x20000000)
		}

		public enum SFGAOF : int {
			SFGAO_CANCOPY			= unchecked((int)0x00000001),
			SFGAO_CANMOVE			= unchecked((int)0x00000002),
			SFGAO_CANLINK			= unchecked((int)0x00000004),
			SFGAO_STORAGE			= unchecked((int)0x00000008),
			SFGAO_CANRENAME			= unchecked((int)0x00000010),
			SFGAO_CANDELETE			= unchecked((int)0x00000020),
			SFGAO_HASPROPSHEET		= unchecked((int)0x00000040),
			SFGAO_DROPTARGET		= unchecked((int)0x00000100),
			SFGAO_CAPABILITYMASK	= unchecked((int)0x00000177),
			SFGAO_SYSTEM			= unchecked((int)0x00001000),
			SFGAO_ENCRYPTED			= unchecked((int)0x00002000),
			SFGAO_ISSLOW			= unchecked((int)0x00004000),
			SFGAO_GHOSTED			= unchecked((int)0x00008000),
			SFGAO_LINK				= unchecked((int)0x00010000),
			SFGAO_SHARE				= unchecked((int)0x00020000),
			SFGAO_READONLY			= unchecked((int)0x00040000),
			SFGAO_HIDDEN			= unchecked((int)0x00080000),
			SFGAO_DISPLAYATTRMASK	= unchecked((int)0x000FC000),
			SFGAO_NONENUMERATED		= unchecked((int)0x00100000),
			SFGAO_NEWCONTENT		= unchecked((int)0x00200000),
			SFGAO_CANMONIKER		= unchecked((int)0x00000000),
			SFGAO_HASSTORAGE		= unchecked((int)0x00000000),
			SFGAO_STREAM			= unchecked((int)0x00400000),
			SFGAO_STORAGEANCESTOR	= unchecked((int)0x00800000),
			SFGAO_VALIDATE			= unchecked((int)0x01000000),
			SFGAO_REMOVABLE			= unchecked((int)0x02000000),
			SFGAO_COMPRESSED		= unchecked((int)0x04000000),
			SFGAO_BROWSABLE			= unchecked((int)0x08000000),
			SFGAO_FILESYSANCESTOR	= unchecked((int)0x10000000),
			SFGAO_FOLDER			= unchecked((int)0x20000000),
			SFGAO_FILESYSTEM		= unchecked((int)0x40000000),
			SFGAO_STORAGECAPMASK	= unchecked((int)0x70C50008),
			SFGAO_HASSUBFOLDER		= unchecked((int)0x80000000),
			SFGAO_CONTENTSMASK		= unchecked((int)0x80000000),
			SFGAO_PKEYSFGAOMASK		= unchecked((int)0x81044000)
		}

		[Flags]
		public enum SHGFI : uint {
			Icon = 0x00000100,
			DisplayName = 0x00000200,
			TypeName = 0x00000400,
			Attributes = 0x00000800,
			IconLocation = 0x00001000,
			ExeType = 0x00002000,
			SysIconIndex = 0x00004000,
			LinkOverlay = 0x00008000,
			Selected = 0x00010000,
			Attr_Specified = 0x00020000,
			LargeIcon = 0x00000000,
			SmallIcon = 0x00000001,
			OpenIcon = 0x00000002,
			ShellIconSize = 0x00000004,
			PIDL = 0x00000008,
			UseFileAttributes = 0x00000010,
			AddOverlays = 0x00000020,
			OverlayIndex = 0x00000040,
		}
	}
}