using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using Windows.Media.Control;

using WRMC.Core;
using WRMC.Core.Models;

namespace WRMC.Windows.Media {
	public class TransportControlsMediaSessionExtractor : MediaSessionExtractor {
		private GlobalSystemMediaTransportControlsSessionManager manager;
		private SynchronizedCollection<GlobalSystemMediaTransportControlsSession> observedSessions;

		public override event TypedEventHandler<MediaSessionExtractor, EventArgs> OnSessionsChanged = null;

		public TransportControlsMediaSessionExtractor() {
			this.manager = GlobalSystemMediaTransportControlsSessionManager.RequestAsync().GetAwaiter().GetResult();
			this.Sessions = new SynchronizedCollection<MediaSession>();
			this.observedSessions = new SynchronizedCollection<GlobalSystemMediaTransportControlsSession>();

			this.manager.SessionsChanged += (s, e) => this.UpdateSessionsList();
		}

		private void UpdateSessionsList() {
			List<GlobalSystemMediaTransportControlsSession> newSessions = new List<GlobalSystemMediaTransportControlsSession>();

			foreach (var session in this.manager.GetSessions()) {
				if (this.observedSessions.Contains(session)) {
					newSessions.Add(session);
					continue;
				}

				session.MediaPropertiesChanged += this.OnMediaPropertiesChanged;
				session.PlaybackInfoChanged += this.OnPlaybackInfoChanged;
				//session.TimelinePropertiesChanged += (s, e) => this.UpdateSessions();

				this.observedSessions.Add(session);
				newSessions.Add(session);
			}

			for (int i = this.observedSessions.Count - 1; i >= 0; i--) {
				if (!newSessions.Contains(this.observedSessions[i])) {
					this.observedSessions[i].MediaPropertiesChanged -= this.OnMediaPropertiesChanged;
					this.observedSessions[i].PlaybackInfoChanged -= this.OnPlaybackInfoChanged;
					this.observedSessions.Remove(this.observedSessions[i]);
				}
			}

			this.UpdateSessions();
		}

		public override void Initialise() {
			this.UpdateSessionsList();
		}

		private void OnMediaPropertiesChanged(object sender, MediaPropertiesChangedEventArgs e) {
			this.UpdateSessions();
		}

		private void OnPlaybackInfoChanged(object sender, PlaybackInfoChangedEventArgs e) {
			this.UpdateSessions();
		}

		private void UpdateSessions() {
			this.Sessions.Clear();

			foreach (var session in this.manager.GetSessions()) {
				int id = 0;
				string name = null;

				Process[] processes = Process.GetProcesses();

				foreach (Process p in processes) {
					try {
						if (p.MainModule.FileName.Contains(session.SourceAppUserModelId)) {
							id = p.Id;
							name = p.MainModule.FileName;
							break;
						}
						else {
							if (p.MainModule.FileName.Contains("WindowsApps")) {
								if (p.MainModule.FileName.Contains("Video.UI.exe")) {
									id = p.Id;
									name = p.MainModule.FileName;
									break;
								}
							}
						}
					}
					catch (Win32Exception e) { }
					catch (InvalidOperationException) { }
				}

				if (id == 0 || string.IsNullOrEmpty(name))
					continue;

				var mediaProperties = session.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
				var playbackInfo = session.GetPlaybackInfo();

				MediaSession ms = new MediaSession() {
					ProcessID = id,
					ProcessName = name,
					Title = mediaProperties.Title,
					Artist = mediaProperties.Artist,
					State = playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing ? MediaSession.PlaybackState.Playing :
							(playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused ? MediaSession.PlaybackState.Paused : MediaSession.PlaybackState.None)
				};

				if (!this.Sessions.Contains(ms))
					this.Sessions.Add(ms);
			}

			this.OnSessionsChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}