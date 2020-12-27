using System.Collections.Generic;

using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for the response data containing all suspended media processes of the server device.
	/// </summary>
	public class SuspendedProcessesResponseBody {
		/// <summary>
		/// List of all suspended processes.
		/// </summary>
		public List<SuspendedProcess> Processes { get; set; }
	}
}