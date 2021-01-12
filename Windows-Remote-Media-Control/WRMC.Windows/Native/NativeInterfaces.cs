using System;
using System.Runtime.InteropServices;

namespace WRMC.Windows.Native {
	public static class NativeInterfaces {
		[ComImport, Guid(NativeGuids.I_PROPERTY_STORE), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPropertyStore {
			[PreserveSig]
			NativeEnums.HRESULT GetCount(
				[Out, MarshalAs(UnmanagedType.U4)] out uint propertyCount
			);

			[PreserveSig]
			NativeEnums.HRESULT GetAt(
				[In, MarshalAs(UnmanagedType.U4)] uint propertyIndex,
				[Out, MarshalAs(UnmanagedType.Struct)] out NativeStructs.PROPERTYKEY key
			);

			[PreserveSig]
			NativeEnums.HRESULT GetValue(
				[In, MarshalAs(UnmanagedType.Struct)] ref NativeStructs.PROPERTYKEY key,
				[Out, MarshalAs(UnmanagedType.Struct)] out NativeStructs.PROPVARIANT pv
			);

			[PreserveSig]
			NativeEnums.HRESULT SetValue(
				[In, MarshalAs(UnmanagedType.Struct)] ref NativeStructs.PROPERTYKEY key,
				[In, MarshalAs(UnmanagedType.Struct)] ref NativeStructs.PROPVARIANT pv
			);

			[PreserveSig]
			NativeEnums.HRESULT Commit();
		}

		[ComImport]
		[Guid(NativeGuids.I_MM_DEVICE)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMMDevice {
			[PreserveSig]
			int Activate(
				[In] ref Guid interfaceId,
				[In, MarshalAs(UnmanagedType.U4)] uint classContext,
				[In, Optional] IntPtr activationParams,
				[Out, MarshalAs(UnmanagedType.IUnknown)] out object instancePtr
			);

			[PreserveSig]
			int OpenPropertyStore(
				[In, MarshalAs(UnmanagedType.U4)] uint stgmAccess,
				[Out, MarshalAs(UnmanagedType.Interface)] out IPropertyStore ppProperties
			);

			[PreserveSig]
			int GetId(
				[Out, MarshalAs(UnmanagedType.LPWStr)] out string strId
			);

			[PreserveSig]
			int GetState(
				[Out, MarshalAs(UnmanagedType.U4)] out uint deviceState
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_MM_DEVICE_ENUMERATOR)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMMDeviceEnumerator {
			[PreserveSig]
			int EnumAudioEndpoints(
				[In, MarshalAs(UnmanagedType.I4)] NativeEnums.EDataFlow dataFlow,
				[In, MarshalAs(UnmanagedType.U4)] uint stateMask,
				[Out, MarshalAs(UnmanagedType.Interface)] out IMMDeviceCollection devices);

			[PreserveSig]
			int GetDefaultAudioEndpoint(
				[In, MarshalAs(UnmanagedType.I4)] NativeEnums.EDataFlow dataFlow,
				[In, MarshalAs(UnmanagedType.I4)] NativeEnums.ERole role,
				[Out, MarshalAs(UnmanagedType.Interface)] out IMMDevice device);

			[PreserveSig]
			int GetDevice(
				[In, MarshalAs(UnmanagedType.LPWStr)] string endpointId,
				[Out, MarshalAs(UnmanagedType.Interface)] out IMMDevice device);

			[PreserveSig]
			int RegisterEndpointNotificationCallback(
				[In, MarshalAs(UnmanagedType.Interface)] IMMNotificationClient client
			);

			[PreserveSig]
			int UnregisterEndpointNotificationCallback(
				[In, MarshalAs(UnmanagedType.Interface)] IMMNotificationClient client
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_MM_DEVICE_COLLECTION)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMMDeviceCollection {
			[PreserveSig]
			int GetCount(
				[Out, MarshalAs(UnmanagedType.U4)] out uint count
			);

			[PreserveSig]
			int Item(
				[In, MarshalAs(UnmanagedType.U4)] uint index,
				[Out, MarshalAs(UnmanagedType.Interface)] out IMMDevice device
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_MM_NOTIFICATION_CLIENT)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMMNotificationClient {
			void OnDeviceStateChanged(
				[MarshalAs(UnmanagedType.LPWStr)] string deviceId,
				[MarshalAs(UnmanagedType.U4)] uint newState
			);

			void OnDeviceAdded(
				[MarshalAs(UnmanagedType.LPWStr)] string deviceId
			);

			void OnDeviceRemoved(
				[MarshalAs(UnmanagedType.LPWStr)] string deviceId
			);

			void OnDefaultDeviceChanged(
				[MarshalAs(UnmanagedType.I4)] NativeEnums.EDataFlow dataFlow,
				[MarshalAs(UnmanagedType.I4)] NativeEnums.ERole deviceRole,
				[MarshalAs(UnmanagedType.LPWStr)] string defaultDeviceId
			);

			void OnPropertyValueChanged(
				[MarshalAs(UnmanagedType.LPWStr)] string deviceId, NativeStructs.PROPERTYKEY propertyKey
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_POLICY_CONFIG)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPolicyConfig {
			[PreserveSig]
			int GetMixFormat(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In] IntPtr ppFormat
			);

			[PreserveSig]
			int GetDeviceFormat(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In, MarshalAs(UnmanagedType.Bool)] bool bDefault,
				[In] IntPtr ppFormat
			);

			[PreserveSig]
			int ResetDeviceFormat(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName
			);

			[PreserveSig]
			int SetDeviceFormat(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In] IntPtr pEndpointFormat,
				[In] IntPtr mixFormat
			);

			[PreserveSig]
			int GetProcessingPerid(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In, MarshalAs(UnmanagedType.Bool)] bool bDefault,
				[In] IntPtr pmftDefaultPerid,
				[In] IntPtr pmftMinimumPerid
			);

			[PreserveSig]
			int SetProcessingPerid(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In] IntPtr pmftPerid
			);

			[PreserveSig]
			int GetShareMode(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In] IntPtr pMode
			);

			[PreserveSig]
			int SetShareMode(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In] IntPtr mode
			);

			[PreserveSig]
			int GetPropertyValue(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In, MarshalAs(UnmanagedType.Bool)] bool bFxStore,
				[In] IntPtr key,
				[In] IntPtr pv
			);

			[PreserveSig]
			int SetPropertyValue(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In, MarshalAs(UnmanagedType.Bool)] bool bFxStore,
				[In] IntPtr key,
				[In] IntPtr pv
			);

			[PreserveSig]
			int SetDefaultEndpoint(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In, MarshalAs(UnmanagedType.U4)] NativeEnums.ERole role
			);

			[PreserveSig]
			int SetEndpointVisibility(
				[In, MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName,
				[In, MarshalAs(UnmanagedType.Bool)] bool bVisible
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_AUDIO_ENDPOINT_VOLUME)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioEndpointVolume {
			[PreserveSig]
			int RegisterControlChangeNotify(
				[In,MarshalAs(UnmanagedType.Interface)] IAudioEndpointVolumeCallback client
			);

			[PreserveSig]
			int UnregisterControlChangeNotify(
				[In, MarshalAs(UnmanagedType.Interface)] IAudioEndpointVolumeCallback client
			);

			[PreserveSig]
			int GetChannelCount(
				[Out, MarshalAs(UnmanagedType.U4)] out int channelCount
			);

			[PreserveSig]
			int SetMasterVolumeLevel(
				[In, MarshalAs(UnmanagedType.R4)] float level,
				[In, MarshalAs(UnmanagedType.LPStruct)] Guid eventContext
			);

			[PreserveSig]
			int SetMasterVolumeLevelScalar(
				[In, MarshalAs(UnmanagedType.R4)] float level,
				[In, MarshalAs(UnmanagedType.LPStruct)] Guid eventContext
			);

			[PreserveSig]
			int GetMasterVolumeLevel(
				[Out, MarshalAs(UnmanagedType.R4)] out float level
			);

			[PreserveSig]
			int GetMasterVolumeLevelScalar(
				[Out, MarshalAs(UnmanagedType.R4)] out float level
			);

			[PreserveSig]
			int SetChannelVolumeLevel(
				[In, MarshalAs(UnmanagedType.U4)] uint channelNumber,
				[In, MarshalAs(UnmanagedType.R4)] float level,
				[In, MarshalAs(UnmanagedType.LPStruct)] Guid eventContext			
			);

			[PreserveSig]
			int SetChannelVolumeLevelScalar(
				[In, MarshalAs(UnmanagedType.U4)] uint channelNumber,
				[In, MarshalAs(UnmanagedType.R4)] float level,
				[In, MarshalAs(UnmanagedType.LPStruct)] Guid eventContext			
			);

			[PreserveSig]
			int GetChannelVolumeLevel(
				[In, MarshalAs(UnmanagedType.U4)] uint channelNumber,
				[Out, MarshalAs(UnmanagedType.R4)] out float level			
			);

			[PreserveSig]
			int GetChannelVolumeLevelScalar(
				[In, MarshalAs(UnmanagedType.U4)] uint channelNumber,
				[Out, MarshalAs(UnmanagedType.R4)] out float level			
			);

			[PreserveSig]
			int SetMute(
				[In, MarshalAs(UnmanagedType.Bool)] bool isMuted,
				[In, MarshalAs(UnmanagedType.LPStruct)] Guid eventContext			
			);

			[PreserveSig]
			int GetMute(
				[Out, MarshalAs(UnmanagedType.Bool)] out bool isMuted			
			);

			[PreserveSig]
			int GetVolumeStepInfo(
				[Out, MarshalAs(UnmanagedType.U4)] out uint step,
				[Out, MarshalAs(UnmanagedType.U4)] out uint stepCount			
			);

			[PreserveSig]
			int VolumeStepUp(
				[In, MarshalAs(UnmanagedType.LPStruct)] Guid eventContext			
			);

			[PreserveSig]
			int VolumeStepDown(
				[In, MarshalAs(UnmanagedType.LPStruct)] Guid eventContext			
			);

			[PreserveSig]
			int QueryHardwareSupport(
				[Out, MarshalAs(UnmanagedType.U4)] out uint hardwareSupportMask			
			);

			[PreserveSig]
			int GetVolumeRange(
				[Out, MarshalAs(UnmanagedType.R4)] out float volumeMin,
				[Out, MarshalAs(UnmanagedType.R4)] out float volumeMax,
				[Out, MarshalAs(UnmanagedType.R4)] out float volumeStep			
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_AUDIO_ENDPOINT_VOLUME_CALLBACK)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioEndpointVolumeCallback {
			[PreserveSig]
			int OnNotify(
				[In] IntPtr notificationDat
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_AUDIO_SESSION_MANAGER)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioSessionManager {
			[PreserveSig]
			int GetAudioSessionControl(
				[MarshalAs(UnmanagedType.LPStruct)] Guid AudioSessionGuid,
				int StreamFlags,
				out IAudioSessionControl SessionControl
			);

			[PreserveSig]
			int GetSimpleAudioVolume(
				[MarshalAs(UnmanagedType.LPStruct)] Guid AudioSessionGuid,
				int StreamFlags,
				ISimpleAudioVolume AudioVolume
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_AUDIO_SESSION_MANAGER_2)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioSessionManager2 {
			[PreserveSig]
			int GetAudioSessionControl(
				[MarshalAs(UnmanagedType.LPStruct)] Guid AudioSessionGuid,
				int StreamFlags,
				out IAudioSessionControl SessionControl
			);

			[PreserveSig]
			int GetSimpleAudioVolume(
				[MarshalAs(UnmanagedType.LPStruct)] Guid AudioSessionGuid, 
				int StreamFlags,
				ISimpleAudioVolume AudioVolume
			);

			[PreserveSig]
			int GetSessionEnumerator(
				out IAudioSessionEnumerator SessionEnum
			);

			[PreserveSig]
			int RegisterSessionNotification(
				IAudioSessionNotification SessionNotification
			);

			[PreserveSig]
			int UnregisterSessionNotification(
				IAudioSessionNotification SessionNotification
			);

			int RegisterDuckNotificationNotImpl();
			int UnregisterDuckNotificationNotImpl();
		}

		[ComImport]
		[Guid(NativeGuids.I_AUDIO_SESSION_NOTIFICATION)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioSessionNotification {
			void OnSessionCreated(
				IAudioSessionControl NewSession
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_AUDIO_SESSION_ENUMERATOR)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioSessionEnumerator {
			[PreserveSig]
			int GetCount(
				out int SessionCount
			);

			[PreserveSig]
			int GetSession(
				int SessionCount,
				out IAudioSessionControl Session
			);

			[PreserveSig]
			int GetSession(
				int SessionCount,
				out IAudioSessionControl2 Session
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_AUDIO_SESSION_CONTROL)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioSessionControl {
			[PreserveSig]
			int GetState(
				out NativeEnums.AudioSessionState pRetVal
			);

			[PreserveSig]
			int GetDisplayName(
				[MarshalAs(UnmanagedType.LPWStr)] out string pRetVal
			);

			[PreserveSig]
			int SetDisplayName(
				[MarshalAs(UnmanagedType.LPWStr)] string Value,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			int GetIconPath(
				[MarshalAs(UnmanagedType.LPWStr)] out string pRetVal
			);

			[PreserveSig]
			int SetIconPath(
				[MarshalAs(UnmanagedType.LPWStr)] string Value,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			int GetGroupingParam(
				out Guid pRetVal
			);

			[PreserveSig]
			int SetGroupingParam(
				[MarshalAs(UnmanagedType.LPStruct)] Guid Override,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			int RegisterAudioSessionNotification(
				IAudioSessionEvents NewNotifications
			);

			[PreserveSig]
			int UnregisterAudioSessionNotification(
				IAudioSessionEvents NewNotifications
			);
		}

		[Guid(NativeGuids.I_AUDIO_SESSION_CONTROL_2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioSessionControl2 {
			[PreserveSig]
			int NotImpl0();

			[PreserveSig]
			int GetDisplayName(
				[MarshalAs(UnmanagedType.LPWStr)] out string pRetVal
			);

			[PreserveSig]
			int SetDisplayName(
				[MarshalAs(UnmanagedType.LPWStr)] string Value,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			int GetIconPath(
				[MarshalAs(UnmanagedType.LPWStr)] out string pRetVal
			);

			[PreserveSig]
			int SetIconPath(
				[MarshalAs(UnmanagedType.LPWStr)] string Value,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			int GetGroupingParam(
				out Guid pRetVal
			);

			[PreserveSig]
			int SetGroupingParam(
				[MarshalAs(UnmanagedType.LPStruct)] Guid Override,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			int NotImpl1();

			[PreserveSig]
			int NotImpl2();

			[PreserveSig]
			int GetSessionIdentifier(
				[MarshalAs(UnmanagedType.LPWStr)] out string pRetVal
			);

			[PreserveSig]
			int GetSessionInstanceIdentifier(
				[MarshalAs(UnmanagedType.LPWStr)] out string pRetVal
			);

			[PreserveSig]
			int GetProcessId(
				out int pRetVal
			);

			[PreserveSig]
			int IsSystemSoundsSession();

			[PreserveSig]
			int SetDuckingPreference(bool optOut);
		}

		[ComImport]
		[Guid(NativeGuids.I_SIMPLE_AUDIO_VOLUME)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface ISimpleAudioVolume {
			[PreserveSig]
			int SetMasterVolume(
				float fLevel,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			int GetMasterVolume(
				out float pfLevel
			);

			[PreserveSig]
			int SetMute(
				bool bMute,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			int GetMute(
				out bool pbMute
			);
		}

		[ComImport]
		[Guid(NativeGuids.I_AUDIO_SESSION_EVENTS)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioSessionEvents {
			[PreserveSig]
			void OnDisplayNameChanged(
				[MarshalAs(UnmanagedType.LPWStr)] string NewDisplayName,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			void OnIconPathChanged(
				[MarshalAs(UnmanagedType.LPWStr)] string NewIconPath,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			void OnSimpleVolumeChanged(
				float NewVolume,
				bool NewMute,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			void OnChannelVolumeChanged(
				int ChannelCount,
				IntPtr NewChannelVolumeArray,
				int ChangedChannel,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			void OnGroupingParamChanged(
				[MarshalAs(UnmanagedType.LPStruct)] Guid NewGroupingParam,
				[MarshalAs(UnmanagedType.LPStruct)] Guid EventContext
			);

			[PreserveSig]
			void OnStateChanged(
				NativeEnums.AudioSessionState NewState
			);

			[PreserveSig]
			void OnSessionDisconnected(
				NativeEnums.AudioSessionDisconnectReason DisconnectReason
			);
		}
	}
}