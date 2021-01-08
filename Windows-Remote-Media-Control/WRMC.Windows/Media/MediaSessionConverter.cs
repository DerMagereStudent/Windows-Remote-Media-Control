using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using Windows.Media.Control;

using WRMC.Core.Models;

namespace WRMC.Windows.Media {
	public static class MediaSessionConverter {
		public static MediaSession FromWindowsMediaTransportControls(GlobalSystemMediaTransportControlsSession session) {
			List<int> ids = new List<int>(); ;
			string name = null;

			Process[] processes = Process.GetProcesses();

			foreach (Process p in processes) {
				try {
					if (p.MainModule.FileName.Contains(session.SourceAppUserModelId)) {
						ids.Add(p.Id);
						name = p.MainModule.FileName;

						foreach (Process cp in processes) {
							try {
								if (cp.MainModule.FileName.Contains(session.SourceAppUserModelId) && cp.Id != p.Id)
									ids.Add(cp.Id);
							}
							catch (Win32Exception) { }
							catch (InvalidOperationException) { }
						}

						break;
					}
					else {
						if (p.MainModule.FileName.Contains("WindowsApps")) {
							if (p.MainModule.FileName.Contains("Video.UI.exe")) {
								ids.Add(p.Id);
								name = p.MainModule.FileName;
							}
						}
					}
				}
				catch (Win32Exception) { }
				catch (InvalidOperationException) { }
			}

			if (ids.Count == 0 || string.IsNullOrEmpty(name))
				return null;

			try {
				var mediaProperties = session.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
				var playbackInfo = session.GetPlaybackInfo();

				MediaSession ms = new MediaSession() {
					ProcessIDs = ids,
					ProcessName = name,
					Title = mediaProperties.Title,
					Artist = mediaProperties.Artist,
					State = playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing ? MediaSession.PlaybackState.Playing :
							(playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused ? MediaSession.PlaybackState.Paused : MediaSession.PlaybackState.None)
				};

				return ms;
			} catch (Exception) {
				return null;
			}
		}
	}
}