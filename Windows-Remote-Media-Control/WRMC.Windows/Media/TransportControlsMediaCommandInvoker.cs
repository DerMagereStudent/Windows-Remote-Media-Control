using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Windows.Media.Control;
using Windows.Storage.Streams;
using Windows.System;

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

			List<IntPtr> hwnds = NativeExtractor.GetHwndsForAUMID(session.AUMID);

			foreach (IntPtr hwnd in hwnds) {
				if (!NativeMethods.IsWindowVisible(hwnd))
					continue;

				// if the window has no area continue
				NativeMethods.GetWindowRect(hwnd, out NativeStructs.RECT rect);
				if (this.GetArea(rect) == 0)
					continue;

				string title = NativeMethods.GetWindowTitle(hwnd);

				if (string.IsNullOrEmpty(title))
					continue;

				this.ShowWindow(hwnd, screen);
			}
		}

		private void ShowWindow(IntPtr hwnd, Screen screen) {
			// if the window is minimized, restore its actual state the get the correct window-rect
			long style = NativeMethods.GetWindowLong(hwnd, NativeDefinitions.GWL_STYLE);
			bool minimized = (style & NativeDefinitions.WS_MINIMIZE) == NativeDefinitions.WS_MINIMIZE;
			if (minimized)
				NativeMethods.ShowWindow(hwnd, NativeDefinitions.SW_RESTORE);

			// Remove fullscreen mode if set
			style = NativeMethods.GetWindowLong(hwnd, NativeDefinitions.GWL_STYLE);
			bool fullscreen = (style & NativeDefinitions.WS_OVERLAPPEDWINDOW) != NativeDefinitions.WS_OVERLAPPEDWINDOW;
			if (fullscreen)
				NativeMethods.SetWindowLong(hwnd, NativeDefinitions.GWL_STYLE, style | NativeDefinitions.WS_OVERLAPPEDWINDOW);

			// Make window normal if maximized to move it
			style = NativeMethods.GetWindowLong(hwnd, NativeDefinitions.GWL_STYLE);
			bool maximized = (style & NativeDefinitions.WS_MAXIMIZE) == NativeDefinitions.WS_MAXIMIZE;
			if (maximized)
				NativeMethods.ShowWindow(hwnd, NativeDefinitions.SW_RESTORE);

			// set window position. Remain relative screen position.
			this.GetScreenPos(hwnd, out int x, out int y);
			NativeMethods.SetWindowPos(
				hwnd, IntPtr.Zero, screen.Bounds.X + x, screen.Bounds.Y + y, 0, 0,
				NativeDefinitions.SWP_NOZORDER | NativeDefinitions.SWP_NOOWNERZORDER | NativeDefinitions.SWP_NOSIZE | NativeDefinitions.SWP_FRAMECHANGED
			);

			// make window maximized again after move
			if (maximized)
				NativeMethods.ShowWindow(hwnd, NativeDefinitions.SW_MAXIMIZE);

			// Restore fullscreen mode if it was set.
			if (fullscreen)
				NativeMethods.SetWindowLong(hwnd, NativeDefinitions.GWL_STYLE, style & ~NativeDefinitions.WS_OVERLAPPEDWINDOW);

			NativeMethods.SetForegroundWindow(hwnd);
		}

		private int GetArea(NativeStructs.RECT rect) => (rect.Right - rect.Left) * (rect.Bottom - rect.Top);

		private void GetScreenPos(IntPtr hwnd, out int relativeX, out int relativeY) {
			NativeMethods.GetWindowRect(hwnd, out NativeStructs.RECT rect);

			foreach (Screen screen in Screen.AllScreens) {
				if (!screen.Bounds.Contains(new System.Drawing.Point(rect.Left, rect.Top)))
					continue;

				relativeX = rect.Left - screen.Bounds.X;
				relativeY = rect.Top - screen.Bounds.Y;
				return;
			}
			
			relativeX = 0;
			relativeY = 0;
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
			List<int> pids = NativeExtractor.GetProcessIDsForAUMID(session.AUMID);

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

					if (pids.Contains(pid)) {
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
			string aumid = NativeExtractor.GetAUMIDForPid(suspendedProcess.ID);
			if (MediaSessionConverter.GetApplicationType(aumid) == MediaSession.ApplicationType.UWP) {
				var diags = AppDiagnosticInfo.RequestInfoForPackageAsync(aumid.Substring(0, aumid.IndexOf("!"))).GetAwaiter().GetResult();
				foreach (AppDiagnosticInfo diag in diags) {
					var resourceGroups = diag.GetResourceGroups();

					foreach (AppResourceGroupInfo groupInfo in resourceGroups)
						groupInfo.StartResumeAsync().GetAwaiter().GetResult();
				}

				return;
			}

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
					endpoints.Add(endpoint);
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
			List<int> pids = NativeExtractor.GetProcessIDsForAUMID(session.AUMID);

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

					if (pids.Contains(pid)) {
						NativeInterfaces.ISimpleAudioVolume audioVolume = control as NativeInterfaces.ISimpleAudioVolume;
						audioVolume.GetMasterVolume(out float volume);
						return (int)(volume * 100.0f);
					}
				}
			}

			return -1;
		}

		public override byte[] GetThumbnail(MediaSession session) {
			TransportControlsMediaSessionExtractor sessionExtractor = MediaSessionExtractor.Default.TryCastOrDefault<TransportControlsMediaSessionExtractor>();
			var thumbnailStream = sessionExtractor?.GetSystemSession(session)?.TryGetMediaPropertiesAsync().GetAwaiter().GetResult().Thumbnail?.OpenReadAsync().GetAwaiter().GetResult();

			if (thumbnailStream == null)
				return new byte[0];

			IBuffer buffer = new global::Windows.Storage.Streams.Buffer((uint)thumbnailStream.Size);

			if (!thumbnailStream.CanRead)
				return new byte[0];

			buffer = thumbnailStream.ReadAsync(buffer, (uint)thumbnailStream.Size, InputStreamOptions.None).GetAwaiter().GetResult();
			byte[] bytes = new byte[buffer.Length];
			DataReader.FromBuffer(buffer).ReadBytes(bytes);

			return bytes;
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

		public override List<DirectoryItem> GetDirectoryContent(string directory) {
			if (string.IsNullOrWhiteSpace(directory)) {
				string quickAccess = "shell:::{679F85CB-0220-4080-B29B-5540CC05AAB6}";
				string thisPC = "shell:::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";

				return new List<DirectoryItem>() {
					new DirectoryItem() {
						Path = quickAccess,
						Name = "Quick Access",
						IsFolder = true,
						Icon = NativeExtractor.GetDirectoryItemIconBytes(quickAccess)
					},
					new DirectoryItem() {
						Path = thisPC,
						Name = "This PC",
						IsFolder = true,
						Icon = NativeExtractor.GetDirectoryItemIconBytes(thisPC)
					}
				};
			}

			Type shellType = Type.GetTypeFromProgID("Shell.Application");
			object shell = Activator.CreateInstance(shellType);
			Shell32.Folder folder = (Shell32.Folder)shellType.InvokeMember("NameSpace", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { directory });

			if (folder is null)
				return new List<DirectoryItem>();

			List<DirectoryItem> folders = new List<DirectoryItem>();
			List<DirectoryItem> files = new List<DirectoryItem>();

			foreach (Shell32.FolderItem item in folder.Items()) {
				if (item.IsFolder) {
					folders.Add(new DirectoryItem() {
						Path = item.Path,
						Name = item.Name,
						IsFolder = true,
						Icon = NativeExtractor.GetDirectoryItemIconBytes(item.Path)
					});
				}
				else {
					int index = item.Path.LastIndexOf(".");
					if (index >= 0 && SUPPORTED_FILES.Contains(item.Path.Substring(index).ToUpper())) {
						files.Add(new DirectoryItem() {
							Path = item.Path,
							Name = item.Name,
							IsFolder = false,
							Icon = NativeExtractor.GetDirectoryItemIconBytes(item.Path)
						});
					}
				}
			}

			folders = folders.OrderBy(item => item.Name).ToList();
			files = files.OrderBy(item => item.Name).ToList();

			return folders.Concat(files).ToList();
		}
	}
}