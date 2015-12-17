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
    }
}