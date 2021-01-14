using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for a command to set the screen for a media session on the server device.
	/// </summary>
	public class SetScreenMessageBody : AuthenticatedMessageBody, IMediaSessionMessageBody {
		/// <summary>
		/// The media session whose process should be moved to the specified screen.
		/// </summary>
		public MediaSession MediaSession { get; set; }

		/// <summary>
		/// The name of the screen the media playback should be displayed on.
		/// </summary>
		public string Screen { get; set; }
	}
}