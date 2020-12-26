using System.Net;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents all necessary information about a server.
	/// </summary>
	public class ServerDevice {
		/// <summary>
		/// The ID of the server.
		/// </summary>
		public string ID { get; set; }

		/// <summary>
		/// The device name of the server.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The local IP-Addresse of the server.
		/// </summary>
		public IPAddress IPAddress { get; set; }
	}
}