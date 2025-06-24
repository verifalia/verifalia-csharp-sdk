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
    public interface IClientCertificatesClient
    {
        Task<ClientCertificate> CreateAsync(string userId, byte[] certificateFile, CancellationToken cancellationToken = default);
        Task<ClientCertificate> CreateAsync(string userId, Stream certificateFile, CancellationToken cancellationToken = default);
        
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        IAsyncEnumerable<ClientCertificate> ListAsync(string userId, ClientCertificateListingOptions? options = null, CancellationToken cancellationToken = default);
#endif

#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<ClientCertificatePagedResult> GetPageAsync(string userId, ClientCertificateListingOptions? options = null, CancellationToken cancellationToken = default);

#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<ClientCertificatePagedResult> GetPageAsync(string userId, ListingCursor cursor, CancellationToken cancellationToken = default);

        Task DeleteAsync(string userId, string contactMethodId, CancellationToken cancellationToken = default);
    }
}