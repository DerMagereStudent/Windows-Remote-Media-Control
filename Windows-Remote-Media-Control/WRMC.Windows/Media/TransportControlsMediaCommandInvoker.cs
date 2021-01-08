using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Windows.Media.Control;

using WRMC.Core.Models;
using WRMC.Windows.Native;

namespace WRMC.Windows.Media {
	public class TransportControlsMediaCommandInvoker : MediaCommandInvoker {
		GlobalSystemMediaTransportControlsSessionManager manager;

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
			List<IntPtr> wndHandles = new List<IntPtr>();

			foreach (Process p in processes)
				wndHandles.AddRange(p.GetWindowHandles());

			foreach (IntPtr hwnd in wndHandles) {
				if (!NativeMethods.IsWindowVisible(hwnd))
					continue;

				NativeStructs.RECT rect = new NativeStructs.RECT();
				NativeMethods.GetWindowRect(hwnd, out rect);

				if (rect.Left == 0 && rect.Top == 0 && rect.Right == 0 && rect.Bottom == 0)
					continue;

				if (string.IsNullOrEmpty(NativeMethods.GetWindowTitle(hwnd)))
					continue;

				NativeMethods.ShowWindow(hwnd, NativeDefinitions.SW_RESTORE);
				NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, screen.Bounds.X, screen.Bounds.Y, 0, 0, NativeDefinitions.SWP_NOSIZE | NativeDefinitions.SWP_NOZORDER);
			}
		}

		public override void SetAudioEndpoint(MediaSession session, AudioEndpoint endpoint) {

		}

		public override void SetVolume(MediaSession session, int volume) {

		}
	}
}