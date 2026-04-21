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

using System.Net;
using System.Text;
using Verifalia.Api.Exceptions.Models;

namespace Verifalia.Api.Exceptions
{
    /// <summary>
    /// This exception is thrown when a request to the Verifalia API returns an unexpected HTTP status code.
    /// When available, it also includes the returned RFC 9457 problem details object. 
    /// </summary>
    /// <inheritdoc />
    public class RequestFailedException : VerifaliaException
    {
        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
        
        /// <summary>
        /// The raw error message.
        /// </summary>
        public string RawErrorMessage { get; }
        
        /// <summary>
        /// The RFC 9457 problem details object returned by the Verifalia API.
        /// </summary>
        public Problem? Problem { get; }

        internal RequestFailedException(HttpStatusCode statusCode, string rawErrorMessage, Problem? problem = null)
            : base(BuildMessage(statusCode, rawErrorMessage, problem))
        {
            StatusCode = statusCode;
            RawErrorMessage = rawErrorMessage;
            Problem = problem;
        }

        private static string BuildMessage(HttpStatusCode statusCode, string rawErrorMessage, Problem? problem)
        {
            var sb = new StringBuilder();

            if (problem?.Title != null)
            {
                sb.Append($"{problem.Title}. ");

                if (problem.Detail != null)
                {
                    sb.Append($"{problem.Detail} ");
                }
                
                sb.Append($"HTTP status code: {(int) statusCode}.");
            }
            else
            {
                sb.Append($"Unexpected HTTP response. HTTP status code: {(int) statusCode}. Raw error message: {rawErrorMessage}");
            }

            return sb.ToString();
        }
    }
}