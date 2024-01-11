/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2024 Cobisi Research
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
using System.Threading.Tasks;
using Flurl.Http.Testing;
using Verifalia.Api.EmailValidations;
using Verifalia.Api.EmailValidations.Models;
using Xunit;

namespace Verifalia.Api.Tests
{
    public partial class ValidationRestClientTests
    {
        [Fact]
        public async Task RetrieveShouldReturnNullForHttp404()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWithJson(new { }, 404);

                var validationClient = new EmailValidationsRestClient(new DummyRestClientFactory());
                var validationId = Guid.Parse("a3706a81-87da-4762-a135-dabaac6e6971");

                var response = await validationClient.GetAsync(validationId);

                Assert.Null(response);
            }
        }

        [Fact]
        public async Task ShouldHandleQueryOfInProgressJobs()
        {
            using (var httpTest = new HttpTest())
            {
                var validationId = Guid.Parse("a3706a81-87da-4762-a135-dabaac6e6971");

                httpTest.RespondWithJson(new
                {
                    overview = new
                    {
                        id = validationId,
                        status = "InProgress"
                    }
                }, 202);

                var validationClient = new EmailValidationsRestClient(new DummyRestClientFactory());

                var response = await validationClient.GetAsync(validationId);

                Assert.Equal(ValidationStatus.InProgress, response.Overview.Status);
            }
        }

        [Fact]
        public async Task ShouldWaitForCompletionOnInProgress()
        {
            var noOfInProgressReplies = 5;

            using (var httpTest = new HttpTest())
            {
                var fakeValidationId = Guid.Parse("a3706a81-87da-4762-a135-dabaac6e6971");

                // Reply for a few times with an in-progress status codes

                for (var i = 0; i < noOfInProgressReplies; i++)
                {
                    httpTest.RespondWithJson(new
                    {
                        overview = new
                        {
                            id = fakeValidationId,
                            status = "in-progress"
                        }
                    }, 202);
                }

                // Then report the completion of the job

                httpTest.RespondWithJson(new
                {
                    overview = new
                    {
                        id = fakeValidationId,
                        status = "completed"
                    }
                }, 200);

                // Fetch the job and poll until it's done

                var validationRestClient = new EmailValidationsRestClient(new DummyRestClientFactory());
                var validation = await validationRestClient.GetAsync(fakeValidationId, WaitOptions.NoWait);

                // Did we make the expected number of requests?

                Assert.Equal(noOfInProgressReplies + 1, httpTest.CallLog.Count);
                Assert.Equal(ValidationStatus.Completed, validation.Overview.Status);
            }
        }

    }
}
