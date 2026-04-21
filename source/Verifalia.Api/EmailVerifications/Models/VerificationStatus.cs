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

namespace Verifalia.Api.EmailVerifications.Models
{
    /// <summary>
    /// Provides enumerated values for the supported statuses for a <see cref="Verification.Overview"/>.
    /// </summary>
    public enum VerificationStatus
    {
        /// <summary>
        /// Unknown status, if the Verifalia API reports a value not included in this SDK.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The email verification job has been completed and its results are available.
        /// </summary>
        Completed,

        /// <summary>
        /// The email verification job has been deleted.
        /// </summary>
        Deleted,

        /// <summary>
        /// The email verification job is expired.
        /// </summary>
        Expired,
        
        /// <summary>
        /// The email verification job is currently being processed by Verifalia.
        /// <remarks>The completion progress, if any, is available through the <see cref="VerificationOverview.Progress"/> property.</remarks>
        /// </summary>
        InProgress,
    }
}
