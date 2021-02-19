namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents a request for data.
	/// </summary>
	public class Request {
		/// <summary>
		/// An enum with all possible request types.
		/// </summary>
		public enum Type {
			Connect,
			FindServer,
			GetAudioEndpoints,
			GetDirectoryContent,
			GetMediaSessions,
			GetScreens,
			GetSuspendedMediaProcesses,
			GetThumbnail,
			GetVolume
		}

		/// <summary>
		/// The type/method of the request.
		/// </summary>
		public Type Method { get; set; }

		/// <summary>
		/// The data body of the request.
		/// </summary>
		public MessageBody Body { get; set; }
	}
}