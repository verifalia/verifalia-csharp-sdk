using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Verifalia.Api.EmailAddresses.Models
{
    /// <summary>
    /// Represents a new email validation request which will be submitted to the Verifalia engine.
    /// </summary>
    /// <remarks>To submit a new email validation request, either pass an instance of this type to the SubmitAsync() method of VerifaliaRestClient.EmailValidations
    /// or just use that method (or one of its async and non-async overloads) to specify your validation options.</remarks>
    public class ValidationRequest
    {
        /// <summary>
        /// The expected results quality for this validation job.
        /// </summary>
        /// <remarks>
        /// To see which results quality levels are available to your plan and choose a default for your subscription,
        /// please visit your Verifalia dashboard at https://verifalia.com/client-area.
        /// </remarks>
        public ValidationQuality Quality { get; set; }

        /// <summary>
        /// The priority for this validation job, with respect to your own Verifalia subscription.
        /// </summary>
        /// <remarks>
        /// A validation job with a higher priority would receive a larger processing slot percentage of the time allocated to
        /// your subscription, compared to a validation job with a lower priority.
        /// </remarks>
        public ValidationPriority Priority { get; set; }

        /// <summary>
        /// The duplicates removal algorithm to use while running this email validation job.
        /// </summary>
        public DeduplicationMode Deduplication { get; set; }

        /// <summary>
        /// The email addresses to validate, each along with an optional custom state to be passed back upon completion.
        /// </summary>
        public ValidationRequestEntry[] Entries { get; set; }

        public ValidationRequest(IEnumerable<string> emailAddresses)
            : this(emailAddresses, ValidationQuality.Default, DeduplicationMode.Default, ValidationPriority.Default)
        {
        }

        public ValidationRequest(IEnumerable<string> emailAddresses, ValidationQuality quality)
            : this(emailAddresses, quality, DeduplicationMode.Default, ValidationPriority.Default)
        {
        }

        public ValidationRequest(IEnumerable<string> emailAddresses, ValidationPriority priority)
            : this(emailAddresses, ValidationQuality.Default, DeduplicationMode.Default, priority)
        {
        }

        public ValidationRequest(IEnumerable<string> emailAddresses, DeduplicationMode deduplicationMode)
            : this(emailAddresses, ValidationQuality.Default, deduplicationMode, ValidationPriority.Default)
        {
        }

        public ValidationRequest(IEnumerable<string> emailAddresses, ValidationQuality quality, DeduplicationMode deduplicationMode, ValidationPriority priority)
            : this(emailAddresses.Select(emailAddress => new ValidationRequestEntry(emailAddress)), quality, deduplicationMode, priority)
        {
        }

        public ValidationRequest(IEnumerable<ValidationRequestEntry> entries)
            : this(entries, ValidationQuality.Default, DeduplicationMode.Default, ValidationPriority.Default)
        {
        }

        public ValidationRequest(IEnumerable<ValidationRequestEntry> entries, ValidationQuality quality)
            : this(entries, quality, DeduplicationMode.Default, ValidationPriority.Default)
        {
        }

        public ValidationRequest(IEnumerable<ValidationRequestEntry> entries, ValidationPriority priority)
            : this(entries, ValidationQuality.Default, DeduplicationMode.Default, priority)
        {
        }

        public ValidationRequest(IEnumerable<ValidationRequestEntry> entries, DeduplicationMode deduplication)
            : this(entries, ValidationQuality.Default, deduplication, ValidationPriority.Default)
        {
        }


        public ValidationRequest(IEnumerable<ValidationRequestEntry> entries, ValidationQuality quality, ValidationPriority priority)
            : this(entries, quality, DeduplicationMode.Default, priority)
        {
        }

        public ValidationRequest(IEnumerable<ValidationRequestEntry> entries, ValidationQuality quality, DeduplicationMode deduplication)
            : this(entries, quality, deduplication, ValidationPriority.Default)
        {
        }

        public ValidationRequest(IEnumerable<ValidationRequestEntry> entries, DeduplicationMode deduplication, ValidationPriority priority)
            : this(entries, ValidationQuality.Default, deduplication, priority)
        {
        }

        public ValidationRequest(IEnumerable<ValidationRequestEntry> entries, ValidationQuality quality, DeduplicationMode deduplication, ValidationPriority priority)
        {
            var enumeratedEntries = entries.ToArray();

            if (enumeratedEntries.Length == 0)
                throw new ArgumentException("Can't create a validation request out of an empty collection of entries.", "entries");

            Entries = enumeratedEntries;
            Quality = quality;
            Deduplication = deduplication;
            Priority = priority;
        }
    }
}