using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Verifalia.Api.Exceptions;
using Verifalia.Api.Models;

namespace Verifalia.Api
{
    public partial class VerifaliaRestClient
    {
        EmailValidationsRestClient _EmailValidations;
        
        /// <summary>
        /// Allows to submit and manage email validations using the Verifalia service.
        /// </summary>
        public EmailValidationsRestClient EmailValidations
        {
            get
            {
                if (_EmailValidations == null)
                {
                    _EmailValidations = new EmailValidationsRestClient(_RestClient);
                }

                return _EmailValidations;
            }
        }

        /// <summary>
        /// Allows to submit and manage email validations using the Verifalia service.
        /// <remarks>The functionalities of this type are exposed by way of the <see cref="VerifaliaRestClient.EmailValidations">EmailValidations property</see>
        /// of <see cref="VerifaliaRestClient">VerifaliaRestClient</see>.</remarks>
        /// </summary>
        public class EmailValidationsRestClient
        {
            /// <summary>
            /// Default interval between subsequent polling requests.
            /// </summary>
            readonly TimeSpan DEFAULT_GET_POLLING_INTERVAL = TimeSpan.FromSeconds(5);

            /// <summary>
            /// Default retrieval timeout for the entire polling operation.
            /// </summary>
            readonly TimeSpan DEFAULT_GET_TIMEOUT = TimeSpan.MaxValue;

            readonly RestClient _RestClient;

            internal EmailValidationsRestClient(RestClient restClient)
            {
                if (restClient == null)
                    throw new ArgumentNullException("restClient", "restClient is null.");

                _RestClient = restClient;
            }

            /// <summary>
            /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
            /// <remarks>Upon initialization, batches usually are in the <see cref="EmailValidationStatus.Pending">Pending</see> status.
            /// Validations are completed only when their <see cref="EmailValidation.Status">Status</see> property
            /// is <see cref="EmailValidationStatus.Completed">Completed</see>.
            /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="QueryResults()">QueryResults() method</see>
            /// along with the batch's <see cref="EmailValidation.UniqueId">unique identifier</see>.
            /// </remarks>
            /// </summary>
            /// <param name="emailAddresses">A collection of email addresses to validate.</param>
            /// <returns>An object representing the email validation batch.</returns>
            public EmailValidation Initiate(IEnumerable<string> emailAddresses)
            {
                // Build the REST request

                var request = new RestRequest();

                request.Method = Method.POST;
                request.RequestFormat = DataFormat.Json;
                request.Resource = "email-validations";

                request.AddBody(new
                {
                    entries = emailAddresses.Select(emailAddress => new
                    {
                        inputData = emailAddress
                    }).ToArray()
                });

                // Send the request to the Verifalia servers

                var response = _RestClient.Execute<EmailValidation>(request);

                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    response.Data.Status = EmailValidationStatus.Pending;
                    return response.Data;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new AuthorizationException(response.ErrorMessage)
                    {
                        Response = response
                    };
                }
                else if (response.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    throw new InsufficientCreditException(response.ErrorMessage)
                    {
                        Response = response
                    };
                }
                else
                {
                    throw new VerifaliaException(response.ErrorMessage)
                    {
                        Response = response
                    };
                }
            }

            /// <summary>
            /// Returns an object representing an email validation batch, identified by the specified unique identifier.
            /// Makes a GET request to the /email-validations/{uniqueId} resource.
            /// <remarks>To initiate a new email validation batch, please use <see cref="Initiate()">Initiate()</see>.</remarks>
            /// </summary>
            /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
            /// <returns>An object representing the current status of the requested email validation batch.</returns>
            public EmailValidation Get(Guid uniqueId)
            {
                var request = new RestRequest();

                request.Resource = "email-validations/{uniqueId}";
                request.AddUrlSegment("uniqueId", uniqueId.ToString());

                // Sends the request to the Verifalia servers

                var response = _RestClient.Execute<EmailValidation>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.Data.Status = EmailValidationStatus.Completed;
                    return response.Data;
                }
                else if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    response.Data.Status = EmailValidationStatus.Pending;
                    return response.Data;
                }
                else
                {
                    throw new VerifaliaException(response.ErrorMessage)
                    {
                        Response = response
                    };
                }
            }

            /// <summary>
            /// Returns an object representing an email validation batch, waiting for its completion and issuing multiple retries if needed.
            /// Makes a GET request to the /email-validations/{uniqueId} resource.
            /// <remarks>To initiate a new email validation batch, please use <see cref="Initiate()">Initiate()</see>.</remarks>
            /// </summary>
            /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
            /// <returns>An object representing the current status of the requested email validation batch.</returns>
            public EmailValidation GetOnceCompleted(Guid uniqueId)
            {
                return GetOnceCompleted(uniqueId,
                    DEFAULT_GET_TIMEOUT,
                    DEFAULT_GET_POLLING_INTERVAL);
            }

            /// <summary>
            /// Returns an object representing an email validation batch, waiting for its completion and issuing multiple retries if needed.
            /// Makes a GET request to the /email-validations/{uniqueId} resource.
            /// <remarks>To initiate a new email validation batch, please use <see cref="Initiate()">Initiate()</see>.</remarks>
            /// </summary>
            /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
            /// <param name="timeout">A timeout for the entire operation.</param>
            /// <returns>An object representing the current status of the requested email validation batch.</returns>
            public EmailValidation GetOnceCompleted(Guid uniqueId, TimeSpan timeout)
            {
                return GetOnceCompleted(uniqueId,
                    timeout,
                    DEFAULT_GET_POLLING_INTERVAL);
            }

            /// <summary>
            /// Returns an object representing an email validation batch, waiting for its completion and issuing multiple retries if needed.
            /// Makes a GET request to the /email-validations/{uniqueId} resource.
            /// <remarks>To initiate a new email validation batch, please use <see cref="Initiate()">Initiate()</see>.</remarks>
            /// </summary>
            /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
            /// <param name="timeout">A timeout for the entire operation.</param>
            /// <param name="pollingInterval">An interval to obey between each subsequent polling request.</param>
            /// <returns>An object representing the current status of the requested email validation batch.</returns>
            public EmailValidation GetOnceCompleted(Guid uniqueId, TimeSpan timeout, TimeSpan pollingInterval)
            {
                var pollingTask = Task.Factory.StartNew<EmailValidation>(() =>
                {
                    while (true)
                    {
                        var result = Get(uniqueId);

                        if (result.Status == EmailValidationStatus.Completed)
                        {
                            return result;
                        }
                        else
                        {
                            // Wait for the polling interval

                            Thread.Sleep(pollingInterval);
                        }
                    }
                });

                // Waits for the request completion or for the timeout to expire

                pollingTask.Wait(timeout);

                // Handles any eventual exception

                if (pollingTask.IsFaulted)
                {
                    throw pollingTask.Exception.InnerException;
                }

                if (pollingTask.IsCompleted)
                {
                    return pollingTask.Result;
                }

                // A timeout occurred

                return null;
            }
        }
    }
}
