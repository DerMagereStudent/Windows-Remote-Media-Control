namespace WRMC.Core.Networking {
	public static class TcpClientExtensions {
		public static bool IsConnected(this System.Net.Sockets.TcpClient client) {
			if (client.Client == null)
				return false;

			if (!client.Connected)
				return false;

			if (!(client.Client.Poll(0, System.Net.Sockets.SelectMode.SelectWrite) && !client.Client.Poll(0, System.Net.Sockets.SelectMode.SelectError)))
				return false;

			//byte[] buffer = new byte[1];
			//return client.Client.Receive(buffer, System.Net.Sockets.SocketFlags.Peek) != 0;
			return true;
		}
	}
}