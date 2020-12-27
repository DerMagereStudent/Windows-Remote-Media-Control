namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for the response data containing the information of a server device.
	/// </summary>
	public class ServerResponseBody {
		/// <summary>
		/// The server device information.
		/// </summary>
		public ServerDevice Server { get; set; }
	}
}