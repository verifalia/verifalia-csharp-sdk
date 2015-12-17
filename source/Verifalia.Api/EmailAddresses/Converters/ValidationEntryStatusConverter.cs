using System;
using Newtonsoft.Json;
using Verifalia.Api.EmailAddresses.Models;

namespace Verifalia.Api.EmailAddresses.Converters
{
    internal class ValidationEntryStatusConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = Convert.ToString(reader.Value);
            ValidationEntryStatus resolvedValue;

            return Enum.TryParse(value, out resolvedValue)
                ? resolvedValue
                : ValidationEntryStatus.Unknown;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (ValidationEntryStatus);
        }
    }
}