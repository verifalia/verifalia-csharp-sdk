/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2020 Cobisi Research
*
* Cobisi Research
* Via Prima Strada, 35
* 35129, Padova
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
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http.Content;

namespace Verifalia.Api
{
    internal static class HttpContentExtensions
    {
        internal static async Task<T> DeserializeAsync<T>(this HttpContent content, IRestClient restClient)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (restClient == null) throw new ArgumentNullException(nameof(restClient));

            using var stream = await content
                .ReadAsStreamAsync()
                .ConfigureAwait(false);
            
            return restClient.Deserialize<T>(stream);
        }

        internal static HttpContent Serialize(this HttpContent content, IRestClient restClient, object obj)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (restClient == null) throw new ArgumentNullException(nameof(restClient));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return new CapturedJsonContent(restClient.Serialize(obj));
        }
    }
}