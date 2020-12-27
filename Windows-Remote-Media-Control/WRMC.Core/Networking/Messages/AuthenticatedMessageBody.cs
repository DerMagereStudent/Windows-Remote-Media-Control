namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the base of every request or message which requires a connected accepted by the server which implies a valid session id.
	/// </summary>
	public class AuthenticatedMessageBody : MessageBody {
		/// <summary>
		/// The client device information to authenticate.
		/// </summary>
		public ClientDevice ClientDevice { get; set; }
	}
}