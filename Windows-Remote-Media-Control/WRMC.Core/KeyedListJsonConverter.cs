using System;
using Newtonsoft.Json;

using WRMC.Core.Networking;

namespace WRMC.Core {
	public class KeyedListJsonConverter<TKey, TValue> : JsonConverter<KeyedList<TKey, TValue>> {
		public override KeyedList<TKey, TValue> ReadJson(JsonReader reader, Type objectType, KeyedList<TKey, TValue> existingValue, bool hasExistingValue, JsonSerializer serializer) {
			if (reader.TokenType == JsonToken.Null)
				return new KeyedList<TKey, TValue>();

			KeyedList<TKey, TValue> dict = new KeyedList<TKey, TValue>();

			while (reader.Read()) {
				TKey key = default(TKey);
				TValue value = default(TValue);

				if (reader.TokenType != JsonToken.PropertyName)
					continue;

				key = JsonConvert.DeserializeObject<TKey>(reader.Value as string, SerializationOptions.DefaultSerializationOptions);
				reader.Read();
				value = JsonConvert.DeserializeObject<TValue>(reader.Value as string, SerializationOptions.DefaultSerializationOptions);

				dict.Add(key, value);
			}

			return dict;
		}

		public override void WriteJson(JsonWriter writer, KeyedList<TKey, TValue> value, JsonSerializer serializer) {
			if (value == null) {
				writer.WriteNull();
				return;
			}

			writer.WriteStartObject();

			foreach (var kv in value) {
				writer.WritePropertyName(JsonConvert.SerializeObject(kv.Key, SerializationOptions.DefaultSerializationOptions));
				writer.WriteValue(JsonConvert.SerializeObject(kv.Value, SerializationOptions.DefaultSerializationOptions));
			}

			writer.WriteEndObject();
		}
	}
}