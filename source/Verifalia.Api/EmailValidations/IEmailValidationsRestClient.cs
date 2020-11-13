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
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Common.Models;
using Verifalia.Api.EmailValidations.Models;

namespace Verifalia.Api.EmailValidations
{
    /// <summary>
    /// Allows to submit, retrieve, list and delete email validations.
    /// <remarks>The features of this type are exposed by way of the <see cref="VerifaliaRestClient.EmailValidations">EmailValidations property</see>
    /// of <see cref="VerifaliaRestClient">VerifaliaRestClient</see>.</remarks>
    /// </summary>
    public interface IEmailValidationsRestClient
    {
        /// <summary>
        /// Submits a new email validation for processing.
        /// <remarks>
        /// By default, this method does not wait for the completion of the email validation job: pass a <see cref="WaitingStrategy"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="SubmitAsync(string,QualityLevelName,WaitingStrategy,CancellationToken)"/> method and wait
        /// for the completion of the submitted job.
        /// <code>
        /// var validation = await SubmitAsync("batman@gmail.com", waitingStrategy: new WaitingStrategy { waitForCompletion: true });
        /// </code>
        /// </example>
        /// <param name="emailAddress">An email addresses to validate.</param>
        /// <param name="quality">The desired results quality for this email validation.</param>
        /// <param name="waitingStrategy">The strategy which rules out how to wait for the completion of the email validation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Validation"/> object representing the submitted email validation job.</returns>
        Task<Validation> SubmitAsync(string emailAddress, QualityLevelName quality = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email validation for processing.
        /// <remarks>
        /// By default, this method does not wait for the completion of the email validation job: pass a <see cref="WaitingStrategy"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="SubmitAsync(IEnumerable{string},QualityLevelName,DeduplicationMode,WaitingStrategy,CancellationToken)"/> method and wait
        /// for the completion of the submitted job.
        /// <code>
        /// var validation = await SubmitAsync(new [] { "batman@gmail.com", "dash.kappei@hotmail.com" }, waitingStrategy: new WaitingStrategy { waitForCompletion: true });
        /// </code>
        /// </example>
        /// <param name="emailAddresses">An enumerable collection of email addresses to validate.</param>
        /// <param name="quality">The desired results quality for this email validation.</param>
        /// <param name="deduplication">The strategy to follow while determining which email addresses are duplicates.</param>
        /// <param name="waitingStrategy">The strategy which rules out how to wait for the completion of the email validation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Validation"/> object representing the submitted email validation job.</returns>
        Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, QualityLevelName quality = default, DeduplicationMode deduplication = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email validation for processing.
        /// <remarks>
        /// By default, this method does not wait for the completion of the email validation job: pass a <see cref="WaitingStrategy"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="SubmitAsync(ValidationRequestEntry,QualityLevelName,WaitingStrategy,CancellationToken)"/> method and wait
        /// for the completion of the submitted job.
        /// <code>
        /// var validation = await SubmitAsync(new ValidationRequestEntry("batman@gmail.com"), waitingStrategy: new WaitingStrategy { waitForCompletion: true });
        /// </code>
        /// </example>
        /// <param name="entry">A <see cref="ValidationRequestEntry"/> to validate.</param>
        /// <param name="quality">The desired results quality for this email validation.</param>
        /// <param name="waitingStrategy">The strategy which rules out how to wait for the completion of the email validation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Validation"/> object representing the submitted email validation job.</returns>
        Task<Validation> SubmitAsync(ValidationRequestEntry entry, QualityLevelName quality = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email validation for processing.
        /// <remarks>
        /// By default, this method does not wait for the completion of the email validation job: pass a <see cref="WaitingStrategy"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="SubmitAsync(IEnumerable{ValidationRequestEntry},QualityLevelName,DeduplicationMode,WaitingStrategy,CancellationToken)"/> method and wait
        /// for the completion of the submitted job.
        /// <code>
        /// var validation = await SubmitAsync(new[] { new ValidationRequestEntry("batman@gmail.com") }, waitingStrategy: new WaitingStrategy { waitForCompletion: true });
        /// </code>
        /// </example>
        /// <param name="entries">An enumerable collection of <see cref="ValidationRequestEntry"/> to validate.</param>
        /// <param name="quality">The desired results quality for this email validation.</param>
        /// <param name="deduplication">The strategy to follow while determining which email addresses are duplicates.</param>
        /// <param name="waitingStrategy">The strategy which rules out how to wait for the completion of the email validation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Validation"/> object representing the submitted email validation job.</returns>
        Task<Validation> SubmitAsync(IEnumerable<ValidationRequestEntry> entries, QualityLevelName quality = default, DeduplicationMode deduplication = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email validation for processing.
        /// <remarks>
        /// By default, this method does not wait for the completion of the email validation job: pass a <see cref="WaitingStrategy"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="SubmitAsync(ValidationRequest,WaitingStrategy,CancellationToken)"/> method and wait
        /// for the completion of the submitted job.
        /// <code>
        /// var validation = await SubmitAsync(new ValidationRequest(new[] { "batman@gmail.com" }, waitingStrategy: new WaitingStrategy { waitForCompletion: true });
        /// </code>
        /// </example>
        /// <param name="validationRequest">A <see cref="ValidationRequest"/> to submit for validation.</param>
        /// <param name="waitingStrategy">The strategy which rules out how to wait for the completion of the email validation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Validation"/> object representing the submitted email validation job.</returns>
        Task<Validation> SubmitAsync(ValidationRequest validationRequest, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns an email validation job previously submitted for processing.
        /// <remarks>In the event retrieving the whole validation job data is not needed and getting just the <see cref="ValidationOverview"/>
        /// would be enough, use the <see cref="GetOverviewAsync"/> method instead.</remarks>
        /// </summary>
        /// <remarks>
        /// By default, this method does not wait for the eventual completion of the email validation job: pass a
        /// <see cref="WaitingStrategy"/> to request a different waiting behavior.
        /// </remarks>
        /// <example>
        /// This sample shows how to call the <see cref="GetAsync"/> method and wait for the completion of the submitted job.
        /// <code>
        /// var validation = await GetAsync(Guid.Parse("c93e972a-7632-4493-aaf8-7523a605a78d"), new WaitingStrategy { waitForCompletion: true });
        /// </code>
        /// </example>
        /// <param name="id">The ID of the email validation job to retrieve.
        /// <remarks>This value is available by way of the <see cref="ValidationOverview.Id"/> property of <see cref="Validation.Overview"/>.</remarks>
        /// </param>
        /// <param name="waitingStrategy">The strategy which rules out how to wait for the completion of the email validation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Validation"/> object representing the requested email validation job.</returns>
        Task<Validation> GetAsync(Guid id, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a lightweight <see cref="ValidationOverview"/> of an email validation job previously submitted for processing.
        /// <remarks>To retrieve the whole job data, including its results, use the <see cref="GetAsync"/> method instead.</remarks>
        /// </summary>
        /// <remarks>
        /// By default, this method does not wait for the eventual completion of the email validation job: pass a
        /// <see cref="WaitingStrategy"/> to request a different waiting behavior.
        /// </remarks>
        /// <example>
        /// This sample shows how to call the <see cref="GetOverviewAsync"/> method and wait for the completion of the submitted job.
        /// <code>
        /// var validation = await GetOverviewAsync(Guid.Parse("c93e972a-7632-4493-aaf8-7523a605a78d"), new WaitingStrategy { waitForCompletion: true });
        /// </code>
        /// </example>
        /// <param name="id">The ID of the email validation job to retrieve the overview for.
        /// <remarks>This value is available by way of the <see cref="ValidationOverview.Id"/> property of <see cref="Validation.Overview"/>.</remarks>
        /// </param>
        /// <param name="waitingStrategy">The strategy which rules out how to wait for the completion of the email validation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValidationOverview"/> object representing the overview for the requested email validation job.</returns>
        Task<ValidationOverview> GetOverviewAsync(Guid id, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an email validation job previously submitted for processing.
        /// </summary>
        /// <param name="id">The ID of the email validation job to delete.
        /// <remarks>This value is available by way of the <see cref="ValidationOverview.Id"/> property of <see cref="Validation.Overview"/>.</remarks>
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins listing validation jobs according to the specified options and user permissions.
        /// <remarks>Use the <see cref="ListSegmentedAsync(ListingCursor,CancellationToken)"/> method to continue listing.</remarks>
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValidationOverviewListSegment"/> with the requested data and an eventual <see cref="ListSegmentMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="ListSegmentedAsync(ListingCursor,CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<ValidationOverviewListSegment> ListSegmentedAsync(ValidationOverviewListingOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Continues listing validation jobs.
        /// <remarks>To begin a listing use the <see cref="ListSegmentedAsync(ValidationOverviewListingOptions,CancellationToken)"/> method.</remarks>
        /// </summary>
        /// <param name="cursor">The cursor to use while traversing the list of validation jobs.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValidationOverviewListSegment"/> with the requested data and an eventual <see cref="ListSegmentMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="ListSegmentedAsync(ListingCursor,CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<ValidationOverviewListSegment> ListSegmentedAsync(ListingCursor cursor, CancellationToken cancellationToken = default);

#if HAS_ASYNC_ENUMERABLE_SUPPORT
        /// <summary>
        /// Lists validation jobs according to the specified options and user permissions.
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An enumerable collection of <see cref="ValidationOverview"/> of the requested data.</returns>
        IAsyncEnumerable<ValidationOverview> ListAsync(ValidationOverviewListingOptions options = null, CancellationToken cancellationToken = default);
#endif

        /// <summary>
        /// Begins listing the available quality levels.
        /// <remarks>Use the <see cref="ListQualityLevelsSegmentedAsync(ListingCursor,CancellationToken)"/> method to continue listing.</remarks>
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="QualityLevelListSegment"/> with the requested data and an eventual <see cref="ListSegmentMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="ListQualityLevelsSegmentedAsync(ListingCursor,CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListQualityLevelsAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<QualityLevelListSegment> ListQualityLevelsSegmentedAsync(ListingOptions options = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Continues listing the available quality levels.
        /// <remarks>To begin a listing use the <see cref="ListQualityLevelsSegmentedAsync(ListingOptions,CancellationToken)"/> method.</remarks>
        /// </summary>
        /// <param name="cursor">The cursor to use while traversing the list of available quality levels.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="QualityLevelListSegment"/> with the requested data and an eventual <see cref="ListSegmentMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="ListQualityLevelsSegmentedAsync(ListingCursor,CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListQualityLevelsAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<QualityLevelListSegment> ListQualityLevelsSegmentedAsync(ListingCursor cursor, CancellationToken cancellationToken = default);

#if HAS_ASYNC_ENUMERABLE_SUPPORT
        /// <summary>
        /// Lists the available quality levels.
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An enumerable collection of <see cref="QualityLevel"/> of the requested data.</returns>
        IAsyncEnumerable<QualityLevel> ListQualityLevelsAsync(ListingOptions options = null, CancellationToken cancellationToken = default);
#endif

        /// <summary>
        /// Begins listing the validated entries for a given validation.
        /// <remarks>Use the <see cref="ListEntriesSegmentedAsync(Guid,ListingCursor,CancellationToken)"/> method to continue listing.</remarks>
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValidationEntryListSegment"/> with the requested data and an eventual <see cref="ListSegmentMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="ListEntriesSegmentedAsync(Guid,ListingCursor,CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListEntriesAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<ValidationEntryListSegment> ListEntriesSegmentedAsync(Guid validationId, ValidationEntryListingOptions options = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Continues listing the validated entries for a given validation.
        /// <remarks>To begin a listing use the <see cref="ListEntriesSegmentedAsync(Guid,ValidationEntryListingOptions,CancellationToken)"/> method.</remarks>
        /// </summary>
        /// <param name="cursor">The cursor to use while traversing the list of validated entries.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValidationEntryListSegment"/> with the requested data and an eventual <see cref="ListSegmentMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="ListEntriesSegmentedAsync(Guid,ListingCursor,CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListEntriesAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<ValidationEntryListSegment> ListEntriesSegmentedAsync(Guid validationId, ListingCursor cursor, CancellationToken cancellationToken = default);

#if HAS_ASYNC_ENUMERABLE_SUPPORT
        /// <summary>
        /// Lists the validated entries for a given validation.
        /// </summary>
        /// <param name="validationId">The unique ID of the validation to list the entries for.</param>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An enumerable collection of <see cref="ValidationEntry"/> of the requested validation.</returns>
        IAsyncEnumerable<ValidationEntry> ListEntriesAsync(Guid validationId, ValidationEntryListingOptions options = default, CancellationToken cancellationToken = default);
#endif
    }
}