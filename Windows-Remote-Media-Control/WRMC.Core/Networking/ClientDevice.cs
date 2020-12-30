using System.Net;

using Newtonsoft.Json;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents all necessary information about a client.
	/// </summary>
	public class ClientDevice {
		/// <summary>
		/// The ID of the client.
		/// </summary>
		public string ID { get; set; }

		/// <summary>
		/// The device name of the client.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The local IP-Addresse of the client.
		/// </summary>
		[JsonConverter(typeof(IPAddressJsonConverter))]
		public IPAddress IPAddress { get; set; }

		/// <summary>
		/// The session id of the client.
		/// </summary>
		public long SessionID { get; set; }
	}
}