using System.Collections.Generic;

using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for the response data containing all files and folders inside a specific directory on the server device.
	/// </summary>
	public class DirectoryContentResponseBody : MessageBody {
		/// <summary>
		/// List of all items inside the requested directory.
		/// </summary>
		public List<DirectoryItem> Items { get; set; }
	}
}