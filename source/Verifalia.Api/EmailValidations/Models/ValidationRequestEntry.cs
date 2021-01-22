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

using System;
using Newtonsoft.Json;

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// A single item of a <see cref="ValidationRequest"/> containing an email address to validate, specified by way
    /// of the <see cref="InputData"/> property.
    /// </summary>
    public class ValidationRequestEntry
    {
        private const int MaxCustomLength = 50;
        private string _custom;

        /// <summary>
        /// The input string to validate, which should represent an email address.
        /// </summary>
        [JsonProperty("inputData", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string InputData { get; private set; }

        /// <summary>
        /// An optional, custom string which is passed back upon completing the validation job.
        /// <remarks>Setting this value is useful in the event you wish to have a custom reference of this <see cref="ValidationRequestEntry"/>
        /// with something else (for example, a record in your database).</remarks>
        /// <remarks>This value accepts a string with a maximum length of 50 characters.</remarks>
        /// </summary>
        [JsonProperty("custom", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Custom
        {
            get => _custom;
            set
            {
                if (value != null)
                {
                    if (value.Length > MaxCustomLength)
                    {
                        throw new ArgumentOutOfRangeException(nameof(value), $"Custom value '{value}' exceeds the maximum allowed length of {MaxCustomLength} characters.");
                    }
                }

                _custom = value;
            }
        }

        /// <summary>
        /// Initializes a new <see cref="ValidationRequestEntry"/>.
        /// </summary>
        /// <param name="inputData">The input data string (which should be an email address) to validate.</param>
        /// <param name="custom">An optional, custom string which is passed back upon completing the validation job.</param>
        public ValidationRequestEntry(string inputData, string custom = null)
        {
            InputData = inputData ?? throw new ArgumentNullException(nameof(inputData));
            Custom = custom;
        }
    }
}