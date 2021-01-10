using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Management.Deployment;
using Windows.Media.Control;

using WRMC.Core.Models;
using WRMC.Windows.Native;

namespace WRMC.Windows.Media {
	public static class MediaSessionConverter {
		public static MediaSession FromWindowsMediaTransportControls(GlobalSystemMediaTransportControlsSession session) {
			MediaSession.ApplicationType type = GetApplicationType(session);

			List<int> ids = new List<int>();
			string name = null;

			Process[] processes = Process.GetProcesses();

			foreach (Process p in processes) {
				try {
					if (type == MediaSession.ApplicationType.UWP) {
						IntPtr hProcess = NativeMethods.OpenProcess(NativeDefinitions.PROCESS_QUERY_LIMITED_INFORMATION, false, p.Id);
						
						uint sbc = 256;
						StringBuilder sbApplicationUserModelId = new StringBuilder((int)sbc);
						NativeMethods.GetApplicationUserModelId(hProcess, ref sbc, sbApplicationUserModelId);

						string sApplicationUserModelId = sbApplicationUserModelId.ToString();

						if (sApplicationUserModelId.Equals(session.SourceAppUserModelId)) {
							ids.Add(p.Id);
							name = p.MainModule.FileName;
						}

						NativeMethods.CloseHandle(hProcess);
					} else if (p.MainModule.FileName.Contains(session.SourceAppUserModelId)) {
						ids.Add(p.Id);
						name = p.MainModule.FileName;
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
							(playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused ? MediaSession.PlaybackState.Paused : MediaSession.PlaybackState.None),
					AppType = type,
					AUMID = session.SourceAppUserModelId
				};

				return ms;
			} catch (Exception) {
				return null;
			}
		}

		private static MediaSession.ApplicationType GetApplicationType(GlobalSystemMediaTransportControlsSession session) {
			PackageManager packageManager = new PackageManager();

			foreach (Package package in packageManager.FindPackagesForUserWithPackageTypes("", PackageTypes.Main | PackageTypes.Optional)) {
				IReadOnlyList<AppListEntry> entries = package.GetAppListEntries();
				foreach (AppListEntry entry in entries)
					if (entry.AppUserModelId.Equals(session.SourceAppUserModelId))
						return MediaSession.ApplicationType.UWP;
			}

			return MediaSession.ApplicationType.Other;
		}
	}
}