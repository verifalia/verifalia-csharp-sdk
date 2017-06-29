using System;

namespace Verifalia.Api.EmailAddresses.Models
{
    public class DeduplicationMode
    {
        internal string NameOrGuid { get; private set; }

        public static DeduplicationMode Default
        {
            get
            {
                return new DeduplicationMode();
            }
        }

        public static DeduplicationMode Off
        {
            get
            {
                return new DeduplicationMode("off");
            }
        }

        public static DeduplicationMode Safe
        {
            get
            {
                return new DeduplicationMode("safe");
            }
        }

        private DeduplicationMode()
        {
        }

        public DeduplicationMode(string modeName)
        {
            if (modeName == null) throw new ArgumentNullException("modeName");

            NameOrGuid = modeName;
        }

        public DeduplicationMode(Guid modeGuid)
        {
            NameOrGuid = modeGuid.ToString("B");
        }

        protected bool Equals(DeduplicationMode other)
        {
            return string.Equals(NameOrGuid, other.NameOrGuid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DeduplicationMode)obj);
        }

        public override int GetHashCode()
        {
            return (NameOrGuid != null ? NameOrGuid.GetHashCode() : 0);
        }

        public static bool operator ==(DeduplicationMode left, DeduplicationMode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DeduplicationMode left, DeduplicationMode right)
        {
            return !Equals(left, right);
        }
    }
}