using System.IO;
using Newtonsoft.Json;
using Verifalia.Api.EmailAddresses.Converters;
using Verifalia.Api.EmailAddresses.Models;
using System.Text;

namespace Verifalia.Api
{
    /// <summary>
    /// Progressive json serializer / deserializer using Newtonsoft Json.
    /// Code partially adapted from: http://bytefish.de/blog/restsharp_custom_json_serializer/
    /// </summary>
    internal class ProgressiveJsonSerializer : Flurl.Http.Configuration.ISerializer
    {
        private readonly JsonSerializer _serializer;

        internal ProgressiveJsonSerializer()
        {
            _serializer = new JsonSerializer()
            {
                NullValueHandling = NullValueHandling.Ignore,
                CheckAdditionalContent = false,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            // TODO: Move the following to a reflection-based initialization in order to decouple the serializer from the data-specific converters

            _serializer.Converters.Add(new ProgressiveStructJsonConverter<ValidationStatus>(ValidationStatus.Unknown));
            _serializer.Converters.Add(new ProgressiveStructJsonConverter<ValidationEntryStatus>(ValidationEntryStatus.Unknown));

            // Email-addresses specific

            _serializer.Converters.Add(new ValidationPriorityConverter());
        }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    _serializer.Serialize(jsonTextWriter, obj);

                    return stringWriter.ToString();
                }
            }
        }

        public T Deserialize<T>(string s)
        {
            using (var textReader = new StringReader(s))
            {
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    return _serializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            {
                return Deserialize<T>(streamReader.ReadToEnd());
            }
        }
    }
}