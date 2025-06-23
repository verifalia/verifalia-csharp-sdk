/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2024 Cobisi Research
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
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Verifalia.Api.Users.Models;

namespace Verifalia.Api.Users.Converters
{
    internal sealed class UserTypeConverter : JsonConverter
    {
        internal static readonly Dictionary<string, UserType> Mappings = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Administrator"] = UserType.Administrator,
            ["Standard"] = UserType.Standard,
            ["BrowserApp"] = UserType.BrowserApp,
        };
        
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            foreach (var mapping in Mappings)
            {
                if (mapping.Value == (UserType) value)
                {
                    writer.WriteValue(mapping.Key);
                    return;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported user type.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = Convert.ToString(reader.Value, CultureInfo.InvariantCulture);

            return Mappings.TryGetValue(value, out var mappedStatus)
                ? mappedStatus
                : throw new NotSupportedException($"User type {value} is not supported.");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(UserType);
        }
    }
}