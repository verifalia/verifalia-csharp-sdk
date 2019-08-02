/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2019 Cobisi Research
*
* Cobisi Research
* Via Prima Strada, 35
* 35129, Padova
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
using Verifalia.Api.EmailValidations.Models;

namespace Verifalia.Api.EmailValidations.Converters
{
    internal class ValidationEntryCollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var rootObject = JObject.Load(reader);
            var validationEntryCollection = new ValidationEntryCollection();

            // Read the key-set metadata

            var meta = (JObject)rootObject["meta"];

            validationEntryCollection.Cursor = meta["cursor"]?.Value<string>();
            validationEntryCollection.IsTruncated = meta["isTruncated"]?.Value<bool>() ?? false;

            // Read the actual items

            using (var dataReader = rootObject["data"].CreateReader())
            {
                var entries = serializer.Deserialize<ValidationEntry[]>(dataReader);
                validationEntryCollection.AddRange(entries);
            }

            return validationEntryCollection;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ValidationEntryCollection);
        }

        public override bool CanWrite => false;
    }
}