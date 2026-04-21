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

namespace Verifalia.Api.EmailVerifications.Models
{
    /// <summary>
    /// Defines the strategy Verifalia uses to detect duplicate email addresses when processing multiple entries.
    /// </summary>
    /// <remarks>Any email address that appears more than once (after the first occurrence) will be marked as <see cref="VerificationEntryStatus.Duplicate"/>.</remarks>
    /// <inheritdoc />
    public sealed class DeduplicationMode : IEquatable<DeduplicationMode>
    {
        internal string NameOrId { get; }

        /// <summary>
        /// Turns off duplicates detection.
        /// </summary>
        public static DeduplicationMode Off => new("Off");

        /// <summary>
        /// Uses strict rules to accurately identify duplicates, minimizing false positives.
        /// </summary>
        public static DeduplicationMode Safe => new("Safe");

        /// <summary>
        /// Applies relaxed rules that assume modern configurations of target email service providers, excluding the
        /// broader options allowed by RFCs from the '80s.
        /// </summary>
        public static DeduplicationMode Relaxed => new("Relaxed");

        private DeduplicationMode()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DeduplicationMode"/> from its name or ID.
        /// </summary>
        /// <param name="nameOrId">The name or ID of the deduplication mode.</param>
        /// <remarks>Use <see cref="Off"/>, <see cref="Relaxed"/>, or <see cref="Safe"/>, if no name or ID is known.</remarks>
        public DeduplicationMode(string nameOrId)
        {
            NameOrId = nameOrId ?? throw new ArgumentNullException(nameof(nameOrId));
        }

        public bool Equals(DeduplicationMode? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return NameOrId == other.NameOrId;
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
            return NameOrId.GetHashCode();
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
            if (String.Equals(NameOrId, Off.NameOrId, StringComparison.OrdinalIgnoreCase))
            {
                return $"{NameOrId} (off)";
            }

            if (String.Equals(NameOrId, Safe.NameOrId, StringComparison.OrdinalIgnoreCase))
            {
                return $"{NameOrId} (safe)";
            }

            if (String.Equals(NameOrId, Relaxed.NameOrId, StringComparison.OrdinalIgnoreCase))
            {
                return $"{NameOrId} (relaxed)";
            }
            
            return NameOrId;
        }
    }
}