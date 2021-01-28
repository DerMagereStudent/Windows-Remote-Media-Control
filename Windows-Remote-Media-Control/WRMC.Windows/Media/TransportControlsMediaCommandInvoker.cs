using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Windows.Media.Control;

using WRMC.Core.Models;
using WRMC.Windows.Native;

namespace WRMC.Windows.Media {
	public class TransportControlsMediaCommandInvoker : MediaCommandInvoker {
		private static readonly string[] SUPPORTED_FILES = new string[] {
			".WAV", ".MP3", ".OGG",
			".AVI", ".MP4", ".WMV", ".M4A"
		};

		private GlobalSystemMediaTransportControlsSessionManager manager;

		public TransportControlsMediaCommandInvoker() {
			this.manager = GlobalSystemMediaTransportControlsSessionManager.RequestAsync().GetAwaiter().GetResult();
		}

		public override void Pause(MediaSession session) {
			TransportControlsMediaSessionExtractor sessionExtractor = MediaSessionExtractor.Default.TryCastOrDefault<TransportControlsMediaSessionExtractor>();
			sessionExtractor?.GetSystemSession(session)?.TryPauseAsync();
		}

		public override void Play(MediaSession session) {
			TransportControlsMediaSessionExtractor sessionExtractor = MediaSessionExtractor.Default.TryCastOrDefault<TransportControlsMediaSessionExtractor>();
			sessionExtractor?.GetSystemSession(session)?.TryPlayAsync();
		}

		public override void SkipNext(MediaSession session) {
			TransportControlsMediaSessionExtractor sessionExtractor = MediaSessionExtractor.Default.TryCastOrDefault<TransportControlsMediaSessionExtractor>();
			sessionExtractor?.GetSystemSession(session)?.TrySkipNextAsync();
		}

		public override void SkipPrevious(MediaSession session) {
			TransportControlsMediaSessionExtractor sessionExtractor = MediaSessionExtractor.Default.TryCastOrDefault<TransportControlsMediaSessionExtractor>();
			sessionExtractor?.GetSystemSession(session)?.TrySkipPreviousAsync();
		}

		public override void MoveToScreen(MediaSession session, string screenName) {
			Screen screen = Screen.AllScreens.FirstOrDefault((s) => s.DeviceName.Equals(screenName));

			if (screen == null)
				return;

			List<Process> processes = new List<Process>();

			foreach (int pid in session.ProcessIDs) {
				try {
					Process p = Process.GetProcessById(pid);
					processes.Add(p);
				}
				catch (ArgumentException) { }
			}

			List<IntPtr> wndHandles = session.AppType == MediaSession.ApplicationType.UWP ?
				new List<IntPtr>(new[] { NativeHWNDExtractor.GetHwndsForUWPApp(session.AUMID) }) :
				NativeHWNDExtractor.GetHwndsForPids(session.ProcessIDs);

			foreach (IntPtr hwnd in wndHandles) {
				if (!NativeMethods.IsWindowVisible(hwnd))
					continue;

				NativeStructs.RECT rect = new NativeStructs.RECT();
				NativeMethods.GetWindowRect(hwnd, out rect);

				if (rect.Left == 0 && rect.Top == 0 && rect.Right == 0 && rect.Bottom == 0)
					continue;

				if (string.IsNullOrEmpty(NativeMethods.GetWindowTitle(hwnd)))
					continue;

				NativeMethods.ShowWindow(hwnd, NativeDefinitions.SW_NORMAL);				
				NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, screen.Bounds.X, screen.Bounds.Y, 0, 0, NativeDefinitions.SWP_NOSIZE | NativeDefinitions.SWP_NOZORDER);
			}
		}

		public override void SetAudioEndpoint(MediaSession session, AudioEndpoint endpoint) {
			NativeInterfaces.IPolicyConfig config = NativeClasses.ComObjectFactory.CreateInstance(new Guid(NativeGuids.POLICY_CONFIG_CLIENT)) as NativeInterfaces.IPolicyConfig;

			config.SetDefaultEndpoint(endpoint.ID, NativeEnums.ERole.eConsole | NativeEnums.ERole.eMultimedia);
		}

		public override void SetMasterVolume(int volume) {
			NativeInterfaces.IMMDeviceEnumerator enumerator = NativeClasses.ComObjectFactory.CreateInstance(new Guid(NativeGuids.MM_DEVICE_ENUMERATOR)) as NativeInterfaces.IMMDeviceEnumerator;

			enumerator.GetDefaultAudioEndpoint(
				NativeEnums.EDataFlow.eRender,
				NativeEnums.ERole.eConsole,
				out NativeInterfaces.IMMDevice device
			);

			device.Activate(new Guid(NativeGuids.I_AUDIO_ENDPOINT_VOLUME), 0x1, IntPtr.Zero, out object volumeObj);
			NativeInterfaces.IAudioEndpointVolume audioEndpointVolume = volumeObj as NativeInterfaces.IAudioEndpointVolume;
			audioEndpointVolume.SetMasterVolumeLevelScalar(volume / 100.0f, new Guid());
		}

		public override void SetVolume(MediaSession session, int volume) {
			NativeInterfaces.IMMDeviceEnumerator enumerator = NativeClasses.ComObjectFactory.CreateInstance(new Guid(NativeGuids.MM_DEVICE_ENUMERATOR)) as NativeInterfaces.IMMDeviceEnumerator;

			enumerator.EnumAudioEndpoints(
				NativeEnums.EDataFlow.eRender,
				0x00000001,
				out NativeInterfaces.IMMDeviceCollection deviceCollection
			);

			deviceCollection.GetCount(out uint collectionCount);

			for (uint i = 0; i < collectionCount; i++) {
				deviceCollection.Item(i, out NativeInterfaces.IMMDevice device);
				device.Activate(new Guid(NativeGuids.I_AUDIO_SESSION_MANAGER_2), 0, IntPtr.Zero, out object obj);
				NativeInterfaces.IAudioSessionManager2 audioSessionManager = obj as NativeInterfaces.IAudioSessionManager2;

				audioSessionManager.GetSessionEnumerator(out NativeInterfaces.IAudioSessionEnumerator sessionEnumerator);
				sessionEnumerator.GetCount(out int count);

				for (int j = 0; j < count; j++) {
					sessionEnumerator.GetSession(j, out NativeInterfaces.IAudioSessionControl control);
					NativeInterfaces.IAudioSessionControl2 control2 = control as NativeInterfaces.IAudioSessionControl2;
					control2.GetProcessId(out int pid);

					if (session.ProcessIDs.Contains(pid)) {
						NativeInterfaces.ISimpleAudioVolume audioVolume = control as NativeInterfaces.ISimpleAudioVolume;
						audioVolume.SetMasterVolume(volume / 100.0f, Guid.Empty);
					}
				}
			}
		}

		public override void SetConfiguration(MediaSession session, Configuration configuration) {
			this.MoveToScreen(session, configuration.Screen);
			this.SetAudioEndpoint(session, configuration.AudioEndpoint);
		}

		public override void ResumeSuspendedProcess(SuspendedProcess suspendedProcess) {
			Process p = null;

			try {
				p = Process.GetProcessById(suspendedProcess.ID);
			} catch(ArgumentException) {
				return;
			}
			
			foreach (ProcessThread t in p.Threads) {
				IntPtr threadHandle = NativeMethods.OpenThread(0x0002, false, (uint)t.Id);

				if (threadHandle == IntPtr.Zero)
					continue;

				NativeMethods.ResumeThread(threadHandle);
				NativeMethods.CloseHandle(threadHandle);
			}
		}

		public override void PlayFile(string filePath) {
			//NativeInterfaces.IApplicationActivationManager applicationActivationManager = NativeClasses.ComObjectFactory.CreateInstance(new Guid(NativeGuids.APPLICATION_ACTIVATION_MANAGER)) as NativeInterfaces.IApplicationActivationManager;

			//NativeMethods.SHCreateItemFromParsingName(filePath, IntPtr.Zero, new Guid(NativeGuids.I_SHELL_ITEM), out NativeInterfaces.IShellItem shellItem);

			ProcessStartInfo startInfo = new ProcessStartInfo() {
				FileName = filePath,
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Maximized
			};

			Process.Start(startInfo);
		}

		public override List<string> GetScreens() => Screen.AllScreens.Select(s => s.DeviceName).ToList();

		public override List<AudioEndpoint> GetAudioEndpoints() {
			List<AudioEndpoint> endpoints = new List<AudioEndpoint>();

			NativeInterfaces.IMMDeviceEnumerator enumerator = NativeClasses.ComObjectFactory.CreateInstance(new Guid(NativeGuids.MM_DEVICE_ENUMERATOR)) as NativeInterfaces.IMMDeviceEnumerator;
			enumerator.EnumAudioEndpoints(NativeEnums.EDataFlow.eRender, 0x00000001, out NativeInterfaces.IMMDeviceCollection collection);
			collection.GetCount(out uint count);

			enumerator.GetDefaultAudioEndpoint(NativeEnums.EDataFlow.eRender, NativeEnums.ERole.eConsole | NativeEnums.ERole.eMultimedia, out NativeInterfaces.IMMDevice defaultIMMDevice);
			AudioEndpoint defaultAudioEndpoint = this.IMMDeviceToAudioEndpoint(defaultIMMDevice);
			
			for (uint i = 0; i < count; i++) {
				collection.Item(i, out NativeInterfaces.IMMDevice device);
				AudioEndpoint endpoint = this.IMMDeviceToAudioEndpoint(device);

				if (endpoint.Equals(defaultAudioEndpoint))
					endpoints.Insert(0, endpoint);
				else
					endpoints.Add(endpoint); ;
			}

			return endpoints;
		}

		private AudioEndpoint IMMDeviceToAudioEndpoint(NativeInterfaces.IMMDevice device) {
			NativeStructs.PROPERTYKEY PKEY_Device_FriendlyName = new NativeStructs.PROPERTYKEY(
				new Guid(
					0xa45c254e,
					0xdf1c,
					0x4efd,
					0x0080,
					0x0020,
					0x0067,
					0x00d1,
					0x0046,
					0x00a8,
					0x0050,
					0x00e0)
				, 14);

			device.GetId(out string id);

			device.OpenPropertyStore(0, out NativeInterfaces.IPropertyStore properties);
			properties.GetValue(PKEY_Device_FriendlyName, out NativeStructs.PROPVARIANT prop);
			string name = Marshal.PtrToStringUni(prop.pwszVal);

			return new AudioEndpoint() {
				ID = id,
				Name = name
			};
		}

		public override int GetMasterVolume() {
			NativeInterfaces.IMMDeviceEnumerator enumerator = NativeClasses.ComObjectFactory.CreateInstance(new Guid(NativeGuids.MM_DEVICE_ENUMERATOR)) as NativeInterfaces.IMMDeviceEnumerator;

			enumerator.GetDefaultAudioEndpoint(
				NativeEnums.EDataFlow.eRender,
				NativeEnums.ERole.eConsole,
				out NativeInterfaces.IMMDevice device
			);

			device.Activate(new Guid(NativeGuids.I_AUDIO_ENDPOINT_VOLUME), 0x1, IntPtr.Zero, out object volumeObj);
			NativeInterfaces.IAudioEndpointVolume audioEndpointVolume = volumeObj as NativeInterfaces.IAudioEndpointVolume;
			audioEndpointVolume.GetMasterVolumeLevelScalar(out float scalar);
			return (int)(scalar * 100.0f);
		}

		public override int GetVolume(MediaSession session) {
			NativeInterfaces.IMMDeviceEnumerator enumerator = NativeClasses.ComObjectFactory.CreateInstance(new Guid(NativeGuids.MM_DEVICE_ENUMERATOR)) as NativeInterfaces.IMMDeviceEnumerator;

			enumerator.EnumAudioEndpoints(
				NativeEnums.EDataFlow.eRender,
				0x00000001,
				out NativeInterfaces.IMMDeviceCollection deviceCollection
			);

			deviceCollection.GetCount(out uint collectionCount);

			for (uint i = 0; i < collectionCount; i++) {
				deviceCollection.Item(i, out NativeInterfaces.IMMDevice device);
				device.Activate(new Guid(NativeGuids.I_AUDIO_SESSION_MANAGER_2), 0, IntPtr.Zero, out object obj);
				NativeInterfaces.IAudioSessionManager2 audioSessionManager = obj as NativeInterfaces.IAudioSessionManager2;

				audioSessionManager.GetSessionEnumerator(out NativeInterfaces.IAudioSessionEnumerator sessionEnumerator);
				sessionEnumerator.GetCount(out int count);

				for (int j = 0; j < count; j++) {
					sessionEnumerator.GetSession(j, out NativeInterfaces.IAudioSessionControl control);
					NativeInterfaces.IAudioSessionControl2 control2 = control as NativeInterfaces.IAudioSessionControl2;
					control2.GetProcessId(out int pid);

					if (session.ProcessIDs.Contains(pid)) {
						NativeInterfaces.ISimpleAudioVolume audioVolume = control as NativeInterfaces.ISimpleAudioVolume;
						audioVolume.GetMasterVolume(out float volume);
						return (int)(volume * 100.0f);
					}
				}
			}

			return -1;
		}

		public override List<SuspendedProcess> GetSuspendedProcesses() {
			List<SuspendedProcess> processes = new List<SuspendedProcess>();

			foreach (Process p in Process.GetProcesses()) {
				bool suspended = true;

				foreach (ProcessThread t in p.Threads) {
					if (!(t.ThreadState == ThreadState.Wait && t.WaitReason == ThreadWaitReason.Suspended)) {
						suspended = false;
						break;
					}
				}

				if (suspended)
					processes.Add(new SuspendedProcess() {
						ID = p.Id,
						Name = p.ProcessName
					});
			}

			return processes;
		}

		public override Tuple<List<string>, List<string>> GetDirectoryContent(string directory) {

			if (string.IsNullOrWhiteSpace(directory)) {
				DriveInfo[] drives = DriveInfo.GetDrives();
				return new Tuple<List<string>, List<string>>(drives.Select(d => d.Name).ToList(), new List<string>());
			}

			if (!Directory.Exists(directory))
				return new Tuple<List<string>, List<string>>(new List<string>(), new List<string>());

			List<string> folders = Directory.GetDirectories(directory).ToList();
			List<string> files = Directory.GetFiles(directory).Where(f => SUPPORTED_FILES.Contains(new FileInfo(f).Extension.ToUpper())).ToList();

			return new Tuple<List<string>, List<string>>(folders, files);
		}
	}
}