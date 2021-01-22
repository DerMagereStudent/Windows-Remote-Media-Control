using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using Windows.Media.Control;

using WRMC.Core;
using WRMC.Core.Models;

namespace WRMC.Windows.Media {
	public class TransportControlsMediaSessionExtractor : MediaSessionExtractor {
		private GlobalSystemMediaTransportControlsSessionManager manager;
		private Dictionary<GlobalSystemMediaTransportControlsSession, MediaSession> sessions;

		public override List<MediaSession> Sessions => new List<MediaSession>(this.sessions.Values);

		public override event TypedEventHandler<MediaSessionExtractor, EventArgs> OnSessionsChanged = null;
		public override event TypedEventHandler<MediaSessionExtractor, EventArgs<MediaSession>> OnSessionChanged = null;

		public TransportControlsMediaSessionExtractor() {
			this.manager = GlobalSystemMediaTransportControlsSessionManager.RequestAsync().GetAwaiter().GetResult();
			this.sessions = new Dictionary<GlobalSystemMediaTransportControlsSession, MediaSession>();

			this.manager.SessionsChanged += (s, e) => this.UpdateSessionsList();
		}

		public GlobalSystemMediaTransportControlsSession GetSystemSession(MediaSession session) {
			foreach (var keyValuePair in this.sessions)
				if (keyValuePair.Value.Equals(session))
					return keyValuePair.Key;

			return null;
		}

		private void UpdateSessionsList() {
			List<GlobalSystemMediaTransportControlsSession> newSessions = new List<GlobalSystemMediaTransportControlsSession>();

			foreach (var session in this.manager.GetSessions()) {
				MediaSession ms = MediaSessionConverter.FromWindowsMediaTransportControls(session);

				if (ms == null)
					continue;

				if (this.sessions.ContainsKey(session)) {
					this.sessions[session] = ms;
					newSessions.Add(session);
					continue;
				}

				session.MediaPropertiesChanged += this.OnMediaPropertiesChanged;
				session.PlaybackInfoChanged += this.OnPlaybackInfoChanged;
				//session.TimelinePropertiesChanged += (s, e) => this.UpdateSessions();

				this.sessions.Add(session, ms);

				newSessions.Add(session);
			}

			List<GlobalSystemMediaTransportControlsSession> keysToRemove = new List<GlobalSystemMediaTransportControlsSession>();

			foreach (var key in this.sessions.Keys) {
				if (!newSessions.Contains(key)) {
					key.MediaPropertiesChanged -= this.OnMediaPropertiesChanged;
					key.PlaybackInfoChanged -= this.OnPlaybackInfoChanged;
					keysToRemove.Add(key);
				}
			}

			foreach (var key in keysToRemove)
				this.sessions.Remove(key);

			this.OnSessionsChanged?.Invoke(this, EventArgs.Empty);
		}

		public override void Initialise() {
			this.UpdateSessionsList();
		}

		private void OnMediaPropertiesChanged(object sender, MediaPropertiesChangedEventArgs e) {
			this.UpdateSession(sender as GlobalSystemMediaTransportControlsSession);
		}

		private void OnPlaybackInfoChanged(object sender, PlaybackInfoChangedEventArgs e) {
			var session = sender as GlobalSystemMediaTransportControlsSession;

			if (!this.sessions.ContainsKey(session))
				return;

			var playbackInfo = session.GetPlaybackInfo();

			if (this.sessions[session].State == MediaSession.PlaybackState.Paused && playbackInfo.PlaybackStatus != GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
				return;

			if (playbackInfo.PlaybackStatus != GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing && playbackInfo.PlaybackStatus != GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
				return;

			this.UpdateSession(session);
		}

		private void UpdateSession(GlobalSystemMediaTransportControlsSession session) {
			MediaSession ms = MediaSessionConverter.FromWindowsMediaTransportControls(session);

			if (ms == null) {
				if (this.sessions.ContainsKey(session))
					this.sessions.Remove(session);

				this.OnSessionsChanged?.Invoke(this, EventArgs.Empty);
				return;
			}

			if (this.sessions.ContainsKey(session))
				this.sessions[session] = ms;
			else
				this.sessions.Add(session, ms);

			this.OnSessionChanged?.Invoke(this, new EventArgs<MediaSession>(ms));
		}
	}
}