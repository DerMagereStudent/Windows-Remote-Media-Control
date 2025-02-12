﻿using System.Collections.Generic;

using Newtonsoft.Json;

namespace WRMC.Core.Networking {
	public static class SerializationOptions {
		public static JsonSerializerSettings DefaultSerializationOptions => new JsonSerializerSettings() {
			TypeNameHandling = TypeNameHandling.All,
			Converters = new List<JsonConverter>() {
				new KeyedListJsonConverter<ServerDevice, long>()
			}
		};
	}
}