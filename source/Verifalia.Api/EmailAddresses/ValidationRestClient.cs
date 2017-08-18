using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.EmailAddresses.Models;
using Verifalia.Api.Exceptions;
using System.Net.Http;

namespace Verifalia.Api.EmailAddresses
{
    /// <summary>
    /// Allows to submit and manage email validations using the Verifalia service.
    /// <remarks>The functionalities of this type are exposed by way of the <see cref="VerifaliaRestClient.EmailValidations">EmailValidations property</see>
    /// of <see cref="VerifaliaRestClient">VerifaliaRestClient</see>.</remarks>
    /// </summary>
    internal class ValidationRestClient : IValidationRestClient
    {
        readonly IRestClientFactory _restClientFactory;

        internal ValidationRestClient(IRestClientFactory restClientFactory)
        {
            _restClientFactory = restClientFactory ?? throw new ArgumentNullException(nameof(restClientFactory));
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="Submit(IEnumerable{string}, ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(string emailAddress)
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));

            return Submit(emailAddress, ValidationQuality.Default, ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="Submit(IEnumerable{string}, ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(string emailAddress, ValidationQuality quality)
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            if (quality == null) throw new ArgumentNullException(nameof(quality));

            return Submit(emailAddress, quality, ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="resultPollingOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(string emailAddress, ResultPollingOptions resultPollingOptions)
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            if (resultPollingOptions == null) throw new ArgumentNullException(nameof(resultPollingOptions));

            return Submit(emailAddress, ValidationQuality.Default, resultPollingOptions);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="resultPollingOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(string emailAddress, ValidationQuality quality, ResultPollingOptions resultPollingOptions)
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            if (quality == null) throw new ArgumentNullException(nameof(quality));
            if (resultPollingOptions == null) throw new ArgumentNullException(nameof(resultPollingOptions));

            return Submit(new[] {emailAddress}, quality, resultPollingOptions);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="Submit(IEnumerable{string}, ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<string> emailAddresses)
        {
            if (emailAddresses == null) throw new ArgumentNullException(nameof(emailAddresses));

            return Submit(emailAddresses, ValidationQuality.Default, ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="Submit(IEnumerable{string}, ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<string> emailAddresses, ValidationQuality quality)
        {
            if (emailAddresses == null) throw new ArgumentNullException(nameof(emailAddresses));
            if (quality == null) throw new ArgumentNullException(nameof(quality));

            return Submit(emailAddresses, quality, ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="Submit(IEnumerable{string}, ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<string> emailAddresses, DeduplicationMode deduplicationMode)
        {
            if (emailAddresses == null) throw new ArgumentNullException(nameof(emailAddresses));

            return Submit(emailAddresses,
                ValidationQuality.Default,
                deduplicationMode,
                ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<string> emailAddresses, ResultPollingOptions resultPollingOptions)
        {
            if (emailAddresses == null) throw new ArgumentNullException(nameof(emailAddresses));

            return Submit(emailAddresses.Select(emailAddress => new ValidationRequestEntry(emailAddress)),
                ValidationQuality.Default, resultPollingOptions);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<string> emailAddresses, ValidationQuality quality, ResultPollingOptions resultPollingOptions)
        {
            return Submit(emailAddresses,
                quality,
                DeduplicationMode.Default,
                resultPollingOptions);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<string> emailAddresses, ValidationQuality quality, DeduplicationMode deduplicationMode, ResultPollingOptions resultPollingOptions)
        {
            if (emailAddresses == null) throw new ArgumentNullException(nameof(emailAddresses));
            if (quality == null) throw new ArgumentNullException(nameof(quality));
            if (deduplicationMode == null) throw new ArgumentNullException(nameof(deduplicationMode));

            return Submit(emailAddresses.Select(emailAddress => new ValidationRequestEntry(emailAddress)),
                quality,
                deduplicationMode,
                resultPollingOptions);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="ValidationRequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(ValidationRequestEntry entry)
        {
            return Submit(new[] {entry},
                ValidationQuality.Default,
                DeduplicationMode.Default,
                ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="ValidationRequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(ValidationRequestEntry entry, ValidationQuality quality)
        {
            return Submit(new[] {entry},
                quality,
                DeduplicationMode.Default,
                ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="ValidationRequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(ValidationRequestEntry entry, ResultPollingOptions resultPollingOptions)
        {
            return Submit(new[] {entry},
                ValidationQuality.Default,
                DeduplicationMode.Default,
                resultPollingOptions);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="ValidationRequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(ValidationRequestEntry entry, ValidationQuality quality, ResultPollingOptions resultPollingOptions)
        {
            return Submit(new[] {entry},
                quality,
                resultPollingOptions);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<ValidationRequestEntry> entries)
        {
            return Submit(entries,
                ValidationQuality.Default,
                DeduplicationMode.Default,
                ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<ValidationRequestEntry> entries, ResultPollingOptions resultPollingOptions)
        {
            return Submit(entries,
                ValidationQuality.Default,
                DeduplicationMode.Default,
                resultPollingOptions);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<ValidationRequestEntry> entries, ValidationQuality quality)
        {
            return Submit(entries,
                quality,
                DeduplicationMode.Default,
                ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<ValidationRequestEntry> entries, ValidationQuality quality, ResultPollingOptions resultPollingOptions)
        {
            return Submit(entries,
                quality,
                DeduplicationMode.Default,
                ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<ValidationRequestEntry> entries, ValidationQuality quality, DeduplicationMode deduplicationMode, ResultPollingOptions resultPollingOptions)
        {
            return Submit(new ValidationRequest(entries, quality, deduplicationMode), resultPollingOptions);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(ValidationRequest request)
        {
            return Submit(request, ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(ValidationRequest request, ResultPollingOptions resultPollingOptions)
        {
            try
            {
                return SubmitAsync(request, resultPollingOptions).Result;
            }
            catch (AggregateException aggregateException)
            {
                var flattenException = aggregateException.Flatten();

                foreach (var innerException in flattenException.InnerExceptions)
                {
                    if (innerException is OperationCanceledException)
                    {
                        throw innerException;
                    }

                    if (innerException is VerifaliaException)
                    {
                        throw innerException;
                    }
                }

                throw new VerifaliaException("One or more errors occurred during the operation; please see the inner exception for further details.", aggregateException);
            }
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="Submit(IEnumerable{string}, ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(string emailAddress, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));

            return SubmitAsync(emailAddress,
                ValidationQuality.Default,
                ResultPollingOptions.NoPolling,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="Submit(IEnumerable{string}, ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> Submit(string emailAddress, ValidationQuality quality, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            if (quality == null) throw new ArgumentNullException(nameof(quality));

            return SubmitAsync(emailAddress,
                quality,
                ResultPollingOptions.NoPolling,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(string emailAddress, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            if (resultPollingOptions == null) throw new ArgumentNullException(nameof(resultPollingOptions));

            return SubmitAsync(emailAddress,
                ValidationQuality.Default,
                resultPollingOptions,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(string emailAddress, ValidationQuality quality, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            if (quality == null) throw new ArgumentNullException(nameof(quality));
            if (resultPollingOptions == null) throw new ArgumentNullException(nameof(resultPollingOptions));

            return SubmitAsync(new[] { emailAddress },
                quality,
                resultPollingOptions,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="Submit(IEnumerable{string}, ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (emailAddresses == null) throw new ArgumentNullException(nameof(emailAddresses));

            return SubmitAsync(emailAddresses,
                ValidationQuality.Default,
                ResultPollingOptions.NoPolling,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="Submit(IEnumerable{string}, ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, ValidationQuality quality, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (emailAddresses == null) throw new ArgumentNullException(nameof(emailAddresses));
            if (quality == null) throw new ArgumentNullException(nameof(quality));

            return SubmitAsync(emailAddresses,
                quality,
                ResultPollingOptions.NoPolling,
                cancellationToken);
        }

        public Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, ValidationQuality quality, DeduplicationMode deduplicationMode, CancellationToken cancellationToken = new CancellationToken())
        {
            return SubmitAsync(emailAddresses,
                quality,
                deduplicationMode,
                ResultPollingOptions.NoPolling,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (emailAddresses == null) throw new ArgumentNullException(nameof(emailAddresses));

            return SubmitAsync(emailAddresses.Select(emailAddress => new ValidationRequestEntry(emailAddress)),
                ValidationQuality.Default,
                resultPollingOptions,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, ValidationQuality quality, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(emailAddresses,
                quality,
                DeduplicationMode.Default,
                resultPollingOptions,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, ValidationQuality quality, DeduplicationMode deduplicationMode, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (emailAddresses == null) throw new ArgumentNullException(nameof(emailAddresses));
            if (quality == null) throw new ArgumentNullException(nameof(quality));

            return SubmitAsync(emailAddresses.Select(emailAddress => new ValidationRequestEntry(emailAddress)),
                quality,
                deduplicationMode,
                resultPollingOptions,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="ValidationRequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(ValidationRequestEntry entry, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(new[] { entry },
                ValidationQuality.Default,
                ResultPollingOptions.NoPolling,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="ValidationRequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(ValidationRequestEntry entry, ValidationQuality quality, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(new[] { entry },
                quality,
                ResultPollingOptions.NoPolling,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="ValidationRequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(ValidationRequestEntry entry, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(new[] { entry },
                ValidationQuality.Default,
                resultPollingOptions,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="ValidationRequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(ValidationRequestEntry entry, ValidationQuality quality, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(new[] { entry },
                quality,
                resultPollingOptions,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<ValidationRequestEntry> entries, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(entries,
                ValidationQuality.Default,
                ResultPollingOptions.NoPolling,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<ValidationRequestEntry> entries, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(entries,
                ValidationQuality.Default,
                resultPollingOptions,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<ValidationRequestEntry> entries, ValidationQuality quality, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(entries,
                quality,
                ResultPollingOptions.NoPolling,
                cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="deduplicationMode">The desired deduplication mode for the submitted job.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<ValidationRequestEntry> entries, ValidationQuality quality, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(new ValidationRequest(entries, quality, DeduplicationMode.Default), resultPollingOptions, cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="deduplicationMode">The desired deduplication mode for the submitted job.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<ValidationRequestEntry> entries, ValidationQuality quality, DeduplicationMode deduplicationMode, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(new ValidationRequest(entries, quality, deduplicationMode), resultPollingOptions, cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="ValidationRequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="deduplicationMode">The desired deduplication mode for the submitted job.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<ValidationRequestEntry> entries, DeduplicationMode deduplicationMode, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SubmitAsync(new ValidationRequest(entries, ValidationQuality.Default, deduplicationMode), resultPollingOptions, cancellationToken);
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="validationRequest">Represents a validation request, along with the email addresses to validate, the desired result quality level, duplicates removal algorithm and priority.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public async Task<Validation> SubmitAsync(ValidationRequest validationRequest, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (validationRequest == null) throw new ArgumentNullException(nameof(validationRequest));
            if (resultPollingOptions == null) throw new ArgumentNullException(nameof(resultPollingOptions));

            // Generate the additional parameters, where needed

            var restClient = _restClientFactory.Build();

            var content = restClient
                .SerializeContent(new
                {
                    entries = validationRequest.Entries,
                    quality = validationRequest.Quality == ValidationQuality.Default
                        ? null
                        : validationRequest.Quality.NameOrGuid,
                    deduplication = validationRequest.Deduplication == DeduplicationMode.Default
                        ? null
                        : validationRequest.Deduplication.NameOrGuid,
                    priority = validationRequest.Priority == ValidationPriority.Default
                        ? (byte?)null
                        : validationRequest.Priority.Value
                });

            // Send the request to the Verifalia servers

            var response = await restClient.InvokeAsync(HttpMethod.Post,
                    "email-validations",
                    content,
                    cancellationToken)
                .ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Accepted:
                {
                        var data = await restClient
                            .DeserializeContentAsync<Validation>(response)
                            .ConfigureAwait(false);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // The batch has been completed in real-time

                            data.Status = ValidationStatus.Completed;
                        }
                        else
                        {
                            // The batch has been accepted but is not yet completed

                            if (ReferenceEquals(resultPollingOptions, ResultPollingOptions.NoPolling))
                            {
                                data.Status = ValidationStatus.Pending;
                            }
                            else
                            {
                                // Poll the service until completion

                                return await QueryAsync(data.UniqueID,
                                        resultPollingOptions,
                                        cancellationToken)
                                    .ConfigureAwait(false);
                            }
                        }

                        return data;
                }

                default:
                {
                    var responseBody = await response
                        .Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false);

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            {
                                // The batch has NOT been accepted because of an issue with the request

                                throw new VerifaliaException(String.Format("Unexpected HTTP response: {0} {1}", (int)response.StatusCode, responseBody))
                                {
                                    Response = response
                                };
                            }

                        case HttpStatusCode.Unauthorized:
                            {
                                // The batch has NOT been accepted because of an issue with the supplied credentials

                                throw new AuthorizationException(responseBody)
                                {
                                    Response = response
                                };
                            }

                        case HttpStatusCode.PaymentRequired:
                            {
                                // The batch has NOT been accepted because of low account credit

                                throw new InsufficientCreditException(responseBody)
                                {
                                    Response = response
                                };
                            }
                    }

                    // An unexpected HTTP status code has been received at this point

                    throw new VerifaliaException(String.Format("Unexpected HTTP response: {0} {1}", (int) response.StatusCode, responseBody))
                    {
                        Response = response
                    };
                }
            }
        }

        /// <summary>
        /// Returns an object representing an email validation batch, identified by the specified unique identifier.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="Submit(IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        public Validation Query(Guid uniqueId)
        {
            return Query(uniqueId, ResultPollingOptions.NoPolling);
        }

        /// <summary>
        /// Returns an object representing an email validation batch, waiting for its completion and issuing multiple retries if needed.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="Submit(IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <param name="resultPollingOptions">The options about waiting for the validation completion.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        public Validation Query(Guid uniqueId, ResultPollingOptions resultPollingOptions)
        {
            try
            {
                return QueryAsync(uniqueId, resultPollingOptions).Result;
            }
            catch (AggregateException aggregateException)
            {
                var flattenException = aggregateException.Flatten();

                foreach (var innerException in flattenException.InnerExceptions)
                {
                    if (innerException is OperationCanceledException)
                    {
                        throw innerException;
                    }

                    if (innerException is VerifaliaException)
                    {
                        throw innerException;
                    }
                }

                throw new VerifaliaException("One or more errors occurred during the operation; please see the inner exception for further details.", aggregateException);
            }
        }

        /// <summary>
        /// Returns an object representing an email validation batch, identified by the specified unique identifier.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="Submit(IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        public Task<Validation> QueryAsync(Guid uniqueId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return QueryAsync(uniqueId, ResultPollingOptions.NoPolling, cancellationToken);
        }

        /// <summary>
        /// Returns an object representing an email validation batch, waiting for its completion and issuing multiple retries if needed.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="Submit(IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <param name="resultPollingOptions">The options about waiting for the validation completion.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        public async Task<Validation> QueryAsync(Guid uniqueId, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Wait for completion

            Validation result = null;

            for (var pollingIteration = 0; pollingIteration <= resultPollingOptions.MaxPollingCount; pollingIteration++)
            {
                result = await QueryOnceAsync(uniqueId, cancellationToken)
                    .ConfigureAwait(false);

                // A null result means the validation has not been found

                if (result == null)
                {
                    return null;
                }

                // Returns immediately if the validation has been completed

                if (result.Status == ValidationStatus.Completed)
                {
                    return result;
                }

                // Wait for the polling interval

                await Task.Delay(resultPollingOptions.PollingInterval, cancellationToken)
                    .ConfigureAwait(false);
            }

            // At this point, we have exhausted the configured maximum number of allowed polling iterations

            if (resultPollingOptions.ThrowIfNotCompleted)
            {
                throw new UncompletedBatchException("Could not complete the validation job within the configured polling iterations limit.");
            }

            return result;
        }

        /// <summary>
        /// Returns an object representing an email validation batch, identified by the specified unique identifier.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="Submit(IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        async Task<Validation> QueryOnceAsync(Guid uniqueId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resource = String.Format("email-validations/{0}", uniqueId);

            // Sends the request to the Verifalia servers

            var restClient = _restClientFactory.Build();
            var response = await restClient
                .InvokeAsync(HttpMethod.Get, resource, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Accepted:
                {
                    var data = await restClient
                            .DeserializeContentAsync<Validation>(response)
                            .ConfigureAwait(false);

                    data.Status = response.StatusCode == HttpStatusCode.Accepted ?
                            ValidationStatus.Pending :
                            ValidationStatus.Completed;

                    return data;
                }

                case HttpStatusCode.Gone:
                case HttpStatusCode.NotFound:
                {
                    return null;
                }
            }

            // An unexpected HTTP status code has been received at this point

            var responseBody = await response
                .Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            throw new VerifaliaException(String.Format("Unexpected HTTP response: {0} {1}", (int)response.StatusCode, responseBody))
            {
                Response = response
            };
        }

        /// <summary>
        /// Deletes an email validation batch, identified by the specified unique identifier.
        /// Makes a DELETE request to the /email-validations/{uniqueId} resource.
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be deleted.</param>
        public void Delete(Guid uniqueId)
        {
            try
            {
                DeleteAsync(uniqueId).Wait();
            }
            catch (AggregateException aggregateException)
            {
                var flattenException = aggregateException.Flatten();

                foreach (var innerException in flattenException.InnerExceptions)
                {
                    if (innerException is OperationCanceledException)
                    {
                        throw innerException;
                    }

                    if (innerException is VerifaliaException)
                    {
                        throw innerException;
                    }
                }

                throw new VerifaliaException("One or more errors occurred during the operation; please see the inner exception for further details.", aggregateException);
            }
        }

        /// <summary>
        /// Deletes an email validation batch, identified by the specified unique identifier.
        /// Makes a DELETE request to the /email-validations/{uniqueId} resource.
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be deleted.</param>
        public async Task DeleteAsync(Guid uniqueId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resource = String.Format("email-validations/{0}", uniqueId);

            // Sends the request to the Verifalia servers

            var restClient = _restClientFactory.Build();
            var response = await restClient
                .InvokeAsync(HttpMethod.Delete, resource, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Gone:
                    {
                        // The batch has been correctly deleted

                        return;
                    }
            }

            // An unexpected HTTP status code has been received at this point

            var responseBody = await response
                .Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            throw new VerifaliaException(String.Format("Unexpected HTTP response: {0} {1}", (int)response.StatusCode, responseBody))
            {
                Response = response
            };
        }
    }
}