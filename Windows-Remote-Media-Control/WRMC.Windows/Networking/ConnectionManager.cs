using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using WRMC.Core.Models;
using WRMC.Core.Networking;
using WRMC.Windows.Media;

using Newtonsoft.Json;
using WRMC.Core;

namespace WRMC.Windows.Networking {
	public static class ConnectionManager {
		private const string KNOWN_CLIENTS_FILE_NAME = "clients.json";

		private static UdpServer udpServer;
		private static TcpServer tcpServer;

		public static List<ClientDevice> Clients { get; set; }
		public static Dictionary<string, long> KnownClients { get; set; }

		public static event EventHandler OnClientsChanged = null;
		public static event EventHandler<ClientRequestEventArgs> OnConnectionRequestReceived = null;

		static ConnectionManager() {
			ConnectionManager.Clients = new List<ClientDevice>();
			ConnectionManager.KnownClients = LoadKnownCLients();

			ConnectionManager.udpServer = new UdpServer();
			ConnectionManager.udpServer.OnFindServerRequestReceived += UdpServer_OnFindServerRequestReceived;
			ConnectionManager.udpServer.Start();

			ConnectionManager.tcpServer = new TcpServer();
			ConnectionManager.tcpServer.OnConnectionAccpeted += TcpServer_OnConnectionAccpeted;
			ConnectionManager.tcpServer.OnConnectionClosed += TcpServer_OnConnectionClosed;
			ConnectionManager.tcpServer.OnConnectRequestReceived += TcpServer_OnConnectRequestReceived;
			ConnectionManager.tcpServer.OnMessageReceived += TcpServer_OnMessageReceived;
			ConnectionManager.tcpServer.OnRequestReceived += TcpServer_OnRequestReceived;
			ConnectionManager.tcpServer.OnResponseReceived += TcpServer_OnResponseReceived;

			ConnectionManager.tcpServer.Start();
		}

		public static void AcceptConnection(System.Net.Sockets.TcpClient client, ClientDevice clientDevice) {
			if (ConnectionManager.KnownClients.ContainsKey(clientDevice.ID))
				if (clientDevice.SessionID == KnownClients[clientDevice.ID]) {
					ConnectionManager.tcpServer.AcceptConnection(client, clientDevice);
					return;
				}

			clientDevice.SessionID = ConnectionManager.GetRandomSessionID();
			ConnectionManager.tcpServer.AcceptConnection(client, clientDevice);
		}

		public static void CloseConnection(ClientDevice clientDevice) {
			ConnectionManager.tcpServer.CloseConnection(clientDevice);
		}

		private static long GetRandomSessionID() {
			Random r = new Random();
			while (true) {
				int i1 = r.Next(int.MaxValue);
				int i2 = r.Next(int.MaxValue);

				long l = ((long)i1 << 32) | (uint)i2;

				if (!KnownClients.ContainsValue(l))
					return l;
			}
		}

		public static void SendMediaSessionsChanged(List<MediaSession> mediaSessions) {
			Message message = new Message() {
				Method = Message.Type.MediaSessionListChanged,
				Body = new MediaSessionListChangedMessageBody() {
					MediaSessions = mediaSessions
				}
			};

			foreach (var client in ConnectionManager.Clients)
				ConnectionManager.tcpServer.SendMessage(message, client);
		}

		public static void SendMediaSessionChanged(MediaSession mediaSession) {
			Message message = new Message() {
				Method = Message.Type.MediaSessionChanged,
				Body = new MediaSessionChangedMessageBody() {
					MediaSession = mediaSession
				}
			};

			foreach (var client in ConnectionManager.Clients)
				ConnectionManager.tcpServer.SendMessage(message, client);
		}

		private static void UdpServer_OnFindServerRequestReceived(UdpServer sender, EventArgs<ClientDevice> e) {
			sender.SendServerInformation(DeviceInformation.ServerDevice, e.Data);
		}

		private static void TcpServer_OnConnectionAccpeted(TcpServer sender, ClientEventArgs e) {
			if (KnownClients.ContainsKey(e.ClientDevice.ID)) {
				if (KnownClients[e.ClientDevice.ID] != e.ClientDevice.SessionID)
					KnownClients[e.ClientDevice.ID] = e.ClientDevice.SessionID;
			} else {
				KnownClients.Add(e.ClientDevice.ID, e.ClientDevice.SessionID);
			}

			ConnectionManager.SaveKnownClients();

			if (Clients.Contains(e.ClientDevice))
				return;

			Clients.Add(e.ClientDevice);
			ConnectionManager.OnClientsChanged?.Invoke(null, EventArgs.Empty);
		}

		private static void TcpServer_OnConnectionClosed(TcpServer sender, ClientEventArgs e) {
			if (!Clients.Contains(e.ClientDevice))
				return;

			Clients.Remove(e.ClientDevice);
			ConnectionManager.OnClientsChanged?.Invoke(null, EventArgs.Empty);
		}

		private static void TcpServer_OnConnectRequestReceived(TcpServer sender, ClientRequestEventArgs e) {
			ConnectionManager.OnConnectionRequestReceived?.Invoke(null, e);
		}

		private static void TcpServer_OnMessageReceived(TcpServer sender, ClientMessageEventArgs e) {
			ClientDevice device;

			switch (e.Message.Method) {
				case Message.Type.Disconnect:
					AuthenticatedMessageBody disconnectAuthenticatedMessageBody = e.Message.Body as AuthenticatedMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(disconnectAuthenticatedMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != disconnectAuthenticatedMessageBody.ClientDevice.SessionID)
						break;

					ConnectionManager.tcpServer.CloseConnection(e.Client, disconnectAuthenticatedMessageBody.ClientDevice, false);
					Clients.Remove(disconnectAuthenticatedMessageBody.ClientDevice);
					break;

				case Message.Type.NextMedia:
					MediaSessionMessageBody nextMediaSessionMessageBody = e.Message.Body as MediaSessionMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(nextMediaSessionMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != nextMediaSessionMessageBody.ClientDevice.SessionID)
						break;

					MediaCommandInvoker.Default.SkipNext(nextMediaSessionMessageBody.MediaSession);
					break;

				case Message.Type.PlayFile:
					PlayFileMessageBody playFileMediaSessionMessageBody = e.Message.Body as PlayFileMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(playFileMediaSessionMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != playFileMediaSessionMessageBody.ClientDevice.SessionID)
						break;

					MediaCommandInvoker.Default.PlayFile(playFileMediaSessionMessageBody.FilePath);
					break;

				case Message.Type.PlayPause:
					MediaSessionMessageBody playPauseFileMediaSessionMessageBody = e.Message.Body as MediaSessionMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(playPauseFileMediaSessionMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != playPauseFileMediaSessionMessageBody.ClientDevice.SessionID)
						break;

					if (playPauseFileMediaSessionMessageBody.MediaSession.State == Core.Models.MediaSession.PlaybackState.Playing)
						MediaCommandInvoker.Default.Pause(playPauseFileMediaSessionMessageBody.MediaSession);
					else
						MediaCommandInvoker.Default.Play(playPauseFileMediaSessionMessageBody.MediaSession);

					break;

				case Message.Type.PreviousMedia:
					MediaSessionMessageBody previousFileMediaSessionMessageBody = e.Message.Body as MediaSessionMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(previousFileMediaSessionMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != previousFileMediaSessionMessageBody.ClientDevice.SessionID)
						break;

					MediaCommandInvoker.Default.SkipPrevious(previousFileMediaSessionMessageBody.MediaSession);
					break;

				case Message.Type.ResumeSuspendedProcess:
					ResumeSuspendedProcessMessageBody resumeProcessMessageBody = e.Message.Body as ResumeSuspendedProcessMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(resumeProcessMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != resumeProcessMessageBody.ClientDevice.SessionID)
						break;

					MediaCommandInvoker.Default.ResumeSuspendedProcess(resumeProcessMessageBody.Process);

					break;

				case Message.Type.SetAudioEndpoint:
					SetAudioEndpointMessageBody audioEndpointMessageBody = e.Message.Body as SetAudioEndpointMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(audioEndpointMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != audioEndpointMessageBody.ClientDevice.SessionID)
						break;

					MediaCommandInvoker.Default.SetAudioEndpoint(audioEndpointMessageBody.MediaSession, audioEndpointMessageBody.AudioEndpoint);
					break;

				case Message.Type.SetConfiguration:
					SetConfigurationMessageBody configurationMessageBody = e.Message.Body as SetConfigurationMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(configurationMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != configurationMessageBody.ClientDevice.SessionID)
						break;

					MediaCommandInvoker.Default.SetConfiguration(configurationMessageBody.MediaSession, configurationMessageBody.Configuration);
					break;

				case Message.Type.SetScreen:
					SetScreenMessageBody screenMessageBody = e.Message.Body as SetScreenMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(screenMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != screenMessageBody.ClientDevice.SessionID)
						break;

					MediaCommandInvoker.Default.MoveToScreen(screenMessageBody.MediaSession, screenMessageBody.Screen);
					break; 

				case Message.Type.SetVolume:
					SetVolumeMessageBody volumeMessageBody = e.Message.Body as SetVolumeMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(volumeMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != volumeMessageBody.ClientDevice.SessionID)
						break;

					MediaCommandInvoker.Default.SetMasterVolume(volumeMessageBody.Volume);
					break;
			}
		}

		private static void TcpServer_OnRequestReceived(TcpServer sender, ClientRequestEventArgs e) {
			ClientDevice device;

			switch (e.Request.Method) {
				case Request.Type.GetAudioEndpoints:
					AuthenticatedMessageBody audioEndpointsAuthenticatedMessageBody = e.Request.Body as AuthenticatedMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(audioEndpointsAuthenticatedMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != audioEndpointsAuthenticatedMessageBody.ClientDevice.SessionID)
						break;

					Response audoEndpointsResponse = new Response() {
						Method = Response.Type.AudioEndpoints,
						Body = new AudioEndpointsResponseBody() {
							AudioEndpoints = MediaCommandInvoker.Default.GetAudioEndpoints()
						}
					};

					ConnectionManager.tcpServer.SendResponse(audoEndpointsResponse, e.ClientDevice);

					break;

				case Request.Type.GetDirectoryContent:
					DirectoryContentRequestBody directoryAuthenticatedMessageBody = e.Request.Body as DirectoryContentRequestBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(directoryAuthenticatedMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != directoryAuthenticatedMessageBody.ClientDevice.SessionID)
						break;

					List<DirectoryItem> directoryContent = MediaCommandInvoker.Default.GetDirectoryContent(directoryAuthenticatedMessageBody.Directory);

					Response directoryResponse = new Response() {
						Method = Response.Type.DirectoryContent,
						Body = new DirectoryContentResponseBody() {
							Items = directoryContent
						}
					};

					ConnectionManager.tcpServer.SendResponse(directoryResponse, e.ClientDevice);

					break;

				case Request.Type.GetMediaSessions:
					AuthenticatedMessageBody mediaSessionsAuthenticatedMessageBody = e.Request.Body as AuthenticatedMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(mediaSessionsAuthenticatedMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != mediaSessionsAuthenticatedMessageBody.ClientDevice.SessionID)
						break;

					Response mediaSessionsResponse = new Response() {
						Method = Response.Type.MediaSessions,
						Body = new MediaSessionsResponseBody() {
							MediaSessions = MediaSessionExtractor.Default.Sessions
						}
					};

					ConnectionManager.tcpServer.SendResponse(mediaSessionsResponse, e.ClientDevice);

					break;

				case Request.Type.GetScreens:
					AuthenticatedMessageBody screensAuthenticatedMessageBody = e.Request.Body as AuthenticatedMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(screensAuthenticatedMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != screensAuthenticatedMessageBody.ClientDevice.SessionID)
						break;

					Response screensResponse = new Response() {
						Method = Response.Type.Screens,
						Body = new ScreensResponseBody() {
							Screens = MediaCommandInvoker.Default.GetScreens()
						}
					};

					ConnectionManager.tcpServer.SendResponse(screensResponse, e.ClientDevice);

					break;

				case Request.Type.GetSuspendedMediaProcesses:
					AuthenticatedMessageBody processAuthenticatedMessageBody = e.Request.Body as AuthenticatedMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(processAuthenticatedMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != processAuthenticatedMessageBody.ClientDevice.SessionID)
						break;

					Response processResponse = new Response() {
						Method = Response.Type.SuspendedProcesses,
						Body = new SuspendedProcessesResponseBody() {
							Processes = MediaCommandInvoker.Default.GetSuspendedProcesses()
						}
					};

					ConnectionManager.tcpServer.SendResponse(processResponse, e.ClientDevice);

					break;

				case Request.Type.GetThumbnail:
					MediaSessionMessageBody thumbnailMediaSessionMessageBody = e.Request.Body as MediaSessionMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(thumbnailMediaSessionMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != thumbnailMediaSessionMessageBody.ClientDevice.SessionID)
						break;

					byte[] bytes = MediaCommandInvoker.Default.GetThumbnail(thumbnailMediaSessionMessageBody.MediaSession);

					Response thumbnailResponse = new Response() {
						Method = Response.Type.Thumbnail,
						Body = new ThumbnailResponseBody() {
							Bytes = bytes
						}
					};

					ConnectionManager.tcpServer.SendResponse(thumbnailResponse, e.ClientDevice);

					break;

				case Request.Type.GetVolume:
					AuthenticatedMessageBody volumeAuthenticatedMessageBody = e.Request.Body as AuthenticatedMessageBody;

					device = ConnectionManager.Clients.Where(d => d.Equals(volumeAuthenticatedMessageBody.ClientDevice)).FirstOrDefault();

					if (device == null || device.SessionID != volumeAuthenticatedMessageBody.ClientDevice.SessionID)
						break;

					Response volumeResponse = new Response() {
						Method = Response.Type.Volume,
						Body = new VolumeResponseBody() {
							Volume = MediaCommandInvoker.Default.GetMasterVolume()
						}
					};

					ConnectionManager.tcpServer.SendResponse(volumeResponse, e.ClientDevice);

					break;
			}
		}

		private static void TcpServer_OnResponseReceived(TcpServer sender, ClientResponseEventArgs e) {

		}

		private static Dictionary<string, long> LoadKnownCLients() {
			return File.Exists(KNOWN_CLIENTS_FILE_NAME) ? JsonConvert.DeserializeObject(File.ReadAllText(KNOWN_CLIENTS_FILE_NAME), SerializationOptions.DefaultSerializationOptions) as Dictionary<string, long> : new Dictionary<string, long>();
		}

		private static void SaveKnownClients() {
			File.WriteAllText(KNOWN_CLIENTS_FILE_NAME, JsonConvert.SerializeObject(ConnectionManager.KnownClients, SerializationOptions.DefaultSerializationOptions));
		}
	}
}