using System.Collections.Generic;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for the response data containing all screens of the server device.
	/// </summary>
	public class ScreensResponseBody : MessageBody {
		/// <summary>
		/// List of all screens.
		/// </summary>
		public List<string> Screens { get; set; }
	}
}