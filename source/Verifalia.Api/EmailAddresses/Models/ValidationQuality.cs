using System;

namespace Verifalia.Api.EmailAddresses.Models
{
    public class ValidationQuality
    {
        internal string NameOrGuid { get; private set; }

        public static ValidationQuality Default
        {
            get
            {
                return new ValidationQuality();
            }
        }

        public static ValidationQuality Standard
        {
            get
            {
                return new ValidationQuality("standard");
            }
        }

        public static ValidationQuality High
        {
            get
            {
                return new ValidationQuality("high");
            }
        }

        public static ValidationQuality Extreme
        {
            get
            {
                return new ValidationQuality("extreme");
            }
        }

        private ValidationQuality()
        {
        }

        public ValidationQuality(string qualityName)
        {
            if (qualityName == null) throw new ArgumentNullException("qualityName");

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