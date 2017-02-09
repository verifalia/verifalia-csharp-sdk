using System;
using Newtonsoft.Json;

namespace Verifalia.Api
{
    internal class ProgressiveStructJsonConverter<T> : JsonConverter
        where T : struct
    {
        private readonly T _unknown;

        public ProgressiveStructJsonConverter(T unknown)
        {
            _unknown = unknown;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = Convert.ToString(reader.Value);
            T resolvedValue;

            return Enum.TryParse(value, out resolvedValue)
                ? resolvedValue
                : _unknown;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}