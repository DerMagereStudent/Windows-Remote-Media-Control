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
		/// the artist of the media being played
		/// </summary>
		public string Artist { get; set; }

		public PlaybackState State { get; set; }

		public bool Equals(MediaSession other) => this.ProcessIDs.Equals(other.ProcessIDs) && this.Title.Equals(other.Title) && this.Artist.Equals(other.Artist);
	}
}