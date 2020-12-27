namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the data body for a command to play a media file on the server device.
	/// </summary>
	public class PlayFileMessageBody : AuthenticatedMessageBody {
		/// <summary>
		/// The absolute path of the file which should be played.
		/// </summary>
		public string FilePath { get; set; }
	}
}