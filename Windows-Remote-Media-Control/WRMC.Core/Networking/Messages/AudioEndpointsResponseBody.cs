using System.Collections.Generic;

using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for the response data containing all audio output devices of the server device.
	/// </summary>
	public class AudioEndpointsResponseBody : MessageBody {
		/// <summary>
		/// List of all audio output devices of the server sevice.
		/// </summary>
		public List<AudioEndpoint> AudioEndpoints { get; set; } 
	}
}