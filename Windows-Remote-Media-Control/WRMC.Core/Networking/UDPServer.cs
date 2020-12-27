using System;
using System.Text;

using Newtonsoft.Json;

namespace WRMC.Core.Networking {
	/// <summary>
	/// Represents the windows-/server-side part of the UDP communication.
	/// </summary>
	public class UdpServer {
		private System.Net.Sockets.UdpClient server;

		/// <summary>
		/// Property which indicates if the server is currently listening for incoming datagrams.
		/// </summary>
		public bool Running { get; protected set; } = false;

		/// <summary>
		/// Event handler which is called when the server received a <see cref="Request.Type.FindServer"/> request.
		/// </summary>
		public event TypedEventHandler<UdpServer, EventArgs> OnFindServerRequestReceived = null;

		/// <summary>
		/// Creates a new instance of the class <see cref="UdpServer"/>.
		/// </summary>
		public UdpServer() {
			this.server = new System.Net.Sockets.UdpClient(UdpOptions.DefaultServerPort);
		}

		/// <summary>
		/// Starts listening to UDP datagrams. Fails if already listening.
		/// </summary>
		public void Start() {
			if (this.Running)
				return;

			// If client was closed due to the call of System.Net.UdpClient.Close.
			if (this.server.Client == null)
				this.server = new System.Net.Sockets.UdpClient(UdpOptions.DefaultServerPort);

			this.server.BeginReceive(this.OnRequestReceived, null);
			this.Running = true;
		}

		/// <summary>
		/// Stops listening for UDP datagrams. Fails if currently not listening.
		/// </summary>
		public void Stop() {
			if (!this.Running)
				return;

			this.server.Close();
			this.Running = false;
		}

		/// <summary>
		/// Sends server device information to all UDP clients in the local network.
		/// </summary>
		/// <param name="serverDevice">The device information which should be sent.</param>
		/// <returns>True if the sending process was successful. False if sending was not possible or the server was not able to send all the data.</returns>
		public bool SendServerInformation(ServerDevice serverDevice) {
			if (!this.Running || this.server.Client == null || serverDevice == null)
				return false;

			Response response = new Response() {
				Method = Response.Type.Servers,
				Body = new ServerResponseBody() {
					Server = serverDevice
				}
			};

			byte[] datagram = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, SerializationOptions.DefaultMessageSerializationOptions));

			return this.server.Send(datagram, datagram.Length, UdpOptions.DefaultServerSendingIPEndPoint) == datagram.Length;
		}

		/// <summary>
		/// Callback which is called once the underlying UDP client receives a datagram or the listening process has been stopped.
		/// </summary>
		/// <param name="ar"></param>
		private void OnRequestReceived(IAsyncResult ar) {
			try {
				// EndReceive throws a ObjectDisposedException if the underlying UDP client was closed.
				byte[] datagram = this.server.EndReceive(ar, ref UdpOptions.DefaultServerListenIPEndPoint);
				string datagramString = Encoding.UTF8.GetString(datagram);
				object datagramObject = JsonConvert.DeserializeObject(datagramString, SerializationOptions.DefaultMessageSerializationOptions);

				if (datagramObject is Request) {
					Request request = datagramObject as Request;

					if (request.Method == Request.Type.FindServer)
						this.OnFindServerRequestReceived?.Invoke(this, EventArgs.Empty);
				}

				this.server.BeginReceive(this.OnRequestReceived, null);
			}
			catch (ObjectDisposedException) {
				// Server was stopped
			}
		}
	}
}