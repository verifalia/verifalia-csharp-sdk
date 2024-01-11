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

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// A reference to a Verifalia quality level. Quality levels determine how Verifalia validates email addresses, including whether
    /// and how the automatic reprocessing logic occurs (for transient statuses) and the verification timeouts settings.
    /// </summary>
    /// <remarks>Use one of <see cref="Standard"/>, <see cref="High"/> or <see cref="Extreme"/> values or a custom quality level ID
    /// if you have one (custom quality levels are available to premium plans only).</remarks>
    public class QualityLevelName : IEquatable<QualityLevelName>
    {
        internal string NameOrGuid { get; }

        /// <summary>
        /// The Standard quality level. Suitable for most businesses, provides good results for the vast majority of email addresses;
        /// features a single validation pass and 5 second anti-tarpit time; less suitable for validating email addresses with temporary
        /// issues (mailbox over quota, greylisting, etc.) and slower mail exchangers.
        /// </summary>
        public static QualityLevelName Standard => new("Standard");

        /// <summary>
        /// The High quality level. Much higher quality, featuring 3 validation passes and 50 seconds of anti-tarpit time, so you can
        /// even validate most addresses with temporary issues, or slower mail exchangers.
        /// </summary>
        public static QualityLevelName High => new("High");

        /// <summary>
        /// The Extreme quality level. Unbeatable, top-notch quality for professionals who need the best results the industry can offer:
        /// performs email validations at the highest level, with 9 validation passes and 2 minutes of anti-tarpit time.
        /// </summary>
        public static QualityLevelName Extreme => new("Extreme");

        private QualityLevelName()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="QualityLevelName"/> from its name.
        /// </summary>
        /// <param name="qualityName">The name of the quality level.</param>
        /// <remarks>Use <see cref="Standard"/>, <see cref="High"/> or <see cref="Extreme"/>, if no name is known.</remarks>
        public QualityLevelName(string qualityName)
        {
            NameOrGuid = qualityName ?? throw new ArgumentNullException(nameof(qualityName));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="QualityLevelName"/> from its ID.
        /// </summary>
        /// <param name="qualityGuid">The ID of the quality level.</param>
        /// <remarks>Use <see cref="Standard"/>, <see cref="High"/> or <see cref="Extreme"/>, if no ID is known.</remarks>
        public QualityLevelName(Guid qualityGuid)
        {
            NameOrGuid = qualityGuid.ToString("B");
        }

        public bool Equals(QualityLevelName? other)
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
            return Equals((QualityLevelName) obj);
        }

        public override int GetHashCode()
        {
            return NameOrGuid.GetHashCode();
        }

        public static bool operator ==(QualityLevelName? left, QualityLevelName? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(QualityLevelName? left, QualityLevelName? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            if (String.Equals(NameOrGuid, Standard.NameOrGuid, StringComparison.OrdinalIgnoreCase))
            {
                return $"{NameOrGuid} (standard)";
            }

            if (String.Equals(NameOrGuid, High.NameOrGuid, StringComparison.OrdinalIgnoreCase))
            {
                return $"{NameOrGuid} (high)";
            }

            if (String.Equals(NameOrGuid, Extreme.NameOrGuid, StringComparison.OrdinalIgnoreCase))
            {
                return $"{NameOrGuid} (extreme)";
            }

            return NameOrGuid;
        }
    }
}