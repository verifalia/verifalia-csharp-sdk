﻿using System.IO;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using Verifalia.Api.EmailAddresses.Converters;

namespace Verifalia.Api
{
    /// <summary>
    /// Progressive json serializer / deserializer using Newtonsoft Json.
    /// Code adapted from: http://bytefish.de/blog/restsharp_custom_json_serializer/
    /// </summary>
    internal class ProgressiveJsonSerializer : ISerializer, IDeserializer
    {
        readonly Newtonsoft.Json.JsonSerializer _serializer;

        internal ProgressiveJsonSerializer()
        {
            _serializer = new Newtonsoft.Json.JsonSerializer()
            {
                NullValueHandling = NullValueHandling.Ignore,
                CheckAdditionalContent = false,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            _serializer.Converters.Add(new ValidationStatusConverter());
            _serializer.Converters.Add(new ValidationEntryStatusConverter());
        }

        public string ContentType
        {
            get { return "application/json"; } // Probably used for Serialization?
            set { }
        }


        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

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

        public T Deserialize<T>(IRestResponse response)
        {
            var content = response.Content;

            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}