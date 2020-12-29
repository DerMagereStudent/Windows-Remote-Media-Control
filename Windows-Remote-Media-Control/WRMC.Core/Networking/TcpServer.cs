﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the Windows-/server-side part of a tcp connection.
	/// </summary>
	public class TcpServer {
		private TcpListener server;

		private Dictionary<System.Net.Sockets.TcpClient, ClientDevice> clients;
		private Dictionary<System.Net.Sockets.TcpClient, byte[]> clientBuffers;

		private object clientsLock = new object();
		private object clientBuffersLock = new object();

		/// <summary>
		/// Indicates if the server is currently listen
		/// </summary>
		public bool Listening { get; protected set; }

		/// <summary>
		/// Event which is called when the server receives a connect request. The receiver of the event has to call <see cref="TcpServer.AcceptConnection(TcpClient, ClientDevice)"/> to allow the client to send messages.
		/// </summary>
		public event TypedEventHandler<TcpServer, ClientRequestEventArgs> OnConnectRequestReceived = null;

		/// <summary>
		/// Called when a new connection was accepted.
		/// </summary>
		public event TypedEventHandler<TcpServer, ClientEventArgs> OnConnectionAccpeted = null;

		/// <summary>
		/// Called when a connection was intentionally closed or when the connection monitoring noticed a connection is not longer valid.
		/// </summary>
		public event TypedEventHandler<TcpServer, ClientEventArgs> OnConnectionClosed = null;

		/// <summary>
		/// Event which is called when the server receives a message from a verified client.
		/// </summary>
		public event TypedEventHandler<TcpServer, ClientMessageEventArgs> OnMessageReceived = null;

		/// <summary>
		/// Event which is called when the server receives a request from a verified client.
		/// </summary>
		public event TypedEventHandler<TcpServer, ClientRequestEventArgs> OnRequestReceived = null;

		/// <summary>
		/// Event which is called when the server receives a response from a verified client.
		/// </summary>
		public event TypedEventHandler<TcpServer, ClientResponseEventArgs> OnResponseReceived = null;

		/// <summary>
		/// Creates a new object of the class <see cref="TcpServer"/>
		/// </summary>
		public TcpServer() {
			this.server = new TcpListener(TcpOptions.DefaultIPAddress, TcpOptions.DefaultPort);
			this.clients = new Dictionary<TcpClient, ClientDevice>();
			this.clientBuffers = new Dictionary<TcpClient, byte[]>();
			Task.Factory.StartNew(() => this.ClientConnectionMonitoring());
		}

		/// <summary>
		/// Starts listening to connection requests.
		/// </summary>
		public void Start() {
			if (this.Listening)
				return;

			this.server.Start();
			this.server.Server.Listen(1);
			this.server.BeginAcceptTcpClient(this.OnConnectionRequestReceived, null);
			this.Listening = true;
		}

		/// <summary>
		/// Stops listening to connection requests.
		/// </summary>
		public void Stop() {
			if (this.Listening) {
				this.server.Stop();
				this.Listening = false;
			}
		}

		/// <summary>
		/// Closes all connections and stops listening for new connection requests.
		/// </summary>
		public void Close() {
			lock (this.clientsLock) {
				foreach (System.Net.Sockets.TcpClient client in this.clients.Keys)
					this.CloseConnection(client);
			}

			this.Stop();
		}

		/// <summary>
		/// Closes the connection to a specified client device.
		/// </summary>
		public void Close(ClientDevice clientDevice) {
			lock (this.clientsLock) {
				foreach (KeyValuePair<System.Net.Sockets.TcpClient, ClientDevice> client in this.clients)
					if (client.Value.Equals(clientDevice))
						this.CloseConnection(client.Key);
			}
		}

		/// <summary>
		/// Accepts the specified connection. The server will from now on raise events when receiving messages, requests or responses sent by the specified client.
		/// </summary>
		/// <param name="client">The TCP client which should be accepted.</param>
		/// <param name="clientDevice">The authentication informationen which is send to the client. Should be used for reconnection.</param>
		public void AcceptConnection(System.Net.Sockets.TcpClient client, ClientDevice clientDevice) {
			lock (this.clientsLock) {
				if (!this.clients.ContainsKey(client))
					return;

				this.clients[client] = clientDevice;
			}

			Response response = new Response() {
				Method = Response.Type.ConnectSuccess,
				Body = new AuthenticatedMessageBody() {
					ClientDevice = clientDevice
				}
			};

			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, SerializationOptions.DefaultMessageSerializationOptions));

			client.GetStream().BeginWrite(data, 0, data.Length, this.OnWrite, client);
		}

		/// <summary>
		/// Sends a message to a specified client.
		/// </summary>
		/// <param name="message">The message to send.</param>
		/// <param name="receiver">The receiver client of the message.</param>
		public void SendMessage(Message message, ClientDevice receiver) {
			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, SerializationOptions.DefaultMessageSerializationOptions));
			this.SendData(data, receiver);
		}

		/// <summary>
		/// Sends a request to a specified client.
		/// </summary>
		/// <param name="request">The request to send.</param>
		/// <param name="receiver">The receiver client of the message.</param>
		public void SendRequest(Request request, ClientDevice receiver) {
			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request, SerializationOptions.DefaultMessageSerializationOptions));
			this.SendData(data, receiver);
		}

		/// <summary>
		/// Sends a response to a specified client.
		/// </summary>
		/// <param name="response">The response to send.</param>
		/// <param name="receiver">The receiver client of the response.</param>
		public void SendRequest(Response response, ClientDevice receiver) {
			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, SerializationOptions.DefaultMessageSerializationOptions));
			this.SendData(data, receiver);
		}

		/// <summary>
		/// Sends an amount of data to a specified client.
		/// </summary>
		/// <param name="data">The data to send.</param>
		/// <param name="receiver">The receiver client of the data.</param>
		public void SendData(byte[] data, ClientDevice receiver) {
			try {
				TcpClient receiverClient = null;

				lock (this.clientsLock) {
					foreach (KeyValuePair<TcpClient, ClientDevice> client in this.clients)
						if (client.Value.Equals(receiver)) {
							receiverClient = client.Key;
							break;
						}
				}

				if (receiverClient == null)
					return;

				receiverClient.GetStream().BeginWrite(data, 0, data.Length, this.OnWrite, receiverClient);
			} catch(ObjectDisposedException) {
				// Connection closed
			}
		}

		/// <summary>
		/// Callback called when writing to a TCP connection network stream.
		/// </summary>
		/// <param name="ar"></param>
		private void OnWrite(IAsyncResult ar) {
			try {
				System.Net.Sockets.TcpClient client = ar.AsyncState as System.Net.Sockets.TcpClient;
				client.GetStream().EndWrite(ar);
			} catch(ObjectDisposedException) {
				// Connection closed
			}
		}

		/// <summary>
		/// Callback called when receiving a connection request.
		/// </summary>
		/// <param name="ar"></param>
		private void OnConnectionRequestReceived(IAsyncResult ar) {
			try {
				System.Net.Sockets.TcpClient client = this.server.EndAcceptTcpClient(ar);
				lock (this.clientsLock)
					this.clients.Add(client, null);
				
				lock (this.clientBuffersLock) {
					this.clientBuffers.Add(client, new byte[1024]);
					client.GetStream().BeginRead(this.clientBuffers[client], 0, this.clientBuffers[client].Length, this.OnDataReceived, client);
				}

				this.server.BeginAcceptTcpClient(this.OnConnectionRequestReceived, null);
			} catch (ObjectDisposedException) {
				// Connection closed or TcpListener stopped
			}
		}

		/// <summary>
		/// Callback called when receiving data from a TCP connection netowrk stream.
		/// </summary>
		/// <param name="ar"></param>
		private void OnDataReceived(IAsyncResult ar) {
			try {
				System.Net.Sockets.TcpClient client = ar.AsyncState as System.Net.Sockets.TcpClient;
				int received = client.GetStream().EndRead(ar);

				lock (this.clientsLock) {
					if (!this.clients.ContainsKey(client))
						return;
				}
				

				string dataString;
				
				lock (this.clientBuffersLock)
					dataString = Encoding.UTF8.GetString(this.clientBuffers[client], 0, received);
				
				object dataObject = JsonConvert.DeserializeObject(dataString, SerializationOptions.DefaultMessageSerializationOptions);

				bool condition;

				lock (this.clientsLock)
					condition = this.clients[client] == null;

				if (condition) {
					if (dataObject is Request) {
						Request request = dataObject as Request;

						if (request.Method == Request.Type.Connect)
							this.OnConnectRequestReceived?.Invoke(this, new ClientRequestEventArgs(client, null, request));
					}
				}
				else {
					if (dataObject is Message)
						this.OnMessageReceived?.Invoke(this, new ClientMessageEventArgs(client, this.clients[client], dataObject as Message));
					else if (dataObject is Request)
						this.OnRequestReceived?.Invoke(this, new ClientRequestEventArgs(client, this.clients[client], dataObject as Request));
					else if (dataObject is Response)
						this.OnResponseReceived?.Invoke(this, new ClientResponseEventArgs(client, this.clients[client], dataObject as Response));
				}

				lock (this.clientBuffersLock)
					client.GetStream().BeginRead(this.clientBuffers[client], 0, this.clientBuffers[client].Length, this.OnDataReceived, client);
			} catch(ObjectDisposedException) {
				// Connection closed
			}
		}

		/// <summary>
		/// Method which is running for the whole lifetime of the TCP server to monitor the connection status of all knows clients.
		/// </summary>
		private void ClientConnectionMonitoring() {
			while (true) {
				lock (this.clientsLock) {
					foreach (System.Net.Sockets.TcpClient client in this.clients.Keys)
						if (!client.IsConnected())
							this.CloseConnection(client);
				}
				Thread.Sleep(1000);
			}
		}


		private void CloseConnection(System.Net.Sockets.TcpClient client) {
			try { client.GetStream().Close(); } catch (ObjectDisposedException) { }
			try { client.Close(); } catch (ObjectDisposedException) { }

			ClientDevice cd;

			lock (this.clientsLock) {
				cd = this.clients[client];
				this.clients.Remove(client);
			}

			this.OnConnectionClosed?.Invoke(this, new ClientEventArgs(client, cd));
		}
	}
}