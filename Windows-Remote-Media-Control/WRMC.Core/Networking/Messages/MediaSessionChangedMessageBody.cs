using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for synchronization message which is sent when a media session changes the playback or the playback state on the server device.
	/// </summary>
	public class MediaSessionChangedMessageBody {
		/// <summary>
		/// The media session which changed.
		/// </summary>
		public MediaSession MediaSession { get; set; }
	}
}