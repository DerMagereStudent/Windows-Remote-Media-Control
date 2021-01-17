using System;
using System.Net;

using Newtonsoft.Json;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents all necessary information about a server.
	/// </summary>
	public class ServerDevice : IEquatable<ServerDevice> {
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
		[JsonConverter(typeof(IPAddressJsonConverter))]
		public IPAddress IPAddress { get; set; }

		public bool Equals(ServerDevice other) {
			if (this.ID == null && other.ID != null || this.ID != null && other.ID == null)
				return false;

			if (this.Name == null && other.Name != null || this.Name != null && other.Name == null)
				return false;

			if (this.IPAddress == null && other.IPAddress != null || this.IPAddress != null && other.IPAddress == null)
				return false;

			return this.ID.Equals(other.ID) && this.IPAddress.Equals(other.IPAddress);
		}
	}
}