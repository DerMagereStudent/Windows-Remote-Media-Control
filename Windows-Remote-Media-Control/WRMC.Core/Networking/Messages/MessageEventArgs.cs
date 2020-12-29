using System;

namespace WRMC.Core.Networking {
	public class MessageBodyEventArgs<T> : EventArgs where T : MessageBody {
		public T DataBody { get; set; }

		public MessageBodyEventArgs(T dataBody) {
			this.DataBody = dataBody;
		}
	}

	public class ClientEventArgs : EventArgs {
		public System.Net.Sockets.TcpClient Client { get; set; }
		public ClientDevice ClientDevice { get; set; }

		public ClientEventArgs(System.Net.Sockets.TcpClient client, ClientDevice clientDevice) {
			this.Client = client;
			this.ClientDevice = clientDevice;
		}
	}

	public class ServerEventArgs : EventArgs {
		public System.Net.Sockets.TcpListener Server { get; set; }
		public ServerDevice ServerDevice { get; set; }

		public ServerEventArgs(System.Net.Sockets.TcpListener server, ServerDevice serverDevice) {
			this.Server = server;
			this.ServerDevice = serverDevice;
		}
	}

	public class ClientMessageEventArgs : ClientEventArgs {
		public Message Message { get; set; }

		public ClientMessageEventArgs(System.Net.Sockets.TcpClient client, ClientDevice clientDevice, Message message) : base(client, clientDevice) {
			this.Message = message;
		}
	}

	public class ClientRequestEventArgs : ClientEventArgs {
		public Request Request { get; set; }

		public ClientRequestEventArgs(System.Net.Sockets.TcpClient client, ClientDevice clientDevice, Request request) : base(client, clientDevice) {
			this.Request = request;
		}
	}

	public class ClientResponseEventArgs : ClientEventArgs {
		public Response Response { get; set; }

		public ClientResponseEventArgs(System.Net.Sockets.TcpClient client, ClientDevice clientDevice, Response response) : base(client, clientDevice) {
			this.Response = response;
		}
	}

	public class ServerMessageEventArgs : ServerEventArgs {
		public Message Message { get; set; }

		public ServerMessageEventArgs(System.Net.Sockets.TcpListener server, ServerDevice serverDevice, Message message) : base(server, serverDevice) {
			this.Message = message;
		}
	}

	public class ServerRequestEventArgs : ServerEventArgs {
		public Request Request { get; set; }

		public ServerRequestEventArgs(System.Net.Sockets.TcpListener server, ServerDevice serverDevice, Request request) : base(server, serverDevice) {
			this.Request = request;
		}
	}

	public class ServerResponseEventArgs : ServerEventArgs {
		public Response Response { get; set; }

		public ServerResponseEventArgs(System.Net.Sockets.TcpListener server, ServerDevice serverDevice, Response response) : base(server, serverDevice) {
			this.Response = response;
		}
	}
}