using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using WRMC.Core.Networking;

using Newtonsoft.Json;

namespace WRMC.Android.Networking {
	public static class ConnectionManager {
		private static readonly string KNOWN_SERVERS_FILE_NAME = Path.Combine("/Interner Speicher/Documents/WRMC.Android", "known_clients.json");

		private static UdpClient udpClient;
		private static TcpClient tcpClient;

		private static ServerDevice pendingServerDevice;

		public static Dictionary<ServerDevice, long> KnownServers { get; set; }
		public static bool IsSearchingServers { get; private set; }

		public static ServerDevice CurrentServer { get; set; }

		public static event EventHandler<MessageBodyEventArgs<ServerResponseBody>> OnFindServerResponseReceived = null;
		public static event EventHandler<EventArgs> OnFindServerFinished = null;

		public static event EventHandler<EventArgs> OnTcpConnectSuccess = null;
		public static event EventHandler<EventArgs> OnTcpConnectFailure = null;
		public static event EventHandler<EventArgs> OnConnectSuccess = null;
		public static event EventHandler<EventArgs> OnConnectFailure = null;

		static ConnectionManager() {
			//ConnectionManager.KnownServers = new Dictionary<ServerDevice, long>() {
			//	{ new ServerDevice() { ID = "a", Name = "Samsung S9" }, 0 },
			//	{ new ServerDevice() { ID = "b", Name = "Desktop-MXTF6Z" }, 0 },
			//	{ new ServerDevice() { ID = "c", Name = "PC-MaPa" }, 0 }
			//};

			ConnectionManager.LoadKnownServers();

			ConnectionManager.udpClient = new UdpClient();
			ConnectionManager.udpClient.OnServersResponseReceived += UdpClient_OnServersResponseReceived;
			ConnectionManager.udpClient.OnFindServerFinished += UdpClient_OnFindServerFinished;

			ConnectionManager.tcpClient = new TcpClient();
			ConnectionManager.tcpClient.OnConnectionClosed += TcpClient_OnConnectionClosed;
			ConnectionManager.tcpClient.OnTcpConnectSuccess += TcpClient_OnTcpConnectSuccess;
			ConnectionManager.tcpClient.OnTcpConnectFailure += TcpClient_OnTcpConnectFailure;
			ConnectionManager.tcpClient.OnConnectSuccess += TcpClient_OnConnectSuccess;
			ConnectionManager.tcpClient.OnMessageReceived += TcpClient_OnMessageReceived;
			ConnectionManager.tcpClient.OnRequestReceived += TcpClient_OnRequestReceived;
			ConnectionManager.tcpClient.OnResponseReceived += TcpClient_OnResponseReceived;
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

		public static bool StartConnect(ServerDevice device) {
			ConnectionManager.pendingServerDevice = device;
			return ConnectionManager.tcpClient.Start(device);
		}

		public static void StopConnect() {
			ConnectionManager.tcpClient.Stop();
		}

		public static void CloseConnection(ClientDevice clientDevice) {
			clientDevice.SessionID = ConnectionManager.KnownServers.ContainsKey(ConnectionManager.CurrentServer) ? ConnectionManager.KnownServers[ConnectionManager.CurrentServer] : 0;
			ConnectionManager.tcpClient.Stop(clientDevice);
		}

		public static void SendConnectRequest(ClientDevice clientDevice) {
			clientDevice.SessionID = ConnectionManager.KnownServers.ContainsKey(ConnectionManager.pendingServerDevice) ? ConnectionManager.KnownServers[ConnectionManager.pendingServerDevice] : 0;

			ConnectionManager.tcpClient.SendRequest(new Request() {
				Method = Request.Type.Connect,
				Body = new AuthenticatedMessageBody() {
					ClientDevice = clientDevice
				}
			});
		}

		public static void SendRequest(Request request) {
			ConnectionManager.tcpClient.SendRequest(request);
		}

		private static void UdpClient_OnServersResponseReceived(UdpClient sender, MessageBodyEventArgs<ServerResponseBody> e) {
			ConnectionManager.OnFindServerResponseReceived?.Invoke(null, e);
		}

		private static void UdpClient_OnFindServerFinished(UdpClient sender, EventArgs e) {
			ConnectionManager.IsSearchingServers = false;
			ConnectionManager.OnFindServerFinished?.Invoke(null, e);
		}

		private static void TcpClient_OnConnectionClosed(TcpClient sender, EventArgs e) {

		}

		private static void TcpClient_OnTcpConnectSuccess(TcpClient sender, ServerEventArgs e) {
			ConnectionManager.OnTcpConnectSuccess?.Invoke(null, EventArgs.Empty);
		}

		private static void TcpClient_OnTcpConnectFailure(TcpClient sender, ServerEventArgs e) {
			ConnectionManager.OnTcpConnectFailure?.Invoke(null, EventArgs.Empty);
		}

		private static void TcpClient_OnConnectSuccess(TcpClient sender, ClientEventArgs e) {
			if (ConnectionManager.KnownServers.ContainsKey(ConnectionManager.pendingServerDevice))
				ConnectionManager.KnownServers[ConnectionManager.pendingServerDevice] = e.ClientDevice.SessionID;
			else
				ConnectionManager.KnownServers.Add(ConnectionManager.pendingServerDevice, e.ClientDevice.SessionID);

			ConnectionManager.SaveKnownServers();

			ConnectionManager.CurrentServer = ConnectionManager.pendingServerDevice;
			ConnectionManager.OnConnectSuccess?.Invoke(null, EventArgs.Empty);
		}

		private static void TcpClient_OnMessageReceived(TcpClient sender, ServerMessageEventArgs e) {

		}

		private static void TcpClient_OnRequestReceived(TcpClient sender, ServerRequestEventArgs e) {

		}

		private static void TcpClient_OnResponseReceived(TcpClient sender, ServerResponseEventArgs e) {

		}

		private static void LoadKnownServers() {
			try {
				ConnectionManager.KnownServers = File.Exists(ConnectionManager.KNOWN_SERVERS_FILE_NAME) ? JsonConvert.DeserializeObject<Dictionary<ServerDevice, long>>(File.ReadAllText(ConnectionManager.KNOWN_SERVERS_FILE_NAME), SerializationOptions.DefaultSerializationOptions) : new Dictionary<ServerDevice, long>();
			} catch(IOException) {
				ConnectionManager.KnownServers = new Dictionary<ServerDevice, long>();
			}
		}

		private static void SaveKnownServers() {
			try {
				File.WriteAllText(ConnectionManager.KNOWN_SERVERS_FILE_NAME, JsonConvert.SerializeObject(ConnectionManager.KnownServers, SerializationOptions.DefaultSerializationOptions));
			} catch (IOException) { }
		}
	}
}