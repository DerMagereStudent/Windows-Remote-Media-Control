using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for a command to set the visual and auditory output devices for a media session on the server device.
	/// </summary>
	public class SetConfigurationMessageBody : AuthenticatedMessageBody, IMediaSessionMessageBody {
		/// <summary>
		/// The media session whose visual and auditory output is to be discontinued.
		/// </summary>
		public MediaSession MediaSession { get; set; }

		/// <summary>
		/// The screen-audio configuration which should be activated.
		/// </summary>
		public Configuration Configuration { get; set; }
	}
}