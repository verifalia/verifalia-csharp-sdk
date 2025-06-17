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

namespace Verifalia.Api.EmailVerifications.Models
{
    /// <summary>
    /// A classification of the <see cref="VerificationEntryStatus"/> of a <see cref="VerificationEntry"/>.
    /// </summary>
    /// <remarks>Use one of <see cref="Deliverable"/>, <see cref="Risky"/>, <see cref="Undeliverable"/> or <see cref="Unknown"/>
    /// values if you don't have a custom classification.</remarks>
    public class VerificationEntryClassification : IEquatable<VerificationEntryClassification>
    {
        /// <summary>
        /// Gets the name of the <see cref="VerificationEntryClassification"/>.
        /// <remarks>The default classifications are available by way of the <see cref="Deliverable"/>, <see cref="Risky"/>,
        /// <see cref="Undeliverable"/> and <see cref="Unknown"/> properties.</remarks>
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// A <see cref="VerificationEntry" /> marked as <see cref="Deliverable"/> refers to an <see cref="VerificationEntry.EmailAddress"/> which is deliverable.
        /// </summary>
        public static VerificationEntryClassification Deliverable => new("Deliverable");

        /// <summary>
        /// A <see cref="VerificationEntry" /> marked as <see cref="Risky"/> refers to an <see cref="VerificationEntry.EmailAddress"/> which could be no longer valid.
        /// </summary>
        public static VerificationEntryClassification Risky => new("Risky");

        /// <summary>
        /// A <see cref="VerificationEntry" /> marked as <see cref="Undeliverable"/> refers to an <see cref="VerificationEntry.EmailAddress"/>
        /// which is either invalid or no longer deliverable.
        /// </summary>
        public static VerificationEntryClassification Undeliverable => new("Undeliverable");

        /// <summary>
        /// A <see cref="VerificationEntry" /> marked as <see cref="Unknown"/> contains an email address whose deliverability is unknown.
        /// </summary>
        public static VerificationEntryClassification Unknown => new("Unknown");

        private VerificationEntryClassification()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="VerificationEntryClassification"/> from its name.
        /// </summary>
        /// <param name="name">The name of the classification.</param>
        /// <remarks>Use one of <see cref="Deliverable"/>, <see cref="Risky"/>, <see cref="Undeliverable"/> or <see cref="Unknown"/> values
        /// if you don't have a custom classification.</remarks>
        public VerificationEntryClassification(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public bool Equals(VerificationEntryClassification? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VerificationEntryClassification) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(VerificationEntryClassification? left, VerificationEntryClassification? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VerificationEntryClassification? left, VerificationEntryClassification? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}