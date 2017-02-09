using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.EmailAddresses.Models;

namespace Verifalia.Api.EmailAddresses
{
    public interface IValidationRestClient
    {
        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string},Verifalia.Api.ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(string emailAddress);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string},Verifalia.Api.ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(string emailAddress, ValidationQuality quality);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(string emailAddress, ResultPollingOptions resultPollingOptions);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(string emailAddress, ValidationQuality quality,
            ResultPollingOptions resultPollingOptions);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string},Verifalia.Api.ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(IEnumerable<string> emailAddresses);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string},Verifalia.Api.ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(IEnumerable<string> emailAddresses, ValidationQuality quality);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(IEnumerable<string> emailAddresses, ResultPollingOptions resultPollingOptions);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(IEnumerable<string> emailAddresses, ValidationQuality quality, ResultPollingOptions resultPollingOptions);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(RequestEntry entry);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(RequestEntry entry, ValidationQuality quality);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(RequestEntry entry, ResultPollingOptions resultPollingOptions);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(RequestEntry entry, ValidationQuality quality,
            ResultPollingOptions resultPollingOptions);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(IEnumerable<RequestEntry> entries);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(IEnumerable<RequestEntry> entries, ResultPollingOptions resultPollingOptions);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(IEnumerable<RequestEntry> entries, ValidationQuality quality);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Validation Submit(IEnumerable<RequestEntry> entries, ValidationQuality quality, ResultPollingOptions resultPollingOptions);

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string},Verifalia.Api.ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(string emailAddress, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string},Verifalia.Api.ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> Submit(string emailAddress, ValidationQuality quality, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(string emailAddress, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(string emailAddress, ValidationQuality quality, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string},Verifalia.Api.ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string},Verifalia.Api.ResultPollingOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, ValidationQuality quality, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, ValidationQuality quality, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(RequestEntry entry, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(RequestEntry entry, ValidationQuality quality, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(RequestEntry entry, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entry">A <see cref="RequestEntry">RequestEntry</see> instance, representing the email address to validate and its optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(RequestEntry entry, ValidationQuality quality, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(IEnumerable<RequestEntry> entries, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(IEnumerable<RequestEntry> entries, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(IEnumerable<RequestEntry> entries, ValidationQuality quality, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>; the <seealso cref="waitForCompletionOptions"/> parameter
        /// allows to wait for the completion of the batch, without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="ValidationRestClient.Query(System.Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="entries">A collection of <see cref="RequestEntry">RequestEntry</see> instances, representing the email addresses to validate and their optional custom data.</param>
        /// <param name="quality">The desired quality of the results for this submission.</param>
        /// <param name="resultPollingOptions">The options which rule out how to wait for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        Task<Validation> SubmitAsync(IEnumerable<RequestEntry> entries, ValidationQuality quality, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns an object representing an email validation batch, identified by the specified unique identifier.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        Validation Query(Guid uniqueId);

        /// <summary>
        /// Returns an object representing an email validation batch, waiting for its completion and issuing multiple retries if needed.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <param name="resultPollingOptionsions">The options about waiting for the validation completion.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        Validation Query(Guid uniqueId, ResultPollingOptions resultPollingOptions);

        /// <summary>
        /// Returns an object representing an email validation batch, identified by the specified unique identifier.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        Task<Validation> QueryAsync(Guid uniqueId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns an object representing an email validation batch, waiting for its completion and issuing multiple retries if needed.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="ValidationRestClient.Submit(System.Collections.Generic.IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <param name="resultPollingOptionsions">The options about waiting for the validation completion.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        Task<Validation> QueryAsync(Guid uniqueId, ResultPollingOptions resultPollingOptions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes an email validation batch, identified by the specified unique identifier.
        /// Makes a DELETE request to the /email-validations/{uniqueId} resource.
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be deleted.</param>
        void Delete(Guid uniqueId);

        /// <summary>
        /// Deletes an email validation batch, identified by the specified unique identifier.
        /// Makes a DELETE request to the /email-validations/{uniqueId} resource.
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be deleted.</param>
        Task DeleteAsync(Guid uniqueId, CancellationToken cancellationToken = default(CancellationToken));
    }
}