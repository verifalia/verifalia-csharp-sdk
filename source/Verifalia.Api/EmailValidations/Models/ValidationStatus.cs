/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2020 Cobisi Research
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

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// Provides enumerated values for the supported statuses for a <see cref="Validation.Overview"/>.
    /// </summary>
    public enum ValidationStatus
    {
        /// <summary>
        /// Unknown status, due to a value reported by the API which is missing in this SDK.
        /// </summary>
        Unknown,

        /// <summary>
        /// The email validation job is being processed by Verifalia.
        /// <remarks>The completion progress, if any, is available through the <see cref="ValidationOverview.Progress"/> property.</remarks>
        /// </summary>
        InProgress,

        /// <summary>
        /// The email validation job has been completed and its results are available.
        /// </summary>
        Completed,

        /// <summary>
        /// The email validation job has either been deleted.
        /// </summary>
        Deleted,

        /// <summary>
        /// The email validation job is expired.
        /// </summary>
        Expired,
    }
}
