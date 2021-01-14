using System;
using System.Collections.Generic;
using System.Text;

using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	public class MediaSessionMessageBody : AuthenticatedMessageBody, IMediaSessionMessageBody {
		/// <summary>
		/// The media session the server should interact with.
		/// </summary>
		public MediaSession MediaSession { get; set; }
	}
}