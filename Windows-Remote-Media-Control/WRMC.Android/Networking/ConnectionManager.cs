using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Content;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

using WRMC.Core;
using WRMC.Core.Models;
using WRMC.Core.Networking;

using Newtonsoft.Json;

namespace WRMC.Android.Networking {
	public static class ConnectionManager {
		private static readonly string KNOWN_SERVERS_FILE_NAME = "known_servers.json";
		private static readonly string KNOWN_SERVERS_DIRECTORY_NAME = Path.Combine(global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryDocuments).AbsolutePath, "WRMC.Android");

		private static UdpClient udpClient;
		private static TcpClient tcpClient;

		private static ServerDevice pendingServerDevice;

		[JsonConverter(typeof(KeyedListJsonConverter<ServerDevice, long>))]
		public static KeyedList<ServerDevice, long> KnownServers { get; set; }

		public static bool IsSearchingServers { get; private set; }

		public static ServerDevice CurrentServer { get; set; }

		public static event EventHandler<MessageBodyEventArgs<ServerResponseBody>> OnFindServerResponseReceived = null;
		public static event EventHandler<EventArgs> OnFindServerFinished = null;

		public static event EventHandler<EventArgs> OnTcpConnectSuccess = null;
		public static event EventHandler<EventArgs> OnTcpConnectFailure = null;
		public static event EventHandler<EventArgs> OnConnectSuccess = null;
		public static event EventHandler<EventArgs> OnConnectFailure = null;
		public static event EventHandler<EventArgs> OnConnectionClosed = null;

		public static event EventHandler<EventArgs<List<MediaSession>>> OnMediaSessionsReceived = null;
		public static event EventHandler<EventArgs<MediaSession>> OnMediaSessionChanged = null;

		static ConnectionManager() {
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
			if (request.Body is AuthenticatedMessageBody)
				(request.Body as AuthenticatedMessageBody).ClientDevice.SessionID = ConnectionManager.KnownServers.ContainsKey(ConnectionManager.CurrentServer) ? ConnectionManager.KnownServers[ConnectionManager.CurrentServer] : 0;
			
			ConnectionManager.tcpClient.SendRequest(request);
		}

		public static void SendMessage(WRMC.Core.Networking.Message message) {
			if (message.Body is AuthenticatedMessageBody)
				(message.Body as AuthenticatedMessageBody).ClientDevice.SessionID = ConnectionManager.KnownServers.ContainsKey(ConnectionManager.CurrentServer) ? ConnectionManager.KnownServers[ConnectionManager.CurrentServer] : 0;

			ConnectionManager.tcpClient.SendMessage(message);
		}

		private static void UdpClient_OnServersResponseReceived(UdpClient sender, MessageBodyEventArgs<ServerResponseBody> e) {
			ConnectionManager.OnFindServerResponseReceived?.Invoke(null, e);
		}

		private static void UdpClient_OnFindServerFinished(UdpClient sender, EventArgs e) {
			ConnectionManager.IsSearchingServers = false;
			ConnectionManager.OnFindServerFinished?.Invoke(null, e);
		}

		private static void TcpClient_OnConnectionClosed(TcpClient sender, EventArgs e) {
			ConnectionManager.CurrentServer = null;
			ConnectionManager.OnConnectionClosed?.Invoke(null, e);
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

			ConnectionManager.KnownServers.MoveToEnd(ConnectionManager.pendingServerDevice);
			ConnectionManager.SaveKnownServers();

			ConnectionManager.CurrentServer = ConnectionManager.pendingServerDevice;
			ConnectionManager.OnConnectSuccess?.Invoke(null, EventArgs.Empty);
		}

		private static void TcpClient_OnMessageReceived(TcpClient sender, ServerMessageEventArgs e) {
			switch (e.Message.Method) {
				case WRMC.Core.Networking.Message.Type.MediaSessionChanged:
					ConnectionManager.OnMediaSessionChanged?.Invoke(null, new EventArgs<MediaSession>((e.Message.Body as MediaSessionChangedMessageBody).MediaSession));
					break;

				case WRMC.Core.Networking.Message.Type.MediaSessionListChanged:
					ConnectionManager.OnMediaSessionsReceived?.Invoke(null, new EventArgs<List<MediaSession>>((e.Message.Body as MediaSessionListChangedMessageBody).MediaSessions));
					break;
			}
		}

		private static void TcpClient_OnRequestReceived(TcpClient sender, ServerRequestEventArgs e) {
			
		}

		private static void TcpClient_OnResponseReceived(TcpClient sender, ServerResponseEventArgs e) {
			switch (e.Response.Method) {
				case Response.Type.AudioEndpoints:
					break;

				case Response.Type.ConnectSuccess:
					break;

				case Response.Type.DirectoryContent:
					break;

				case Response.Type.MediaSessions:
					ConnectionManager.OnMediaSessionsReceived?.Invoke(null, new EventArgs<List<MediaSession>>((e.Response.Body as MediaSessionsResponseBody).MediaSessions));
					break;

				case Response.Type.Screens:
					break;

				case Response.Type.Servers:
					break;

				case Response.Type.SuspendedProcesses:
					break;

				case Response.Type.Volume:
					break;
			}
		}

		private static void LoadKnownServers() {
			if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.ReadExternalStorage) == Permission.Granted) {
				try {
					if (File.Exists(Path.Combine(ConnectionManager.KNOWN_SERVERS_DIRECTORY_NAME, ConnectionManager.KNOWN_SERVERS_FILE_NAME))) {
						string json = File.ReadAllText(Path.Combine(ConnectionManager.KNOWN_SERVERS_DIRECTORY_NAME, ConnectionManager.KNOWN_SERVERS_FILE_NAME));
						ConnectionManager.KnownServers = JsonConvert.DeserializeObject<KeyedList<ServerDevice, long>>(json, SerializationOptions.DefaultSerializationOptions);
					} else
						ConnectionManager.KnownServers = new KeyedList<ServerDevice, long>();
				}
				catch (IOException e) {
					ConnectionManager.KnownServers = new KeyedList<ServerDevice, long>();
				}
			} else {
				ConnectionManager.KnownServers = new KeyedList<ServerDevice, long>();
			}
		}

		private static void SaveKnownServers() {
			if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
				return;

			try {
				if (!Directory.Exists(ConnectionManager.KNOWN_SERVERS_DIRECTORY_NAME))
					Directory.CreateDirectory(ConnectionManager.KNOWN_SERVERS_DIRECTORY_NAME);

				File.WriteAllText(Path.Combine(ConnectionManager.KNOWN_SERVERS_DIRECTORY_NAME, ConnectionManager.KNOWN_SERVERS_FILE_NAME), JsonConvert.SerializeObject(ConnectionManager.KnownServers, SerializationOptions.DefaultSerializationOptions));
			} catch (IOException e) { }
		}
	}
}