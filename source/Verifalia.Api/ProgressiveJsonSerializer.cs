/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2021 Cobisi Research
*
* Cobisi Research
* Via Della Costituzione, 31
* 35010 Vigonza
* Italy - European Union
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System.IO;
using System.Text;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Verifalia.Api.EmailValidations.Converters;
using Verifalia.Api.Common.Converters;

//using Verifalia.Api.EmailAddresses.Converters;

namespace Verifalia.Api
{
    /// <summary>
    /// Progressive json serializer / deserializer using Newtonsoft Json.
    /// Code partially adapted from: http://bytefish.de/blog/restsharp_custom_json_serializer/
    /// </summary>
    internal class ProgressiveJsonSerializer : ISerializer
    {
        private readonly JsonSerializer _serializer;

        internal ProgressiveJsonSerializer()
        {
            _serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                CheckAdditionalContent = false,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            // TODO: Move the following to a reflection-based initialization in order to decouple the serializer from the data-specific converters

            // Email-addresses specific

            _serializer.Converters.Add(new ValidationStatusConverter());
            _serializer.Converters.Add(new ValidationEntryCollectionConverter());
            _serializer.Converters.Add(new ValidationEntryStatusConverter());
            _serializer.Converters.Add(new ValidationPriorityConverter());
            _serializer.Converters.Add(new ValidationQualityConverter());
            _serializer.Converters.Add(new IPAddressConverter());
            _serializer.Converters.Add(new DeduplicationModeConverter());
            _serializer.Converters.Add(new ValidationEntryClassificationConverter());
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