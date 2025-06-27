/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2025 Cobisi Research
*
* Cobisi Research
* Via Della Costituzione, 31
* 35010 Vigonza
* Italy - European Union
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.ClientCertificates.Models;
using Verifalia.Api.Common.Models;

namespace Verifalia.Api.ClientCertificates
{ 
    /// <summary>
    /// Interface for client certificates operations.
    /// </summary>
    public interface IClientCertificatesClient
    {
        /// <summary>
        /// Creates a new client certificate for the specified user from a given client certificate file.
        /// </summary>
        /// <param name="userId">The ID of the Verifalia user who owns the client certificate.</param>
        /// <param name="certificateFileInfo">File information containing the client certificate file to create.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, which returns a <see cref="ClientCertificate"/>.</returns>
        /// <remarks>Supported certificate file formats are:
        /// <ul>
        /// <li>Base64-encoded format (commonly with .pem, .crt, or .cer extensions);</li>
        /// <li>Binary (DER) format (commonly with .der or .cer extensions).</li>
        /// </ul>
        ///
        /// For enhanced security and in compliance with RFC 5280, Verifalia only accepts X.509 client certificates
        /// that include the Extended Key Usage extension <b>id-kp-clientAuth</b> (OID 1.3.6.1.5.5.7.3.2).
        /// </remarks>
        Task<ClientCertificate> CreateAsync(string userId, FileInfo certificateFileInfo, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new client certificate for the specified user from a given client certificate file.
        /// </summary>
        /// <param name="userId">The ID of the Verifalia user who owns the client certificate.</param>
        /// <param name="certificateFile">A byte array containing the client certificate file.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, which returns a <see cref="ClientCertificate"/>.</returns>
        /// <remarks>Supported certificate file formats are:
        /// <ul>
        /// <li>Base64-encoded format (commonly with .pem, .crt, or .cer extensions);</li>
        /// <li>Binary (DER) format (commonly with .der or .cer extensions).</li>
        /// </ul>
        /// For enhanced security and in compliance with RFC 5280, Verifalia only accepts X.509 client certificates
        /// that include the Extended Key Usage extension <b>id-kp-clientAuth</b> (OID 1.3.6.1.5.5.7.3.2).
        /// </remarks>
        Task<ClientCertificate> CreateAsync(string userId, byte[] certificateFile, CancellationToken cancellationToken = default);


        /// <summary>
        /// Creates a new client certificate for the specified user from a given stream containing the client certificate file.
        /// </summary>
        /// <param name="userId">The ID of the Verifalia user who owns the client certificate.</param>
        /// <param name="certificateFile">A stream representing the client certificate file to create.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, which returns a <see cref="ClientCertificate"/>.</returns>
        /// <remarks>Supported certificate file formats are:
        /// <ul>
        /// <li>Base64-encoded format (commonly with .pem, .crt, or .cer extensions);</li>
        /// <li>Binary (DER) format (commonly with .der or .cer extensions).</li>
        /// </ul>
        /// For enhanced security and in compliance with RFC 5280, Verifalia only accepts X.509 client certificates
        /// that include the Extended Key Usage extension <b>id-kp-clientAuth</b> (OID 1.3.6.1.5.5.7.3.2).
        /// </remarks>
        Task<ClientCertificate> CreateAsync(string userId, Stream certificateFile, CancellationToken cancellationToken = default);
        
#if HAS_ASYNC_ENUMERABLE_SUPPORT

        /// <summary>
        /// Lists client certificates associated with the specified user.
        /// </summary>
        /// <param name="userId">The ID of the Verifalia user who owns the client certificates.</param>
        /// <param name="options">Options for filtering and sorting the client certificate list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An asynchronous enumerable sequence of <see cref="ClientCertificate"/>.</returns>
        IAsyncEnumerable<ClientCertificate> ListAsync(string userId, ClientCertificateListingOptions? options = null, CancellationToken cancellationToken = default);
#endif

        /// <summary>
        /// Retrieves the first page of client certificates for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the Verifalia user who owns the client certificates.</param>
        /// <param name="options">Options to control the retrieval of the client certificate list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, which returns a page of client certificates in a <see cref="ClientCertificatePagedResult"/>.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<ClientCertificatePagedResult> GetPageAsync(string userId, ClientCertificateListingOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a page of client certificates for the specified user, using the provided cursor.
        /// </summary>
        /// <param name="userId">The ID of the Verifalia user who owns the client certificates.</param>
        /// <param name="cursor">A cursor object containing pagination information to fetch this page.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, which returns a <see cref="ClientCertificatePagedResult"/>.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<ClientCertificatePagedResult> GetPageAsync(string userId, ListingCursor cursor, CancellationToken cancellationToken = default);


        /// <summary>
        /// Deletes a client certificate associated with the specified user.
        /// </summary>
        /// <param name="userId">The ID of the Verifalia user who owns the client certificate.</param>
        /// <param name="clientCertificateId">The ID of the client certificate to be deleted.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>This method permanently deletes the specified client certificate; once deleted, it cannot be recovered.
        /// </remarks>
        Task DeleteAsync(string userId, string clientCertificateId, CancellationToken cancellationToken = default);
    }
}