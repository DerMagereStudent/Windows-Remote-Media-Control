namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for the response data containing the volume on the server device.
	/// </summary>
	public class VolumeResponseBody {
		/// <summary>
		/// The current volume on the server device.
		/// </summary>
		public int Volume { get; set; }
	}
}