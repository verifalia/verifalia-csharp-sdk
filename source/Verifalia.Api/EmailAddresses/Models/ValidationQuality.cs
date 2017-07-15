using System;

namespace Verifalia.Api.EmailAddresses.Models
{
    public class ValidationQuality
    {
        internal string NameOrGuid { get; }

        public static ValidationQuality Default => new ValidationQuality();

        public static ValidationQuality Standard => new ValidationQuality("standard");

        public static ValidationQuality High => new ValidationQuality("high");

        public static ValidationQuality Extreme => new ValidationQuality("extreme");

        private ValidationQuality()
        {
        }

        public ValidationQuality(string qualityName)
        {
            if (qualityName == null) throw new ArgumentNullException(nameof(qualityName));

            NameOrGuid = qualityName;
        }

        public ValidationQuality(Guid qualityGuid)
        {
            NameOrGuid = qualityGuid.ToString("B");
        }

        protected bool Equals(ValidationQuality other)
        {
            return string.Equals(NameOrGuid, other.NameOrGuid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValidationQuality)obj);
        }

        public override int GetHashCode()
        {
            return (NameOrGuid != null ? NameOrGuid.GetHashCode() : 0);
        }

        public static bool operator ==(ValidationQuality left, ValidationQuality right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValidationQuality left, ValidationQuality right)
        {
            return !Equals(left, right);
        }
    }
}