using System.Collections.Generic;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for the response data containing all files and folders inside a specific directory on the server device.
	/// </summary>
	public class DirectoryContentResponseBody : MessageBody {
		/// <summary>
		/// List of all folders inside the requested directory.
		/// </summary>
		public List<string> Folders { get; set; }

		/// <summary>
		/// List of all files inside the requested directory.
		/// </summary>
		public List<string> Files { get; set; }
	}
}