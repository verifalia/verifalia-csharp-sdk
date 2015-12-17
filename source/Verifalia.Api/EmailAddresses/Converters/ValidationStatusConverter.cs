using System;
using Newtonsoft.Json;
using Verifalia.Api.EmailAddresses.Models;

namespace Verifalia.Api.EmailAddresses.Converters
{
    internal class ValidationStatusConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = Convert.ToString(reader.Value);
            ValidationStatus resolvedValue;

            return Enum.TryParse(value, out resolvedValue)
                ? resolvedValue
                : ValidationStatus.Unknown;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ValidationStatus);
        }
    }
}