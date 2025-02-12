﻿using System;
using System.Net;

using Newtonsoft.Json;

namespace WRMC.Core {
    public class IPAddressJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) => objectType == typeof(IPAddress);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            writer.WriteValue(value != null ? value.ToString() : null);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            return reader.Value != null ? IPAddress.Parse((string)reader.Value) : null;
        }
    }
}