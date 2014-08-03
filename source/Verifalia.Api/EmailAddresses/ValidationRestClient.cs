using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class ValidationRestClient
    {
        readonly RestClient _restClient;

        internal ValidationRestClient(RestClient restClient)
        {
            if (restClient == null)
                throw new ArgumentNullException("restClient", "restClient is null.");

            _restClient = restClient;
        }

        /// <summary>
        /// Initiates a new email validation batch. Makes a POST request to the /email-validations resource.
        /// <remarks>Upon initialization, batches usually are in the <see cref="ValidationStatus.Pending">Pending</see> status.
        /// Validations are completed only when their <see cref="Validation.Status">Status</see> property
        /// is <see cref="ValidationStatus.Completed">Completed</see>. Use the <see cref="Submit(IEnumerable{string}, WaitForCompletionOptions)"/> overload
        /// to wait for the completion of the batch without having to manually poll the API.
        /// In order to retrieve the most up-to-date snapshot of a validation batch, call the <see cref="Query(Guid)">Query() method</see>
        /// along with the batch's <see cref="Validation.UniqueID">unique identifier</see>.
        /// </remarks>
        /// </summary>
        /// <param name="emailAddresses">A collection of email addresses to validate.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<string> emailAddresses)
        {
            return Submit(emailAddresses, WaitForCompletionOptions.DontWait);
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
        /// <param name="waitForCompletionOptions">The options about waiting for the validation completion.</param>
        /// <returns>An object representing the email validation batch.</returns>
        public Validation Submit(IEnumerable<string> emailAddresses, WaitForCompletionOptions waitForCompletionOptions)
        {
            if (emailAddresses == null)
                throw new ArgumentNullException("emailAddresses");
            if (waitForCompletionOptions == null)
                throw new ArgumentNullException("waitForCompletionOptions");

            var requestEntries = emailAddresses
                .Select(emailAddress => new
                {
                    inputData = emailAddress
                }).ToArray();

            if (requestEntries.Length == 0)
                throw new ArgumentException("Can't validate an empty batch.", "emailAddresses");

            // Build the REST request

            var request = new RestRequest
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Json,
                Resource = "email-validations"
            };

            request.AddBody(new
            {
                entries = requestEntries
            });

            // Send the request to the Verifalia servers

            var response = _restClient.Execute<Validation>(request);

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

                    if (ReferenceEquals(waitForCompletionOptions, WaitForCompletionOptions.DontWait))
                    {
                        response.Data.Status = ValidationStatus.Pending;
                        return response.Data;
                    }
                    
                    // Poll the service until completion

                    return Query(response.Data.UniqueID, waitForCompletionOptions);
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
                    // An unhandled exception happened at the Verifalia side

                    throw new VerifaliaException(response.ErrorMessage)
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
            return Query(uniqueId, WaitForCompletionOptions.DontWait);
        }

        /// <summary>
        /// Returns an object representing an email validation batch, waiting for its completion and issuing multiple retries if needed.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="Submit(IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <param name="waitForCompletionOptions">The options about waiting for the validation completion.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        public Validation Query(Guid uniqueId, WaitForCompletionOptions waitForCompletionOptions)
        {
            // Handle the case when the client wishes to avoid waiting for completion

            if (waitForCompletionOptions == WaitForCompletionOptions.DontWait)
                return QueryOnce(uniqueId);

            // The client needs to wait for completion

            var pollingTask = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var result = QueryOnce(uniqueId);

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

                    Thread.Sleep(waitForCompletionOptions.PollingInterval);
                }
            });

            // Waits for the request completion or for the timeout to expire

            pollingTask.Wait(waitForCompletionOptions.Timeout);

            // Handles any eventual exception

            if (pollingTask.IsFaulted)
            {
                Debug.Assert(pollingTask.Exception != null, "pollingTask.Exception != null");

                if (pollingTask.Exception.InnerException != null)
                    throw pollingTask.Exception.InnerException;

                throw pollingTask.Exception;
            }

            if (pollingTask.IsCompleted)
            {
                return pollingTask.Result;
            }

            // A timeout occurred

            return null;
        }

        /// <summary>
        /// Returns an object representing an email validation batch, identified by the specified unique identifier.
        /// Makes a GET request to the /email-validations/{uniqueId} resource.
        /// <remarks>To initiate a new email validation batch, please use <see cref="Submit(IEnumerable{string})" />.</remarks>
        /// </summary>
        /// <param name="uniqueId">The unique identifier for an email validation batch to be retrieved.</param>
        /// <returns>An object representing the current status of the requested email validation batch.</returns>
        Validation QueryOnce(Guid uniqueId)
        {
            var request = new RestRequest
            {
                Resource = "email-validations/{uniqueId}"
            };

            request.AddUrlSegment("uniqueId", uniqueId.ToString());

            // Sends the request to the Verifalia servers

            var response = _restClient.Execute<Validation>(request);

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
            }
            
            throw new VerifaliaException(response.ErrorMessage)
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
            var request = new RestRequest
            {
                Resource = "email-validations/{uniqueId}"
            };

            request.AddUrlSegment("uniqueId", uniqueId.ToString());
            request.Method = Method.DELETE;

            // Sends the request to the Verifalia servers

            var response = _restClient.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // The batch has been correctly deleted

                return;
            }

            throw new VerifaliaException(response.ErrorMessage)
            {
                Response = response
            };
        }
    }
}