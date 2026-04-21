/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2026 Cobisi Research
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

namespace Verifalia.Api.EmailVerifications.Models
{
    /// <summary>
    /// Represents a single item in a <see cref="VerificationRequest"/> that contains an email address to verify.
    /// </summary>
    public class VerificationRequestEntry
    {
        private const int MaxCustomLength = 50;
        private string? _custom;

        /// <summary>
        /// The input string to verify, which should represent an email address.
        /// </summary>
        [JsonProperty("inputData", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string InputData { get; private set; }

        /// <summary>
        /// An optional, custom string which is passed back upon completing the verification job.
        /// </summary>
        /// <remarks>
        /// This value is useful when you need to associate this <see cref="VerificationRequestEntry"/> 
        /// with something else, such as a record in your database. Maximum length is 50 characters.
        /// </remarks>
        [JsonProperty("custom", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string? Custom
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
        /// Initializes a new <see cref="VerificationRequestEntry"/>.
        /// </summary>
        /// <param name="inputData">The email address to verify.</param>
        /// <param name="custom">An optional, custom string which is passed back upon completing the verification job.</param>
        public VerificationRequestEntry(string inputData, string? custom = null)
        {
            InputData = inputData ?? throw new ArgumentNullException(nameof(inputData));
            Custom = custom;
        }
    }
}