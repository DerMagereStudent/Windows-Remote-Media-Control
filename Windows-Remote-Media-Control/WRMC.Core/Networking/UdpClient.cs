using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WRMC.Core.Networking {
	public class UdpClient {
		private System.Net.Sockets.UdpClient client;
		private CancellationTokenSource cancellationTokenSource;

		/// <summary>
		/// Property which indicates if the client is currently listening for incoming datagrams.
		/// </summary>
		public bool Running { get; protected set; } = false;

		/// <summary>
		/// Event handler which is called when the client received a <see cref="Response.Type.Servers"/> response.
		/// </summary>
		public event TypedEventHandler<UdpClient, MessageBodyEventArgs<ServerResponseBody>> OnServersResponseReceived = null;

		/// <summary>
		/// Creates a new instance of the class <see cref="UdpClient"/>.
		/// </summary>
		public UdpClient() {
			this.client = new System.Net.Sockets.UdpClient(UdpOptions.DefaultClientPort);
		}

		/// <summary>
		/// Starts listening to UDP datagrams. Fails if already listening.
		/// </summary>
		public void StartListening() {
			if (this.Running)
				return;

			// If client was closed due to the call of System.Net.UdpClient.Close.
			if (this.client.Client == null)
				this.client = new System.Net.Sockets.UdpClient(UdpOptions.DefaultClientPort);

			this.client.BeginReceive(this.OnResponseReceived, null);
			this.Running = true;
		}

		/// <summary>
		/// Starts sending <see cref="Request.Type.FindServer"/> requests to the local network. Fails if sending is still active while calling this method a second time.
		/// </summary>
		/// <param name="sendTimeout">The time span between two requests.</param>
		/// <param name="searchDuration">The duration the client should keep sending requests.</param>
		public void StartSending(int sendTimeout, int searchDuration) {
			if (this.cancellationTokenSource == null || !this.cancellationTokenSource.IsCancellationRequested)
				return;

			this.cancellationTokenSource = new CancellationTokenSource(searchDuration);

			Task.Factory.StartNew(() => {
				Request request = new Request() {
					Method = Request.Type.FindServer,
					Body = null
				};

				byte[] datagram = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request, SerializationOptions.DefaultMessageSerializationOptions));

				while (!this.cancellationTokenSource.IsCancellationRequested) {
					this.client.Send(datagram, datagram.Length, UdpOptions.DefaultClientSendingIPEndPoint);
					Thread.Sleep(sendTimeout);
				}
			}, this.cancellationTokenSource.Token);
		}

		/// <summary>
		/// Stops listening for UDP datagrams. Fails if currently not listening.
		/// </summary>
		public void StopListening() {
			if (!this.Running)
				return;

			this.client.Close();
			this.Running = false;
		}

		/// <summary>
		/// Stops the current sending process.
		/// </summary>
		public void StopSending() {
			if (this.cancellationTokenSource != null && !this.cancellationTokenSource.IsCancellationRequested)
				this.cancellationTokenSource.Cancel();
		}

		/// <summary>
		/// Callback which is called once the underlying UDP client receives a datagram or the listening process has been stopped.
		/// </summary>
		/// <param name="ar"></param>
		private void OnResponseReceived(IAsyncResult ar) {
			try {
				// EndReceive throws a ObjectDisposedException if the underlying UDP client was closed.
				byte[] datagram = this.client.EndReceive(ar, ref UdpOptions.DefaultClientListenIPEndPoint);
				string datagramString = Encoding.UTF8.GetString(datagram);
				object datagramObject = JsonConvert.DeserializeObject(datagramString, SerializationOptions.DefaultMessageSerializationOptions);

				if (datagramObject is Response) {
					Response response = datagramObject as Response;

					if (response.Method == Response.Type.Servers)
						this.OnServersResponseReceived?.Invoke(this, new MessageBodyEventArgs<ServerResponseBody>(response.Body as ServerResponseBody));
				}
			}
			catch (ObjectDisposedException) {
				// Server was stopped
			}
		}
	}
}