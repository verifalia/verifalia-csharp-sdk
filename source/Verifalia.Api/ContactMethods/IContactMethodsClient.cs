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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Common.Models;
using Verifalia.Api.ContactMethods.Models;

namespace Verifalia.Api.ContactMethods
{ 
    /// <summary>
    /// Interface for contact methods operations.
    /// </summary>
    public interface IContactMethodsClient
    {
        /// <summary>
        /// Creates a new contact method for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="contactMethod">The contact method to create.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to request the operation to cancel.</param>
        /// <returns>The created contact method.</returns>
        Task<ContactMethod> CreateAsync(string userId, ContactMethod contactMethod, CancellationToken cancellationToken = default);

        /// <summary>
        /// Activates a contact method for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="contactMethodId">The ID of the contact method to activate.</param>
        /// <param name="activationCode">The activation code for the contact method.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to request the operation to cancel.</param>
        /// <returns></returns>
        Task ActivateAsync(string userId, string contactMethodId, string activationCode, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific contact method for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="contactMethodId">The ID of the contact method to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to request the operation to cancel.</param>
        /// <returns>The retrieved contact method, or null if not found.</returns>
        Task<ContactMethod?> GetAsync(string userId, string contactMethodId, CancellationToken cancellationToken = default);

#if HAS_ASYNC_ENUMERABLE_SUPPORT
        /// <summary>
        /// Lists contact methods for the specified user according to the provided options and user permissions.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An enumerable collection of <see cref="ContactMethod"/> of the requested data.</returns>
        IAsyncEnumerable<ContactMethod> ListAsync(string userId, ContactMethodListingOptions? options = null, CancellationToken cancellationToken = default);
#endif

        /// <summary>
        /// Retrieves the first page of contact methods for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="options">Options to control the retrieval of the contact methods list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, which returns a page of contact methods in a <see cref="ContactMethodPagedResult"/>.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<ContactMethodPagedResult> GetPageAsync(string userId, ContactMethodListingOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a page of contact methods for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cursor">A cursor object containing pagination information to fetch this page.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, which returns a page of contact methods in a <see cref="ContactMethodPagedResult"/>.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<ContactMethodPagedResult> GetPageAsync(string userId, ListingCursor cursor, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a contact method for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="contactMethodId">The ID of the contact method to update.</param>
        /// <param name="changeset">A lambda expression describing the changes to be made to the contact method.</param>
        /// <param name="ifMatch">An optional ETag value that must match the current version of the contact method in order for this operation to succeed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to request the operation to cancel.</param>
        Task UpdateAsync(string userId, string contactMethodId, Expression<Func<ContactMethod, ContactMethod>> changeset, string? ifMatch = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a contact method associated with the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="contactMethodId">The ID of the contact method to delete.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to request the operation to cancel.</param>
        Task DeleteAsync(string userId, string contactMethodId, CancellationToken cancellationToken = default);
    }
}