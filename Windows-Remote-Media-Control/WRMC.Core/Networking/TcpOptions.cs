using System.Net;

namespace WRMC.Core.Networking {
	public static class TcpOptions {
		/// <summary>
		/// The defualt port used for a TCP connection.
		/// </summary>
		public static int DefaultPort = 6198;

		/// <summary>
		/// The default maximum tries to build a TCP connection.
		/// </summary>
		public static int DefaultMaxConnectTries = 3;

		public static int BufferSize = 4096;

		/// <summary>
		/// The default IP address the server listens to.
		/// </summary>
		public static IPAddress DefaultListenIPAddress = IPAddress.Any;

		/// <summary>
		/// The default IP end point the server listens to.
		/// </summary>
		public static IPEndPoint DefaultServerListenIPEndPoint = new IPEndPoint(IPAddress.Any, DefaultPort);
	}
}