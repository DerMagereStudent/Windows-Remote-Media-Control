using System;

namespace WRMC.Core.Networking {
	public class MessageBodyEventArgs<T> : EventArgs where T : MessageBody {
		public MessageBody DataBody;

		public MessageBodyEventArgs(T dataBody) {
			this.DataBody = dataBody;
		}
	}

	public class MessageEventArgs<T> : EventArgs where T : Message {
		public Message Message;

		public MessageEventArgs(T message) {
			this.Message = message;
		}
	}

	public class RequestEventArgs<T> : EventArgs where T : Request {
		public Request Request;

		public RequestEventArgs(T request) {
			this.Request = request;
		}
	}

	public class ResponseEventArgs<T> : EventArgs where T : Response {
		public Response Response;

		public ResponseEventArgs(T response) {
			this.Response = response;
		}
	}
}