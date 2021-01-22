using System;
using System.Collections.Generic;

namespace WRMC.Core.Models {
	/// <summary>
	/// Represents a single windows transport controls media session.
	/// </summary>
	public class MediaSession : IEquatable<MediaSession> {
		public enum PlaybackState {
			None,
			Paused,
			Playing
		}

		public enum MediaType {
			Video,
			Audio,
			Other
		}

		public enum ApplicationType {
			UWP,
			Other
		}

		/// <summary>
		/// The ids of the corresponding processes.
		/// </summary>
		public List<int> ProcessIDs { get; set; }

		/// <summary>
		/// The name of the corresponding process.
		/// </summary>
		public string ProcessName { get; set; }

		/// <summary>
		/// The title of the media being played.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// the artist of the media being played.
		/// </summary>
		public string Artist { get; set; }

		/// <summary>
		/// The current state of the media playback.
		/// </summary>
		public PlaybackState State { get; set; }

		/// <summary>
		/// The type of the media playing back.
		/// </summary>
		public MediaType Type { get; set; }

		/// <summary>
		/// The type of the application playing back.
		/// </summary>
		public ApplicationType AppType { get; set; }

		public string AUMID { get; set; }

		public void Update(MediaSession session) {
			this.AppType = session.AppType;
			this.Artist = session.Artist;
			this.AUMID = session.AUMID;
			this.ProcessIDs = session.ProcessIDs;
			this.ProcessName = session.ProcessName;
			this.State = session.State;
			this.Title = session.Title;
		}

		public bool Equals(MediaSession other) => /*this.ProcessIDs.Equals(other.ProcessIDs) && */this.ProcessName.Equals(other.ProcessName) && this.AUMID.Equals(other.AUMID);
	}
}