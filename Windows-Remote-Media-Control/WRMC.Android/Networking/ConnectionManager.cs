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

		/// <summary>
		/// List containing all known servers and the session ids the client can use to authenticate.
		/// </summary>
		[JsonConverter(typeof(KeyedListJsonConverter<ServerDevice, long>))]
		public static KeyedList<ServerDevice, long> KnownServers { get; set; }

		/// <summary>
		/// Boolean taht indicates if the client is currently searching servers by sending <see cref="Request.Type.FindServer"/> requests.
		/// </summary>
		public static bool IsSearchingServers { get; private set; }

		/// <summary>
		/// The server information of the server the client is currently connected to. Null if no connection.
		/// </summary>
		public static ServerDevice CurrentServer { get; set; }

		/// <summary>
		/// Event handler that is called whenever the client receives a <see cref="Response.Type.Servers"/> response.
		/// </summary>
		public static event EventHandler<MessageBodyEventArgs<ServerResponseBody>> OnFindServerResponseReceived = null;

		/// <summary>
		/// Event handler that is called when the find server process is finished sending packages.
		/// </summary>
		public static event EventHandler<EventArgs> OnFindServerFinished = null;

		/// <summary>
		/// Event handler that is called when the client established a TCP connection.
		/// </summary>
		public static event EventHandler<EventArgs> OnTcpConnectSuccess = null;

		/// <summary>
		/// Event handler that is called when the client failed to establish a TCP connection.
		/// </summary>
		public static event EventHandler<EventArgs> OnTcpConnectFailure = null;

		/// <summary>
		/// Event handler that is called when the client receives a <see cref="Response.Type.ConnectSuccess"/> response.
		/// </summary>
		public static event EventHandler<EventArgs> OnConnectSuccess = null;

		/// <summary>
		/// Event handler that should be called when the server rejects the connect request of when client gets no response. Currenly not implemented.
		/// </summary>
		public static event EventHandler<EventArgs> OnConnectFailure = null;

		/// <summary>
		/// Event handler that is called when the server closed the connection or when connection monitoring process recognizes the server does not responde to Pings.
		/// </summary>
		public static event EventHandler<EventArgs> OnConnectionClosed = null;

		/// <summary>
		/// Event handler that is called when the client receives screen information.
		/// </summary>
		public static event EventHandler<EventArgs<List<string>>> OnScreensReceived = null;

		/// <summary>
		/// Event handler that is called when the client receives audio device information.
		/// </summary>
		public static event EventHandler<EventArgs<List<AudioEndpoint>>> OnAudioDevicesReceived = null;

		/// <summary>
		/// Event handler that is called when the client receives volume information.
		/// </summary>
		public static event EventHandler<EventArgs<int>> OnVolumeReceived = null;

		/// <summary>
		/// Event handler that is called when the client receives the thumbnail bytes.
		/// </summary>
		public static event EventHandler<EventArgs<byte[]>> OnThumbnailReceived = null;

		/// <summary>
		/// Event handler that is called when the client receives the list of suspended processes.
		/// </summary>
		public static event EventHandler<EventArgs<List<SuspendedProcess>>> OnSuspendedProcessesReceived = null;

		/// <summary>
		/// Event handler that is called when the client receives the content of a specific directory.
		/// </summary>
		public static event EventHandler<EventArgs<List<DirectoryItem>>> OnDirectoryContentReceived = null;

		/// <summary>
		/// Event handler that is called when the client receives the list of media sessions active on the server device.
		/// </summary>
		public static event EventHandler<EventArgs<List<MediaSession>>> OnMediaSessionsReceived = null;

		/// <summary>
		/// Event handler that is called when the client receives information about a specific media session.
		/// </summary>
		public static event EventHandler<EventArgs<MediaSession>> OnMediaSessionChanged = null;

		/// <summary>
		/// Static contructor called by the CLI before using static fields.
		/// </summary>
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

		/// <summary>
		/// Starts the find server process by sending UDP <see cref="Request.Type.FindServer"/> requests.
		/// </summary>
		public static void StartFindServer() {
			if (ConnectionManager.IsSearchingServers)
				return;

			ConnectionManager.udpClient.StartListening();
			ConnectionManager.udpClient.StartSending(50, 10000, DeviceInformation.GetClientDevice(Application.Context));
			ConnectionManager.IsSearchingServers = true;
		}

		/// <summary>
		/// Starts the find server process.
		/// </summary>
		public static void StopFindServer() {
			if (!ConnectionManager.IsSearchingServers)
				return;

			ConnectionManager.udpClient.StopListening();
			ConnectionManager.udpClient.StopSending();
			ConnectionManager.IsSearchingServers = false;
		}

		/// <summary>
		/// Starts to try to establish a TCP connection to a given server device.
		/// </summary>
		/// <param name="device">The server to connect to.</param>
		/// <returns>True if the try was successful and a connection was established.</returns>
		public static bool StartConnect(ServerDevice device) {
			ConnectionManager.pendingServerDevice = device;
			return ConnectionManager.tcpClient.Start(device);
		}

		public static void StopConnect() {
			ConnectionManager.tcpClient.Stop();
		}

		/// <summary>
		/// Closes the connection and sends a <see cref="WRMC.Core.Networking.Message.Type.Disconnect"/> to the server.
		/// </summary>
		/// <param name="clientDevice"></param>
		public static void CloseConnection(ClientDevice clientDevice) {
			clientDevice.SessionID = ConnectionManager.KnownServers.ContainsKey(ConnectionManager.CurrentServer) ? ConnectionManager.KnownServers[ConnectionManager.CurrentServer] : 0;
			ConnectionManager.tcpClient.Stop(clientDevice);
		}

		/// <summary>
		/// Sends a <see cref="Request.Type.Connect"/> request to the current server.
		/// </summary>
		/// <param name="clientDevice">The client information used to authenticate.</param>
		public static void SendConnectRequest(ClientDevice clientDevice) {
			clientDevice.SessionID = ConnectionManager.KnownServers.ContainsKey(ConnectionManager.pendingServerDevice) ? ConnectionManager.KnownServers[ConnectionManager.pendingServerDevice] : 0;

			ConnectionManager.tcpClient.SendRequest(new Request() {
				Method = Request.Type.Connect,
				Body = new AuthenticatedMessageBody() {
					ClientDevice = clientDevice
				}
			});
		}

		/// <summary>
		/// Sends a request to the server.
		/// </summary>
		/// <param name="request">The request to send.</param>
		public static void SendRequest(Request request) {
			if (request.Body is AuthenticatedMessageBody)
				(request.Body as AuthenticatedMessageBody).ClientDevice.SessionID = ConnectionManager.KnownServers.ContainsKey(ConnectionManager.CurrentServer) ? ConnectionManager.KnownServers[ConnectionManager.CurrentServer] : 0;
			
			ConnectionManager.tcpClient.SendRequest(request);
		}

		/// <summary>
		/// Sends a message to the server.
		/// </summary>
		/// <param name="message">The message to send.</param>
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
				case WRMC.Core.Networking.Message.Type.Disconnect:
					ConnectionManager.CurrentServer = null;
					ConnectionManager.OnConnectionClosed?.Invoke(null, EventArgs.Empty);
					break;
				case WRMC.Core.Networking.Message.Type.MediaSessionChanged:
					ConnectionManager.OnMediaSessionChanged?.Invoke(null, (e.Message.Body as MediaSessionChangedMessageBody).MediaSession);
					break;
				case WRMC.Core.Networking.Message.Type.MediaSessionListChanged:
					ConnectionManager.OnMediaSessionsReceived?.Invoke(null, (e.Message.Body as MediaSessionListChangedMessageBody).MediaSessions);
					break;
			}
		}

		private static void TcpClient_OnRequestReceived(TcpClient sender, ServerRequestEventArgs e) {
			
		}

		private static void TcpClient_OnResponseReceived(TcpClient sender, ServerResponseEventArgs e) {
			switch (e.Response.Method) {
				case Response.Type.AudioEndpoints:
					ConnectionManager.OnAudioDevicesReceived?.Invoke(null, (e.Response.Body as AudioEndpointsResponseBody).AudioEndpoints);
					break;

				case Response.Type.DirectoryContent:
					DirectoryContentResponseBody responseBody = (e.Response.Body as DirectoryContentResponseBody);
					ConnectionManager.OnDirectoryContentReceived?.Invoke(null, responseBody.Items);
					break;

				case Response.Type.MediaSessions:
					ConnectionManager.OnMediaSessionsReceived?.Invoke(null, (e.Response.Body as MediaSessionsResponseBody).MediaSessions);
					break;

				case Response.Type.Screens:
					ConnectionManager.OnScreensReceived?.Invoke(null, (e.Response.Body as ScreensResponseBody).Screens);
					break;

				case Response.Type.SuspendedProcesses:
					ConnectionManager.OnSuspendedProcessesReceived?.Invoke(null, (e.Response.Body as SuspendedProcessesResponseBody).Processes);
					break;

				case Response.Type.Thumbnail:
					ConnectionManager.OnThumbnailReceived?.Invoke(null, (e.Response.Body as ThumbnailResponseBody).Bytes);
					break;

				case Response.Type.Volume:
					ConnectionManager.OnVolumeReceived?.Invoke(null, (e.Response.Body as VolumeResponseBody).Volume);
					break;
			}
		}

		/// <summary>
		/// Loads the known server list from the servers json file.
		/// </summary>
		private static void LoadKnownServers() {
			if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.ReadExternalStorage) == Permission.Granted) {
				try {
					if (File.Exists(Path.Combine(ConnectionManager.KNOWN_SERVERS_DIRECTORY_NAME, ConnectionManager.KNOWN_SERVERS_FILE_NAME))) {
						string json = File.ReadAllText(Path.Combine(ConnectionManager.KNOWN_SERVERS_DIRECTORY_NAME, ConnectionManager.KNOWN_SERVERS_FILE_NAME));
						ConnectionManager.KnownServers = JsonConvert.DeserializeObject<KeyedList<ServerDevice, long>>(json, SerializationOptions.DefaultSerializationOptions);

						if (ConnectionManager.KnownServers == null)
							ConnectionManager.KnownServers = new KeyedList<ServerDevice, long>();
					} else
						ConnectionManager.KnownServers = new KeyedList<ServerDevice, long>();
				}
				catch (IOException) {
					ConnectionManager.KnownServers = new KeyedList<ServerDevice, long>();
				}
			} else {
				ConnectionManager.KnownServers = new KeyedList<ServerDevice, long>();
			}
		}

		/// <summary>
		/// Saves the known server list to the servers json file.
		/// </summary>
		private static void SaveKnownServers() {
			if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
				return;

			try {
				if (!Directory.Exists(ConnectionManager.KNOWN_SERVERS_DIRECTORY_NAME))
					Directory.CreateDirectory(ConnectionManager.KNOWN_SERVERS_DIRECTORY_NAME);

				File.WriteAllText(Path.Combine(ConnectionManager.KNOWN_SERVERS_DIRECTORY_NAME, ConnectionManager.KNOWN_SERVERS_FILE_NAME), JsonConvert.SerializeObject(ConnectionManager.KnownServers, SerializationOptions.DefaultSerializationOptions));
			} catch (IOException) { }
		}
	}
}