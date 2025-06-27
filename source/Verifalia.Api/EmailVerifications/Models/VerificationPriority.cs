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
using System.Globalization;

namespace Verifalia.Api.EmailVerifications.Models
{
    /// <summary>
    /// The priority (speed) of an email verification job relative to other jobs in the parent Verifalia account. 
    /// When an account has many concurrent verification jobs, this value allows you to increase the processing 
    /// speed of a job relative to the others.
    /// <remarks>The allowed range of values ranges from <see cref="Lowest"/> (0 - lowest priority) to <see cref="Highest"/>
    /// (255 - highest priority), where the middle value <see cref="Normal"/> (127) represents normal priority. If not specified,
    /// Verifalia processes all concurrent verification jobs for an account at the same speed.</remarks>
    /// </summary>
    /// <inheritdoc />
    public class VerificationPriority : IEquatable<VerificationPriority>
    {
        internal byte Value { get; }

        /// <summary>
        /// The lowest possible processing priority for an email verification job.
        /// </summary>
        public static readonly VerificationPriority Lowest = new(0);

        /// <summary>
        /// Normal processing priority for an email verification job.
        /// </summary>
        public static readonly VerificationPriority Normal = new(127);

        /// <summary>
        /// The highest possible processing priority for an email verification job.
        /// </summary>
        public static readonly VerificationPriority Highest = new(255);

        public VerificationPriority(byte value)
        {
            Value = value;
        }

        public bool Equals(VerificationPriority? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VerificationPriority) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(VerificationPriority? left, VerificationPriority? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VerificationPriority? left, VerificationPriority? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            if (Value == Lowest.Value)
            {
                return $"{Value} (lowest)";
            }

            if (Value == Normal.Value)
            {
                return $"{Value} (normal)";
            }

            if (Value == Highest.Value)
            {
                return $"{Value} (highest)";
            }

            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}