namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents a command or a synchronization message which does not expect an answer.
	/// </summary>
	public class Message {
		/// <summary>
		/// An enum with all possible message types.
		/// </summary>
		public enum Type {
			Disconnect,
			MediaSessionChanged,
			MediaSessionListChanged,
			NextMedia,
			PlayFile,
			PlayPause,
			PreviousMedia,
			ResumeSuspendedProcess,
			SetAudioEndpoint,
			SetConfiguration,
			SetScreen,
			SetVolume
		}

		/// <summary>
		/// The type/method of the message.
		/// </summary>
		public Type Method { get; set; }

		/// <summary>
		/// The data body of the message.
		/// </summary>
		public MessageBody Body { get; set; }
	}
}