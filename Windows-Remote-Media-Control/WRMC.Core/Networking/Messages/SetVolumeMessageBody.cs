namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for a command to set the volume on the server device.
	/// </summary>
	public class SetVolumeMessageBody : AuthenticatedMessageBody {
		/// <summary>
		/// The new volume.
		/// </summary>
		public int Volume { get; set; }
	}
}