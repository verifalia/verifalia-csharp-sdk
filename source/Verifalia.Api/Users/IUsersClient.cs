/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2026 Cobisi Research
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
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Common.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Verifalia.Api.Users.Models;

namespace Verifalia.Api.Users
{ 
    /// <summary>
    /// Enables you to create, retrieve, list, update and delete Verifalia users.
    /// </summary>
    /// <remarks>The features of this type are exposed through the <see cref="VerifaliaClient.Users">Users property</see>
    /// of <see cref="VerifaliaClient">VerifaliaClient</see>.</remarks>
    public interface IUsersClient
    {
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        /// <summary>
        /// Lists users according to the specified options. 
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An enumerable collection of <see cref="UserOverview"/> of the requested data.</returns>
        IAsyncEnumerable<UserOverview> ListAsync(UserListingOptions? options = null, CancellationToken cancellationToken = default);
#endif

        /// <summary>
        /// Retrieves the first page of users according to the specified options.
        /// </summary>
        /// <remarks>Use the <see cref="GetPageAsync(Verifalia.Api.Common.Models.ListingCursor,System.Threading.CancellationToken)"/> method to retrieve the other pages.</remarks>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="UserOverview"/> with the requested data and a potential <see cref="PagedResultMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="GetPageAsync(Verifalia.Api.Common.Models.ListingCursor,System.Threading.CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListUsersAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<UserPagedResult> GetPageAsync(UserListingOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a page of users according to the specified options.
        /// </summary>
        /// <remarks>To retrieve the first page use the <see cref="GetPageAsync(Verifalia.Api.Users.Models.UserListingOptions?,System.Threading.CancellationToken)"/> method.</remarks>
        /// <param name="cursor">The cursor to use while traversing the list of users.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="UserOverview"/> with the requested data and a potential <see cref="PagedResultMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="GetPageAsync(Verifalia.Api.Common.Models.ListingCursor,System.Threading.CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListUsersAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<UserPagedResult> GetPageAsync(ListingCursor cursor, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the details of a user, given its ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve. This value is available by way of the <see cref="UserOverview.Id"/> property of <see cref="UserOverview"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="User"/> object representing the user details.</returns>        
        Task<User?> GetAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new user in Verifalia, according to the specified details and settings.
        /// </summary>
        /// <param name="user">The settings of the new user.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="UserOverview"/> object representing the overview of the newly created user.</returns>        
        Task<UserOverview> CreateAsync(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing user, according to the specified changeset.
        /// </summary>
        /// <param name="userId">The ID of the user to update. This value is available by way of the <see cref="UserOverview.Id"/> property of <see cref="UserOverview"/>.</param>
        /// <param name="changeset">A LINQ expression tree with the changes to apply to the user.</param>
        /// <param name="ifMatch">If set, performs a conditional update and only completes the update if the specified
        /// value matches the ETag header of the user.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        Task UpdateAsync(string userId, Expression<Func<User, User>> changeset, string? ifMatch = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="userId">The ID of the user to delete. This value is available by way of the <see cref="UserOverview.Id"/> property of <see cref="UserOverview"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        Task DeleteAsync(string userId, CancellationToken cancellationToken = default);
    }
}