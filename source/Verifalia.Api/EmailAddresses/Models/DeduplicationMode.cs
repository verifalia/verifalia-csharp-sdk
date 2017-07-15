using System;

namespace Verifalia.Api.EmailAddresses.Models
{
    public class DeduplicationMode
    {
        internal string NameOrGuid { get; }

        public static DeduplicationMode Default => new DeduplicationMode();

        public static DeduplicationMode Off => new DeduplicationMode("off");

        public static DeduplicationMode Safe => new DeduplicationMode("safe");

        private DeduplicationMode()
        {
        }

        public DeduplicationMode(string modeName)
        {
            if (modeName == null) throw new ArgumentNullException(nameof(modeName));

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