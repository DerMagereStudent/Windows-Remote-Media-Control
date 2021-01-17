using System;
using System.Collections.Generic;

using WRMC.Core.Networking;

namespace WRMC.Android.Networking {
	public static class ConnectionManager {
		private static UdpClient udpClient;

		public static List<ServerDevice> KnownServers { get; set; }
		public static bool IsSearchingServers { get; private set; }

		public static event EventHandler<MessageBodyEventArgs<ServerResponseBody>> OnFindServerResponseReceived = null;
		public static event EventHandler<EventArgs> OnFindServerFinished = null;

		static ConnectionManager() {
			ConnectionManager.KnownServers = new List<ServerDevice>() {
				new ServerDevice() { Name = "Samsung S9" },
				new ServerDevice() { Name = "Desktop-MXTF6Z" },
				new ServerDevice() { Name = "PC-MaPa" }
			};

			ConnectionManager.udpClient = new UdpClient();
			ConnectionManager.udpClient.OnServersResponseReceived += UdpClient_OnServersResponseReceived;
			ConnectionManager.udpClient.OnFindServerFinished += UdpClient_OnFindServerFinished;
		}

		public static void StartFindServer() {
			if (ConnectionManager.IsSearchingServers)
				return;

			ConnectionManager.udpClient.StartListening();
			ConnectionManager.udpClient.StartSending(100, 5000);
			ConnectionManager.IsSearchingServers = true;
		}

		public static void StopFindServer() {
			if (!ConnectionManager.IsSearchingServers)
				return;

			ConnectionManager.udpClient.StopListening();
			ConnectionManager.udpClient.StopSending();
			ConnectionManager.IsSearchingServers = false;
		}

		private static void UdpClient_OnServersResponseReceived(UdpClient sender, MessageBodyEventArgs<ServerResponseBody> e) {
			ConnectionManager.OnFindServerResponseReceived?.Invoke(null, e);
		}

		private static void UdpClient_OnFindServerFinished(UdpClient sender, EventArgs e) {
			ConnectionManager.IsSearchingServers = false;
			ConnectionManager.OnFindServerFinished?.Invoke(null, e);
		}
	}
}