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

using System;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.EmailValidations.Models;

namespace Verifalia.Api.EmailValidations
{
    internal partial class EmailValidationsRestClient
    {
        private async Task<TResult> WaitForCompletionAsync<TResult>(ValidationOverview validationOverview, WaitingStrategy waitingStrategy, CancellationToken cancellationToken) where TResult : class
        {
            if (validationOverview == null) throw new ArgumentNullException(nameof(validationOverview));
            if (waitingStrategy == null) throw new ArgumentNullException(nameof(waitingStrategy));

            var resultOverview = validationOverview;

            do
            {
                // Fires a progress, since we are not yet completed

                waitingStrategy.Progress?.Report(resultOverview);

                // Wait for the next polling schedule

                await waitingStrategy
                    .WaitForNextPoll(resultOverview, cancellationToken)
                    .ConfigureAwait(false);

                // Fetch the job from the API

                TResult result;

                if (typeof(TResult) == typeof(Validation))
                {
                    var validationResult = await GetAsync(validationOverview.Id,
                            cancellationToken: cancellationToken)
                        .ConfigureAwait(false);

                    result = (TResult)(object)validationResult;

                    if (result == null)
                    {
                        // A null result means the validation has been deleted (or is expired) between a poll and the next one

                        return null;
                    }

                    resultOverview = validationResult.Overview;
                }
                else if (typeof(TResult) == typeof(ValidationOverview))
                {
                    resultOverview = await GetOverviewAsync(validationOverview.Id,
                            cancellationToken: cancellationToken)
                        .ConfigureAwait(false);

                    // A null result means the validation has been deleted (or is expired) between a poll and the next one

                    if (resultOverview == null)
                    {
                        return null;
                    }

                    result = (TResult)(object)resultOverview;
                }
                else
                {
                    throw new NotSupportedException("TResult must be either of type Validation or ValidationOverview.");
                }

                // Returns immediately if the validation has been completed

                if (resultOverview.Status == ValidationStatus.Completed)
                {
                    return result;
                }
            } while (true);
        }
    }
}