using System.Collections.Generic;

using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for the response data containing all active media sessions on the server device.
	/// </summary>
	public class MediaSessionsResponseBody {
		/// <summary>
		/// List of all media sessions.
		/// </summary>
		public List<MediaSession> MediaSessions { get; set; }

		/// <summary>
		/// The index of the most recently updated session on the server device. 
		/// </summary>
		public int Current { get; set; }
	}
}