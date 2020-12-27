using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for a command to set the audio output device for a media session on the server device.
	/// </summary>
	public class SetAudioEndpointMessageBody : AuthenticatedMessageBody {
		/// <summary>
		/// The media session whose audio output should be set to the specified audio output device.
		/// </summary>
		public MediaSession MediaSession { get; set; }

		/// <summary>
		/// The new audio output device.
		/// </summary>
		public AudioEndpoint AudioEndpoint { get; set; }
	}
}