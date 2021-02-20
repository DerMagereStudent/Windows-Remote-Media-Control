using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the Android-/client-side part of a tcp connection.
	/// </summary>
	public class TcpClient {
		private System.Net.Sockets.TcpClient client;
		private ServerDevice serverDevice;
		private int pingTries = 0;

		private byte[] buffer;
		private string remainingData;

		private object clientLock = new object();

		/// <summary>
		/// Event which is called when the client could establish a TCP connection to a remote server.
		/// </summary>
		public event TypedEventHandler<TcpClient, ServerEventArgs> OnTcpConnectSuccess = null;

		/// <summary>
		/// Event which is called when the client could not establish a TCP connection to a remote server.
		/// </summary>
		public event TypedEventHandler<TcpClient, ServerEventArgs> OnTcpConnectFailure = null;

		/// <summary>
		/// Event which is called when the client receives a <see cref="Response.Type.ConnectSuccess"/> response from a remote server.
		/// </summary>
		public event TypedEventHandler<TcpClient, ClientEventArgs> OnConnectSuccess = null;

		/// <summary>
		/// Event which is called when connection of the underlying <see cref="System.Net.Sockets.TcpClient"/> was closed.
		/// </summary>
		public event TypedEventHandler<TcpClient, EventArgs> OnConnectionClosed = null;

		/// <summary>
		/// Event which is called when the client receives a message from a remote server.
		/// </summary>
		public event TypedEventHandler<TcpClient, ServerMessageEventArgs> OnMessageReceived = null;

		/// <summary>
		/// Event which is called when the client receives a request from a remote server.
		/// </summary>
		public event TypedEventHandler<TcpClient, ServerRequestEventArgs> OnRequestReceived = null;

		/// <summary>
		/// Event which is called when the client receives a response from a remote server.
		/// </summary>
		public event TypedEventHandler<TcpClient, ServerResponseEventArgs> OnResponseReceived = null;

		/// <summary>
		/// Creates a new object of the class <see cref="TcpClient"/>
		/// </summary>
		public TcpClient() {
			this.client = new System.Net.Sockets.TcpClient();
			this.buffer = new byte[TcpOptions.BufferSize];
			this.remainingData = "";

			Task.Factory.StartNew(() => this.ClientConnectionMonitoring());
		}

		/// <summary>
		/// Connects to a given server. Closes the previous connection.
		/// </summary>
		/// <param name="serverDevice">The server to connect to.</param>
		public bool Start(ServerDevice serverDevice) {
			lock (this.clientLock)
				if (this.serverDevice != null && this.serverDevice.Equals(serverDevice) && this.client.IsConnected())
					return false;

			this.Stop();

			lock (this.clientLock)
				try {
					this.client.BeginConnect(serverDevice.IPAddress, TcpOptions.DefaultPort, this.OnConnect, new Tuple<ServerDevice, int>(serverDevice, 0));
					return true;
				} catch (SocketException) { return false; }
		}

		/// <summary>
		/// Closes the current connection.
		/// </summary>
		public void Stop() {
			lock (this.clientLock) {
				try { this.client.GetStream().Close(); } catch (ObjectDisposedException) { } catch (InvalidOperationException) { } catch (IOException) { }
				try { this.client.Close(); } catch (ObjectDisposedException) { } catch (InvalidOperationException) { } catch (IOException) { }
				this.client = new System.Net.Sockets.TcpClient();
			}

			this.serverDevice = null;

			this.OnConnectionClosed?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Closes the current connection.
		/// </summary>
		public void Stop(ClientDevice clientDevice) {
			try {
				Message message = new Message() {
					Method = Message.Type.Disconnect,
					Body = new AuthenticatedMessageBody() {
						ClientDevice = clientDevice
					}
				};

				byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, SerializationOptions.DefaultSerializationOptions) + TcpOptions.MessageSeparator);
				this.client.GetStream().Write(buffer, 0, buffer.Length);
				this.client.GetStream().Flush();
			}
			catch (ObjectDisposedException) { }
			catch (InvalidOperationException) { }
			catch (IOException) { }

			this.Stop();
		}

		/// <summary>
		/// Sends a message to the connected server.
		/// </summary>
		/// <param name="message">The message to send.</param>
		public void SendMessage(Message message) {
			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, SerializationOptions.DefaultSerializationOptions) + TcpOptions.MessageSeparator);
			this.SendData(data);
		}

		/// <summary>
		/// Sends a request to the connected server.
		/// </summary>
		/// <param name="request">The request to send.</param>
		public void SendRequest(Request request) {
			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request, SerializationOptions.DefaultSerializationOptions) + TcpOptions.MessageSeparator);
			this.SendData(data);
		}

		/// <summary>
		/// Sends a response to the connected server.
		/// </summary>
		/// <param name="response">The response to send.</param>
		public void SendResponse(Response response) {
			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, SerializationOptions.DefaultSerializationOptions) + TcpOptions.MessageSeparator);
			this.SendData(data);
		}

		/// <summary>
		/// Sends a given amount of data to the connected server.
		/// </summary>
		/// <param name="data">The data to send.</param>
		public void SendData(byte[] data) {
			try {
				lock (this.clientLock)
					this.client.GetStream().BeginWrite(data, 0, data.Length, this.OnWrite, null);
			} catch (ObjectDisposedException) {
				// Connection closed
			} catch (IOException) {
				// Connection closed
			} catch (InvalidOperationException) {

			}
		}

		/// <summary>
		/// Callback called when writing to a TCP connection network stream.
		/// </summary>
		/// <param name="ar"></param>
		private void OnConnect(IAsyncResult ar) {
			Tuple<ServerDevice, int> state = ar.AsyncState as Tuple<ServerDevice, int>;
			try {
				lock (this.clientLock) {
					this.client.EndConnect(ar);

					if (this.client.IsConnected()) {
						this.client.GetStream().BeginRead(this.buffer, 0, this.buffer.Length, this.OnDataReceived, null);
						this.serverDevice = state.Item1;
						this.OnTcpConnectSuccess?.Invoke(this, new ServerEventArgs(state.Item1));
					}
					else {
						if (state.Item2 < TcpOptions.DefaultMaxConnectTries) {
							state = new Tuple<ServerDevice, int>(state.Item1, state.Item2 + 1);
							this.client.BeginConnect(state.Item1.IPAddress, TcpOptions.DefaultPort, this.OnConnect, state);
						}
						else
							this.OnTcpConnectFailure?.Invoke(this, new ServerEventArgs(state.Item1));
					}
				}
			} catch (SocketException) {
				this.OnTcpConnectFailure?.Invoke(this, new ServerEventArgs(state.Item1));
			} catch (ObjectDisposedException) {
				this.OnTcpConnectFailure?.Invoke(this, new ServerEventArgs(state.Item1));
			} catch (NullReferenceException) {
				this.OnTcpConnectFailure?.Invoke(this, new ServerEventArgs(state.Item1));
			}
		}

		/// <summary>
		/// Callback called when writing to a TCP connection network stream.
		/// </summary>
		/// <param name="ar"></param>
		private void OnWrite(IAsyncResult ar) {
			try {
				lock (this.clientLock)
					this.client.GetStream().EndWrite(ar);
			} catch (ObjectDisposedException) {
				// Connection closed
			} catch (IOException) {
				// Connection closed
			} catch (InvalidOperationException) {

			}
		}

		/// <summary>
		/// Callback called when receiving data from a TCP connection netowrk stream.
		/// </summary>
		/// <param name="ar"></param>
		private void OnDataReceived(IAsyncResult ar) {
			try {
				int received;

				lock (this.clientLock)
					received = this.client.GetStream().EndRead(ar);

				string dataString = this.remainingData + Encoding.UTF8.GetString(this.buffer, 0, received);

				while (this.SplitReceivedString(dataString, out string message, out this.remainingData)) {
					object dataObject = JsonConvert.DeserializeObject(message, SerializationOptions.DefaultSerializationOptions);

					if (dataObject is Message)
						this.OnMessageReceived?.Invoke(this, new ServerMessageEventArgs(this.serverDevice, dataObject as Message));
					else if (dataObject is Request) {
						Request request = dataObject as Request;

						if (request.Method == Request.Type.Ping)
							this.SendResponse(new Response() {
								Method = Response.Type.Ping,
								Body = null
							});
						else
							this.OnRequestReceived?.Invoke(this, new ServerRequestEventArgs(this.serverDevice, request));
					}
					else if (dataObject is Response) {
						Response response = dataObject as Response;

						switch (response.Method) {
							case Response.Type.ConnectSuccess:
								AuthenticatedMessageBody body = response.Body as AuthenticatedMessageBody;
								this.OnConnectSuccess?.Invoke(this, new ClientEventArgs(null, body.ClientDevice));
								break;
							case Response.Type.Ping:
								this.pingTries = 0;
								break;
							default:
								this.OnResponseReceived?.Invoke(this, new ServerResponseEventArgs(this.serverDevice, response));
								break;
						}
					}

					dataString = this.remainingData;
				}

				lock (this.clientLock)
					this.client.GetStream().BeginRead(this.buffer, 0, this.buffer.Length, this.OnDataReceived, null);
			} catch (ObjectDisposedException) {
				// Connection closed
			} catch (IOException) {
				// Connection closed
			} catch (InvalidOperationException) {

			}
		}

		public bool SplitReceivedString(in string dataString, out string message, out string remainingData) {
			if (!dataString.Contains(TcpOptions.MessageSeparator)) {
				remainingData = dataString;
				message = null;
				return false;
			}

			int index = dataString.IndexOf(TcpOptions.MessageSeparator);
			message = dataString.Substring(0, index);

			int remainingIndex = index + TcpOptions.MessageSeparator.Length;
			remainingData = dataString.Length > remainingIndex ? dataString.Substring(remainingIndex) : "";

			return true;
		}

		/// <summary>
		/// Method which is running for the whole lifetime of the TCP client to monitor the connection status of the underlying client.
		/// </summary>
		private void ClientConnectionMonitoring() {
			while (true) {
				try {
					if (this.serverDevice == null)
						continue;

					lock (this.clientLock) {

						if (!this.client.IsConnected() || this.pingTries == MonitoringOptions.MaxPingTries)
							this.Stop();
						else
							this.SendResponse(new Response() {
								Method = Response.Type.Ping,
								Body = null
							});
					}

					Thread.Sleep(MonitoringOptions.Delay);
				}
				catch (ObjectDisposedException) { }
				catch (IOException) { }
				catch (InvalidOperationException) { }
			}
		}
	}
}