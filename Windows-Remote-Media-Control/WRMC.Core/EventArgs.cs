using System;
using System.Collections.Generic;
using System.Text;

namespace WRMC.Core {
	public class EventArgs<T> : EventArgs {
		public static new EventArgs<T> Empty => new EventArgs<T>(default(T));

		public T Data { get; set; }

		public EventArgs(T data) {
			this.Data = data;
		}

		public static implicit operator EventArgs<T>(T data) => new EventArgs<T>(data);
	}
}