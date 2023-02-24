/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2023 Cobisi Research
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

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// A strategy Verifalia follows while determining which email addresses are duplicates, within a multiple items validation.
    /// <remarks>Duplicated items (after the first occurrence) will have the <see cref="ValidationEntryStatus.Duplicate"/> status.</remarks>
    /// </summary>
    public class DeduplicationMode : IEquatable<DeduplicationMode>
    {
        internal string NameOrGuid { get; }

        /// <summary>
        /// Duplicates detection is turned off.
        /// </summary>
        public static DeduplicationMode Off => new("Off");

        /// <summary>
        /// Identifies duplicates using an algorithm with safe rules-only, which guarantee no false duplicates.
        /// </summary>
        public static DeduplicationMode Safe => new("Safe");

        /// <summary>
        /// Identifies duplicates using a set of relaxed rules which assume the target email service providers
        /// are configured with modern settings only (instead of the broader options the RFCs from the '80s allow).
        /// </summary>
        public static DeduplicationMode Relaxed => new("Relaxed");

        private DeduplicationMode()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DeduplicationMode"/> from its name.
        /// </summary>
        /// <param name="modeName">The name of the deduplication mode.</param>
        /// <remarks>Use <see cref="Off"/> or <see cref="Safe"/>, if no name is known.</remarks>
        public DeduplicationMode(string modeName)
        {
            NameOrGuid = modeName ?? throw new ArgumentNullException(nameof(modeName));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DeduplicationMode"/> from its ID.
        /// </summary>
        /// <param name="modeGuid">The ID of the deduplication mode.</param>
        /// <remarks>Use <see cref="Off"/> or <see cref="Safe"/>, if no ID is known.</remarks>
        public DeduplicationMode(Guid modeGuid)
        {
            NameOrGuid = modeGuid.ToString("B");
        }

        public bool Equals(DeduplicationMode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return NameOrGuid == other.NameOrGuid;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DeduplicationMode) obj);
        }

        public override int GetHashCode()
        {
            return NameOrGuid.GetHashCode();
        }

        public static bool operator ==(DeduplicationMode? left, DeduplicationMode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DeduplicationMode? left, DeduplicationMode? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            if (String.Equals(NameOrGuid, Off.NameOrGuid, StringComparison.OrdinalIgnoreCase))
            {
                return $"{NameOrGuid} (off)";
            }

            if (String.Equals(NameOrGuid, Safe.NameOrGuid, StringComparison.OrdinalIgnoreCase))
            {
                return $"{NameOrGuid} (safe)";
            }

            if (String.Equals(NameOrGuid, Relaxed.NameOrGuid, StringComparison.OrdinalIgnoreCase))
            {
                return $"{NameOrGuid} (relaxed)";
            }
            
            return NameOrGuid;
        }
    }
}