using System.Net;

namespace WRMC.Core.Networking {
	public static class UdpOptions {
		/// <summary>
		/// The port the udp server listens to.
		/// </summary>
		public static int DefaultServerPort = 6196;

		/// <summary>
		/// The port that the UDP client is listening on. Different from the default server port to prevent the server from receiving its own message while sending and vice versa.
		/// </summary>
		public static int DefaultClientPort = 6197;

		/// <summary>
		/// The default IP end point the server listens to.
		/// </summary>
		public static IPEndPoint DefaultServerListenIPEndPoint = new IPEndPoint(IPAddress.Any, DefaultServerPort);

		/// <summary>
		/// The default IP end point the client listens to.
		/// </summary>
		public static IPEndPoint DefaultClientListenIPEndPoint = new IPEndPoint(IPAddress.Any, DefaultClientPort);

		/// <summary>
		/// The default IP end point the server addresses when sending data.
		/// </summary>
		public static IPEndPoint DefaultServerSendingIPEndPoint = new IPEndPoint(IPAddress.Broadcast, DefaultClientPort);

		/// <summary>
		/// The default IP end point the client addresses when sending data.
		/// </summary>
		public static IPEndPoint DefaultClientSendingIPEndPoint = new IPEndPoint(IPAddress.Broadcast, DefaultServerPort);
	}
}
