using System.Collections.Generic;

using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for a synchronization message which is sent when a session was created or closed on the server device.
	/// </summary>
	public class MediaSessionListChangedMessageBody : MessageBody {
		/// <summary>
		/// List of all media sessions on the server device.
		/// </summary>
		public List<MediaSession> MediaSessions { get; set; }
	}
}