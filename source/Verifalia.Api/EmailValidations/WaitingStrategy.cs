/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2021 Cobisi Research
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
using Verifalia.Api.EmailValidations.Models;

namespace Verifalia.Api.EmailValidations
{
    /// <summary>
    /// A strategy to use while waiting for the completion of an email validation job.
    /// </summary>
    public class WaitingStrategy
    {
        /// <summary>
        /// Gets a value that controls whether to wait for the completion of an email validation job.
        /// </summary>
        public bool WaitForCompletion { get; }

        /// <summary>
        /// Gets an <see cref="IProgress{ValidationOverview}"/> instance which eventually receives completion
        /// progress updates for an email validation job.
        /// </summary>
        public IProgress<ValidationOverview> Progress { get; }

        /// <summary>
        /// Initializes a <see cref="WaitingStrategy"/>.
        /// </summary>
        public WaitingStrategy(bool waitForCompletion, IProgress<ValidationOverview> progress = null)
        {
            WaitForCompletion = waitForCompletion;
            Progress = progress;
        }

        /// <summary>
        /// Waits for the next polling interval of the specified <see cref="ValidationOverview"/>.
        /// </summary>
        /// <param name="validationOverview">The <see cref="ValidationOverview"/> for which to wait for the next polling interval.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public virtual Task WaitForNextPoll(ValidationOverview validationOverview, CancellationToken cancellationToken)
        {
            if (validationOverview == null) throw new ArgumentNullException(nameof(validationOverview));

            // TODO: For better results, consider the job age while determining the polling delay

            //var jobAge = DateTime.UtcNow - validationOverview.SubmittedOn;

            //if (jobAge < TimeSpan.Zero)
            //{
            //    // The client's clock is not in sync with the Verifalia' servers - for the sake of best performance,
            //    // let's assume the job has just been submitted.

            //    jobAge = TimeSpan.Zero;
            //}

            // Observe the ETA if we have one, otherwise a delay given the formula: max(0.5, min(30, 2^(log(noOfEntries, 10) - 1)))

            var delay = validationOverview.Progress?.EstimatedTimeRemaining ??
                   TimeSpan.FromSeconds(Math.Max(0.5, Math.Min(30, Math.Pow(2, Math.Log10(validationOverview.NoOfEntries) - 1))));

            return Task.Delay(delay, cancellationToken);
        }
    }
}