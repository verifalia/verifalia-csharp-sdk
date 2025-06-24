/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2025 Cobisi Research
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

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Verifalia.Api.EmailVerifications.Models;

namespace Verifalia.Api.EmailVerifications.Converters
{
    internal class VerificationEntryCollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var rootObject = JObject.Load(reader);
            var verificationEntryCollection = new VerificationEntryCollection();

            // Read the key-set metadata

            var meta = (JObject?)rootObject["meta"];

            if (meta != null)
            {
                verificationEntryCollection.Cursor = meta["cursor"]?.Value<string>();
                verificationEntryCollection.IsTruncated = meta["isTruncated"]?.Value<bool>() ?? false;
            }

            // Read the actual items

            using (var dataReader = rootObject["data"].CreateReader())
            {
                var entries = serializer.Deserialize<VerificationEntry[]>(dataReader);
                verificationEntryCollection.AddRange(entries);
            }

            return verificationEntryCollection;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(VerificationEntryCollection);
        }

        public override bool CanWrite => false;
    }
}