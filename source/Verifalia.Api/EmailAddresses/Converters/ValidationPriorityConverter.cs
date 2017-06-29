using System;
using Newtonsoft.Json;
using Verifalia.Api.EmailAddresses.Models;

namespace Verifalia.Api.EmailAddresses.Converters
{
    internal class ValidationPriorityConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((ValidationPriority)value).Value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = Convert.ToByte(reader.Value);
            return new ValidationPriority(value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ValidationPriority);
        }
    }
}