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
		private byte[] buffer;

		private object clientLock = new object();

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
			this.buffer = new byte[1024];

			Task.Factory.StartNew(() => this.ClientConnectionMonitoring());
		}

		/// <summary>
		/// Connects to a given server. Closes the previous connection.
		/// </summary>
		/// <param name="serverDevice">The server to connect to.</param>
		/// <returns>True if the connection was established.</returns>
		public bool Start(ServerDevice serverDevice) {
			lock (this.clientLock)
				if (this.serverDevice != null && this.serverDevice.Equals(serverDevice) && this.client.IsConnected())
					return true;

			this.Stop();

			lock (this.clientLock)
				for (int i = 0; i < TcpOptions.DefaultMaxConnectTries; i++) {
					try {
						this.client.Connect(serverDevice.IPAddress, TcpOptions.DefaultPort);
					} catch (SocketException) { }

					if (this.client.IsConnected()) {
						this.client.GetStream().BeginRead(this.buffer, 0, this.buffer.Length, this.OnDataReceived, null);
						this.serverDevice = serverDevice;
						return true;
					}
				}

			return false;
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

			this.OnConnectionClosed?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Sends a message to the connected server.
		/// </summary>
		/// <param name="message">The message to send.</param>
		public void SendMessage(Message message) {
			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, SerializationOptions.DefaultSerializationOptions));
			this.SendData(data);
		}

		/// <summary>
		/// Sends a request to the connected server.
		/// </summary>
		/// <param name="request">The request to send.</param>
		public void SendRequest(Request request) {
			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request, SerializationOptions.DefaultSerializationOptions));
			this.SendData(data);
		}

		/// <summary>
		/// Sends a response to the connected server.
		/// </summary>
		/// <param name="response">The response to send.</param>
		public void SendResponse(Response response) {
			byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, SerializationOptions.DefaultSerializationOptions));
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

				string dataString = Encoding.UTF8.GetString(this.buffer, 0, received);
				object dataObject = JsonConvert.DeserializeObject(dataString, SerializationOptions.DefaultSerializationOptions);

				if (dataObject is Message)
					this.OnMessageReceived?.Invoke(this, new ServerMessageEventArgs(this.serverDevice, dataObject as Message));
				else if (dataObject is Request)
					this.OnRequestReceived?.Invoke(this, new ServerRequestEventArgs(this.serverDevice, dataObject as Request));
				else if (dataObject is Response) {
					Response response = dataObject as Response;

					if (response.Method == Response.Type.ConnectSuccess) {
						AuthenticatedMessageBody body = response.Body as AuthenticatedMessageBody;
						this.OnConnectSuccess?.Invoke(this, new ClientEventArgs(null, body.ClientDevice));
					}
					else
						this.OnResponseReceived?.Invoke(this, new ServerResponseEventArgs(this.serverDevice, response));
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

		/// <summary>
		/// Method which is running for the whole lifetime of the TCP client to monitor the connection status of the underlying client.
		/// </summary>
		private void ClientConnectionMonitoring() {
			while (true) {
				try {
					lock (this.clientLock)
						if (!this.client.IsConnected())
							this.Stop();

					Thread.Sleep(1000);
				}
				catch (ObjectDisposedException) { }
				catch (IOException) { }
				catch (InvalidOperationException) { }
			}
		}
	}
}