﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Verifalia.Api.EmailAddresses.Models;
using Verifalia.Api.Exceptions;

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
            if (restClientFactory == null)
                throw new ArgumentNullException("restClientFactory", "restClientFactory is null.");

            _restClientFactory = restClientFactory;
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
            if (emailAddress == null)
                throw new ArgumentNullException("emailAddress");

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
            if (emailAddress == null)
                throw new ArgumentNullException("emailAddress");
            if (quality == null)
                throw new ArgumentNullException("quality");

            return Submit(emailAddress, quality, ResultPollingOptions.NoPolling);
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
        public Validation Submit(string emailAddress, ResultPollingOptions resultPollingOptions)
        {
            if (emailAddress == null)
                throw new ArgumentNullException("emailAddress");
            if (resultPollingOptions == null)
                throw new ArgumentNullException("resultPollingOptions");

            return Submit(emailAddress, ValidationQuality.Default, resultPollingOptions);
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
        public Validation Submit(string emailAddress, ValidationQuality quality,
            ResultPollingOptions resultPollingOptions)
        {
            if (emailAddress == null)
                throw new ArgumentNullException("emailAddress");
            if (quality == null)
                throw new ArgumentNullException("quality");
            if (resultPollingOptions == null)
                throw new ArgumentNullException("resultPollingOptions");

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
            if (emailAddresses == null)
                throw new ArgumentNullException("emailAddresses");

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
            if (emailAddresses == null)
                throw new ArgumentNullException("emailAddresses");
            if (quality == null)
                throw new ArgumentNullException("quality");

            return Submit(emailAddresses, quality, ResultPollingOptions.NoPolling);
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
            if (emailAddresses == null)
                throw new ArgumentNullException("emailAddresses");

            return Submit(emailAddresses.Select(emailAddress => new RequestEntry(emailAddress)),
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
            if (emailAddresses == null)
                throw new ArgumentNullException("emailAddresses");
            if (quality == null)
                throw new ArgumentNullException("quality");

            return Submit(emailAddresses.Select(emailAddress => new RequestEntry(emailAddress)),
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
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(RequestEntry entry)
        {
            return Submit(new[] {entry},
                ValidationQuality.Default,
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
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(RequestEntry entry, ValidationQuality quality)
        {
            return Submit(new[] {entry},
                quality,
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
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(RequestEntry entry, ResultPollingOptions resultPollingOptions)
        {
            return Submit(new[] {entry},
                ValidationQuality.Default,
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
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(RequestEntry entry, ValidationQuality quality,
            ResultPollingOptions resultPollingOptions)
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
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<RequestEntry> entries)
        {
            return Submit(entries,
                ValidationQuality.Default,
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
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<RequestEntry> entries, ResultPollingOptions resultPollingOptions)
        {
            return Submit(entries,
                ValidationQuality.Default,
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
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<RequestEntry> entries, ValidationQuality quality)
        {
            return Submit(entries,
                quality,
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
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<RequestEntry> entries, ValidationQuality quality, ResultPollingOptions resultPollingOptions)
        {
            try
            {
                return SubmitAsync(entries, quality, resultPollingOptions).Result;
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
            if (emailAddress == null)
                throw new ArgumentNullException("emailAddress");

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
            if (emailAddress == null)
                throw new ArgumentNullException("emailAddress");
            if (quality == null)
                throw new ArgumentNullException("quality");

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
            if (emailAddress == null)
                throw new ArgumentNullException("emailAddress");
            if (resultPollingOptions == null)
                throw new ArgumentNullException("resultPollingOptions");

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
            if (emailAddress == null)
                throw new ArgumentNullException("emailAddress");
            if (quality == null)
                throw new ArgumentNullException("quality");
            if (resultPollingOptions == null)
                throw new ArgumentNullException("resultPollingOptions");

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
            if (emailAddresses == null)
                throw new ArgumentNullException("emailAddresses");

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
            if (emailAddresses == null)
                throw new ArgumentNullException("emailAddresses");
            if (quality == null)
                throw new ArgumentNullException("quality");

            return SubmitAsync(emailAddresses,
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
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (emailAddresses == null)
                throw new ArgumentNullException("emailAddresses");

            return SubmitAsync(emailAddresses.Select(emailAddress => new RequestEntry(emailAddress)),
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
            if (emailAddresses == null)
                throw new ArgumentNullException("emailAddresses");
            if (quality == null)
                throw new ArgumentNullException("quality");

            return SubmitAsync(emailAddresses.Select(emailAddress => new RequestEntry(emailAddress)),
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
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(RequestEntry entry, CancellationToken cancellationToken = default(CancellationToken))
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
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(RequestEntry entry, ValidationQuality quality, CancellationToken cancellationToken = default(CancellationToken))
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
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(RequestEntry entry, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
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
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(RequestEntry entry, ValidationQuality quality, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
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
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<RequestEntry> entries, CancellationToken cancellationToken = default(CancellationToken))
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
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<RequestEntry> entries, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
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
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Task<Validation> SubmitAsync(IEnumerable<RequestEntry> entries, ValidationQuality quality, CancellationToken cancellationToken = default(CancellationToken))
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
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public async Task<Validation> SubmitAsync(IEnumerable<RequestEntry> entries, ValidationQuality quality, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            if (quality == null)
                throw new ArgumentNullException("quality");
            if (resultPollingOptions == null)
                throw new ArgumentNullException("resultPollingOptions");

            var enumeratedEntries = entries.ToArray();

            if (enumeratedEntries.Length == 0)
                throw new ArgumentException("Can't validate an empty batch.", "entries");

            // Build the REST request

            var request = new RestRequest
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Json,
                Resource = "email-validations",
                JsonSerializer = new ProgressiveJsonSerializer()
            };

            request.AddBody(new
            {
                entries = enumeratedEntries,
                quality = quality.NameOrGuid
            });

            // Send the request to the Verifalia servers

            var restClient = _restClientFactory.Build();
            var response = await restClient.ExecuteTaskAsync<Validation>(request, cancellationToken)
                .ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    // The batch has been completed in real time

                    response.Data.Status = ValidationStatus.Completed;
                    return response.Data;
                }

                case HttpStatusCode.Accepted:
                {
                    // The batch has been accepted but is not yet completed

                    if (ReferenceEquals(resultPollingOptions, ResultPollingOptions.NoPolling))
                    {
                        response.Data.Status = ValidationStatus.Pending;
                        return response.Data;
                    }

                    // Poll the service until completion

                    return await QueryAsync(response.Data.UniqueID, resultPollingOptions, cancellationToken)
                            .ConfigureAwait(false);
                }

                case HttpStatusCode.BadRequest:
                {
                    // The batch has NOT been accepted because of an issue with the request

                    throw new VerifaliaException(response.StatusDescription)
                    {
                        Response = response
                    };
                }

                case HttpStatusCode.Unauthorized:
                {
                    // The batch has NOT been accepted because of an issue with the supplied credentials

                    throw new AuthorizationException(response.ErrorMessage)
                    {
                        Response = response
                    };
                }

                case HttpStatusCode.PaymentRequired:
                {
                    // The batch has NOT been accepted because of low account credit

                    throw new InsufficientCreditException(response.ErrorMessage)
                    {
                        Response = response
                    };
                }

                default:
                {
                    // An unexpected HTTP status code has been received

                    throw new VerifaliaException(String.Format("Unexpected HTTP response: {0} {1}", (int) response.StatusCode, response.StatusDescription))
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
        /// <param name="resultPollingOptionsions">The options about waiting for the validation completion.</param>
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
        /// <param name="resultPollingOptionsions">The options about waiting for the validation completion.</param>
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
            var request = new RestRequest
            {
                Resource = "email-validations/{uniqueId}",
                JsonSerializer = new ProgressiveJsonSerializer()
            };

            request.AddUrlSegment("uniqueId", uniqueId.ToString());

            // Sends the request to the Verifalia servers

            var restClient = _restClientFactory.Build();
            var response = await restClient.ExecuteTaskAsync<Validation>(request, cancellationToken)
                .ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    response.Data.Status = ValidationStatus.Completed;
                    return response.Data;
                }

                case HttpStatusCode.Accepted:
                {
                    response.Data.Status = ValidationStatus.Pending;
                    return response.Data;
                }

                case HttpStatusCode.Gone:
                case HttpStatusCode.NotFound:
                {
                    return null;
                }

                default:
                {
                    // An unexpected HTTP status code has been received

                    throw new VerifaliaException(String.Format("Unexpected HTTP response: {0} {1}", (int) response.StatusCode, response.StatusDescription))
                    {
                        Response = response
                    };
                }
            }
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
            var request = new RestRequest
            {
                Resource = "email-validations/{uniqueId}",
                JsonSerializer = new ProgressiveJsonSerializer()
            };

            request.AddUrlSegment("uniqueId", uniqueId.ToString());
            request.Method = Method.DELETE;

            // Sends the request to the Verifalia servers

            var restClient = _restClientFactory.Build();
            var response = await restClient.ExecuteTaskAsync(request, cancellationToken)
                .ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Gone:
                    {
                        // The batch has been correctly deleted

                        return;
                    }

                default:
                    {
                        // An unexpected HTTP status code has been received

                        throw new VerifaliaException(String.Format("Unexpected HTTP response: {0} {1}", (int)response.StatusCode, response.StatusDescription))
                        {
                            Response = response
                        };
                    }
            }
        }
    }
}