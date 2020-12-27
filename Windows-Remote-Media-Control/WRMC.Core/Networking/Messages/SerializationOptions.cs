using Newtonsoft.Json;

namespace WRMC.Core.Networking {
	public static class SerializationOptions {
		public static JsonSerializerSettings DefaultMessageSerializationOptions => new JsonSerializerSettings() {
			TypeNameHandling = TypeNameHandling.All
		};
	}
}
