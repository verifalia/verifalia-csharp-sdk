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

#nullable enable

using System;
using System.Globalization;

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// The priority (speed) of a validation job, relative to the parent Verifalia account. In the event of an account
    /// with many concurrent validation jobs, this value allows to increase the processing speed of a job with respect to the others.
    /// <remarks>The allowed range of values spans from <see cref="Lowest"/> (0 - lowest priority) to <see cref="Highest"/>
    /// (255 - highest priority), where the midway value  <see cref="Normal"/> (127) means normal priority; if not specified,
    /// Verifalia processes all the concurrent validation jobs for an account using the same speed.</remarks>
    /// </summary>
    public class ValidationPriority : IEquatable<ValidationPriority>
    {
        internal byte Value { get; }

        /// <summary>
        /// The lowest possible processing priority (speed) for a validation job.
        /// </summary>
        public static ValidationPriority Lowest = new(0);

        /// <summary>
        /// Normal processing priority (speed) for a validation job.
        /// </summary>
        public static ValidationPriority Normal = new(127);

        /// <summary>
        /// The highest possible processing priority (speed) for a validation job.
        /// </summary>
        public static ValidationPriority Highest = new(255);

        public ValidationPriority(byte value)
        {
            Value = value;
        }

        public bool Equals(ValidationPriority? other)
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
            return Equals((ValidationPriority) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(ValidationPriority? left, ValidationPriority? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValidationPriority? left, ValidationPriority? right)
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