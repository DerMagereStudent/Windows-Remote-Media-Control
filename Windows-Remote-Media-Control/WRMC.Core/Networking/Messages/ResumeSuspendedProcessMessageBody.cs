using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for a command to resume a suspended process on the server device.
	/// </summary>
	public class ResumeSuspendedProcessMessageBody : AuthenticatedMessageBody {
		/// <summary>
		/// The process, which should be resumed.
		/// </summary>
		public SuspendedProcess Process { get; set; }
	}
}