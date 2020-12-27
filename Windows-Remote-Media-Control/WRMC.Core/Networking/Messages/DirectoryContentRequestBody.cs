namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for a request for the content of a specific directory on the server device.
	/// </summary>
	public class DirectoryContentRequestBody : AuthenticatedMessageBody {
		/// <summary>
		/// The requested directory.
		/// </summary>
		public string Directory { get; set; }
	}
}