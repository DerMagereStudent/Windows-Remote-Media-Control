namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents an answer for a request providing the requested data.
	/// </summary>
	public class Response {
		/// <summary>
		/// An enum with all possible response types.
		/// </summary>
		public enum Type {
			AudioEndpoints,
			ConnectSuccess,
			DirectoryContent,
			MediaSessions,
			Ping,
			Screens,
			Servers,
			SuspendedProcesses,
			Thumbnail,
			Volume
		}

		/// <summary>
		/// The type/method of the response.
		/// </summary>
		public Type Method { get; set; }

		/// <summary>
		/// The data body of the response.
		/// </summary>
		public MessageBody Body { get; set; }
	}
}