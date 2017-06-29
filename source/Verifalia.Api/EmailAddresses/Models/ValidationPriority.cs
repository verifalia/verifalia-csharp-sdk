using System;

namespace Verifalia.Api.EmailAddresses.Models
{
    public class ValidationPriority
    {
        internal byte Value { get; }

        public static ValidationPriority Lowest = new ValidationPriority(0);
        public static ValidationPriority Default = new ValidationPriority(127);
        public static ValidationPriority Highest = new ValidationPriority(255);

        public ValidationPriority(byte value)
        {
            Value = value;
        }

        public override string ToString()
        {
            if (Value == Lowest.Value)
            {
                return String.Format("{0} (lowest)", Value);
            }

            if (Value == Default.Value)
            {
                return String.Format("{0} (default)", Value);
            }

            if (Value == Highest.Value)
            {
                return String.Format("{0} (highest)", Value);
            }

            return Value.ToString();
        }

        protected bool Equals(ValidationPriority other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValidationPriority)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(ValidationPriority left, ValidationPriority right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValidationPriority left, ValidationPriority right)
        {
            return !Equals(left, right);
        }
    }
}