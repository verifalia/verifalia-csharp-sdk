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

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// A classification of the <see cref="ValidationEntryStatus"/> of a <see cref="ValidationEntry"/>.
    /// </summary>
    /// <remarks>Use one of <see cref="Deliverable"/>, <see cref="Risky"/>, <see cref="Undeliverable"/> or <see cref="Unknown"/>
    /// values if you don't have a custom classification.</remarks>
    public class ValidationEntryClassification : IEquatable<ValidationEntryClassification>
    {
        /// <summary>
        /// Gets the name of the <see cref="ValidationEntryClassification"/>.
        /// <remarks>The default classifications are available by way of the <see cref="Deliverable"/>, <see cref="Risky"/>,
        /// <see cref="Undeliverable"/> and <see cref="Unknown"/> properties.</remarks>
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// A <see cref="ValidationEntry" /> marked as <see cref="Deliverable"/> refers to an <see cref="ValidationEntry.EmailAddress"/> which is deliverable.
        /// </summary>
        public static ValidationEntryClassification Deliverable => new("Deliverable");

        /// <summary>
        /// A <see cref="ValidationEntry" /> marked as <see cref="Risky"/> refers to an <see cref="ValidationEntry.EmailAddress"/> which could be no longer valid.
        /// </summary>
        public static ValidationEntryClassification Risky => new("Risky");

        /// <summary>
        /// A <see cref="ValidationEntry" /> marked as <see cref="Undeliverable"/> refers to an <see cref="ValidationEntry.EmailAddress"/>
        /// which is either invalid or no longer deliverable.
        /// </summary>
        public static ValidationEntryClassification Undeliverable => new("Undeliverable");

        /// <summary>
        /// A <see cref="ValidationEntry" /> marked as <see cref="Unknown"/> contains an email address whose deliverability is unknown.
        /// </summary>
        public static ValidationEntryClassification Unknown => new("Unknown");

        private ValidationEntryClassification()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ValidationEntryClassification"/> from its name.
        /// </summary>
        /// <param name="name">The name of the classification.</param>
        /// <remarks>Use one of <see cref="Deliverable"/>, <see cref="Risky"/>, <see cref="Undeliverable"/> or <see cref="Unknown"/> values
        /// if you don't have a custom classification.</remarks>
        public ValidationEntryClassification(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public bool Equals(ValidationEntryClassification? other)
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
            return Equals((ValidationEntryClassification) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(ValidationEntryClassification? left, ValidationEntryClassification? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValidationEntryClassification? left, ValidationEntryClassification? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}