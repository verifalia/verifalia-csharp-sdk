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
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Common.Models;
using Verifalia.Api.EmailVerifications.Models;

namespace Verifalia.Api.EmailVerifications
{
    /// <summary>
    /// Allows to submit, retrieve, list and delete email verifications.
    /// <remarks>The features of this type are exposed by way of the <see cref="VerifaliaClient.EmailVerifications">EmailVerifications property</see>
    /// of <see cref="VerifaliaClient">VerifaliaRestClient</see>.</remarks>
    /// </summary>
    public interface IEmailVerificationsClient
    {
        /// <summary>
        /// Submits a new email verification for processing.
        /// <remarks>
        /// By default, this method waits for the completion of the email verification job: pass a <see cref="WaitOptions"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="RunAsync"/> method and wait
        /// for the completion of the submitted job.
        /// <code>
        /// var verification = await RunAsync("batman@gmail.com");
        /// </code>
        /// </example>
        /// <param name="emailAddress">An email address to verify.</param>
        /// <param name="quality">The desired results quality for this email verification.</param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Verification"/> object representing the submitted email verification job.</returns>
        Task<Verification> RunAsync(string emailAddress, QualityLevelName? quality = null, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email verification for processing.
        /// <remarks>
        /// By default, this method waits for the completion of the email verification job: pass a <see cref="WaitOptions"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="RunAsync(System.Collections.Generic.IEnumerable{string},Verifalia.Api.EmailVerifications.Models.QualityLevelName?,Verifalia.Api.EmailVerifications.Models.DeduplicationMode?,Verifalia.Api.EmailVerifications.WaitOptions?,System.Threading.CancellationToken)"/> method and wait
        /// for the completion of the submitted job.
        /// <code>
        /// var verification = await RunAsync(new [] { "batman@gmail.com", "walt@a1acarwash.com" });
        /// </code>
        /// </example>
        /// <param name="emailAddresses">An enumerable collection of email addresses to verify.</param>
        /// <param name="quality">The desired results quality for this email verification.</param>
        /// <param name="deduplication">The strategy to follow while determining which email addresses are duplicates.</param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Verification"/> object representing the submitted email verification job.</returns>
        Task<Verification> RunAsync(IEnumerable<string> emailAddresses, QualityLevelName? quality = null, DeduplicationMode? deduplication = null, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email verification for processing.
        /// <remarks>
        /// By default, this method waits for the completion of the email verification job: pass a <see cref="WaitOptions"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="RunAsync(Verifalia.Api.EmailVerifications.Models.VerificationRequestEntry,Verifalia.Api.EmailVerifications.Models.QualityLevelName?,Verifalia.Api.EmailVerifications.WaitOptions?,System.Threading.CancellationToken)"/> method and wait
        /// for the completion of the submitted job.
        /// <code>
        /// var verification = await RunAsync(new VerificationRequestEntry("batman@gmail.com"));
        /// </code>
        /// </example>
        /// <param name="entry">A <see cref="VerificationRequestEntry"/> to verify.</param>
        /// <param name="quality">The desired results quality for this email verification.</param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Verification"/> object representing the submitted email verification job.</returns>
        Task<Verification> RunAsync(VerificationRequestEntry entry, QualityLevelName? quality = null, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email verification for processing.
        /// <remarks>
        /// By default, this method waits for the completion of the email verification job: pass a <see cref="WaitOptions"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="RunAsync(System.Collections.Generic.IEnumerable{Verifalia.Api.EmailVerifications.Models.VerificationRequestEntry},Verifalia.Api.EmailVerifications.Models.QualityLevelName?,Verifalia.Api.EmailVerifications.Models.DeduplicationMode?,Verifalia.Api.EmailVerifications.WaitOptions?,System.Threading.CancellationToken)"/> method and wait
        /// for the completion of the submitted job.
        /// <code>
        /// var verification = await RunAsync(new[] { new VerificationRequestEntry("batman@gmail.com") });
        /// </code>
        /// </example>
        /// <param name="entries">An enumerable collection of <see cref="VerificationRequestEntry"/> to verify.</param>
        /// <param name="quality">The desired results quality for this email verification.</param>
        /// <param name="deduplication">The strategy to follow while determining which email addresses are duplicates.</param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Verification"/> object representing the submitted email verification job.</returns>
        Task<Verification> RunAsync(IEnumerable<VerificationRequestEntry> entries, QualityLevelName? quality = null, DeduplicationMode? deduplication = null, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email verification for processing.
        /// <remarks>
        /// By default, this method waits for the completion of the email verification job: pass a <see cref="WaitOptions"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="RunAsync(Verifalia.Api.EmailVerifications.Models.VerificationRequest,Verifalia.Api.EmailVerifications.WaitOptions?,System.Threading.CancellationToken)"/> method and wait
        /// for the completion of the submitted job.
        /// <code>
        /// var verification = await RunAsync(new VerificationRequest(new[] { "batman@gmail.com" });
        /// </code>
        /// </example>
        /// <param name="request">A <see cref="VerificationRequest"/> to submit for verification.</param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Verification"/> object representing the submitted email verification job.</returns>
        Task<Verification> RunAsync(VerificationRequest request, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email verification for processing through a file, with support for the following formats:
        /// - plain text files (.txt), with one email address per line
        /// - comma-separated values (.csv), tab-separated values (.tsv) and other delimiter-separated values files
        /// - Microsoft Excel spreadsheets (.xls and .xlsx)
        /// <remarks>
        /// By default, this method waits for the completion of the email verification job: pass a <see cref="WaitOptions"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <param name="file">An array of bytes with the content of the file to submit for verification.</param>
        /// <param name="contentType">The MIME content type of the file.</param>
        /// <param name="quality">The desired results quality for this email verification.</param>
        /// <param name="deduplication">The strategy to follow while determining which email addresses are duplicates.</param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Verification"/> object representing the submitted email verification job.</returns>
        Task<Verification> RunAsync(byte[] file, MediaTypeHeaderValue contentType, QualityLevelName? quality = null, DeduplicationMode? deduplication = null, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email verification for processing through a file, with support for the following formats:
        /// - plain text files (.txt), with one email address per line
        /// - comma-separated values (.csv), tab-separated values (.tsv) and other delimiter-separated values files
        /// - Microsoft Excel spreadsheets (.xls and .xlsx)
        /// <remarks>
        /// By default, this method waits for the completion of the email verification job: pass a <see cref="WaitOptions"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <param name="fileInfo">A <see cref="FileInfo"/> instance pointing to the file to submit.</param>
        /// <param name="contentType">The MIME content type of the file.
        /// <remarks>If <see langword="null" /> (default value), the library attempts to guess the content type of the file based on its extension.</remarks>
        /// </param>
        /// <param name="quality">The desired results quality for this email verification.</param>
        /// <param name="deduplication">The strategy to follow while determining which email addresses are duplicates.</param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Verification"/> object representing the submitted email verification job.</returns>
        Task<Verification> RunAsync(FileInfo fileInfo, MediaTypeHeaderValue? contentType = null, QualityLevelName? quality = null, DeduplicationMode? deduplication = null, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email verification for processing through a file, with support for the following formats:
        /// - plain text files (.txt), with one email address per line
        /// - comma-separated values (.csv), tab-separated values (.tsv) and other delimiter-separated values files
        /// - Microsoft Excel spreadsheets (.xls and .xlsx)
        /// <remarks>
        /// By default, this method waits for the completion of the email verification job: pass a <see cref="WaitOptions"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <param name="file">A <see cref="Stream"/> with the file content to submit.</param>
        /// <param name="contentType">The MIME content type of the file.</param>
        /// <param name="quality">The desired results quality for this email verification.</param>
        /// <param name="deduplication">The strategy to follow while determining which email addresses are duplicates.</param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Verification"/> object representing the submitted email verification job.</returns>
        Task<Verification> RunAsync(Stream file, MediaTypeHeaderValue contentType, QualityLevelName? quality = null, DeduplicationMode? deduplication = null, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new email verification for processing through a file, with support for the following formats:
        /// - plain text files (.txt), with one email address per line
        /// - comma-separated values (.csv), tab-separated values (.tsv) and other delimiter-separated values files
        /// - Microsoft Excel spreadsheets (.xls and .xlsx)
        /// <remarks>
        /// By default, this method waits for the completion of the email verification job: pass a <see cref="WaitOptions"/> to request
        /// a different waiting behavior.
        /// </remarks>
        /// </summary>
        /// <param name="request">A <see cref="FileVerificationRequest"/> describing the verification request for a file.</param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Verification"/> object representing the submitted email verification job.</returns>
        Task<Verification> RunAsync(FileVerificationRequest request, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Returns an email verification job that was previously submitted for processing.
        /// <remarks>In the event retrieving the whole verification job data is not needed and getting just the <see cref="VerificationOverview"/>
        /// would be enough, use the <see cref="GetOverviewAsync"/> method instead.</remarks>
        /// </summary>
        /// <remarks>
        /// By default, this method waits for the eventual completion of the email verification job: pass a
        /// <see cref="WaitOptions"/> to request a different waiting behavior.
        /// </remarks>
        /// <example>
        /// This sample shows how to call the <see cref="GetAsync"/> method and wait for the completion of the submitted job.
        /// <code>
        /// var verification = await GetAsync("c93e972a-7632-4493-aaf8-7523a605a78d");
        /// </code>
        /// </example>
        /// <param name="verificationId">The ID of the email verification job to retrieve.
        /// <remarks>This value is available by way of the <see cref="VerificationOverview.Id"/> property of <see cref="Verification.Overview"/>.</remarks>
        /// </param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Verification"/> object representing the requested email verification job.</returns>
        Task<Verification?> GetAsync(string verificationId, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a lightweight <see cref="VerificationOverview"/> of an email verification job that was previously submitted for processing.
        /// <remarks>To retrieve the whole job data, including its results, use the <see cref="GetAsync"/> method instead.</remarks>
        /// </summary>
        /// <remarks>
        /// By default, this method waits for the eventual completion of the email verification job: pass a
        /// <see cref="WaitOptions"/> to request a different waiting behavior.
        /// </remarks>
        /// <example>
        /// This sample shows how to call the <see cref="GetOverviewAsync"/> method and wait for the completion of the submitted job.
        /// <code>
        /// var verification = await GetOverviewAsync("c93e972a-7632-4493-aaf8-7523a605a78d");
        /// </code>
        /// </example>
        /// <param name="verificationId">The ID of the email verification job to retrieve the overview for.
        /// <remarks>This value is available by way of the <see cref="VerificationOverview.Id"/> property of <see cref="Verification.Overview"/>.</remarks>
        /// </param>
        /// <param name="waitOptions">Defines the options that specify how to wait for the completion of the email verification job.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="VerificationOverview"/> object representing the overview for the requested email verification job.</returns>
        Task<VerificationOverview?> GetOverviewAsync(string verificationId, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an email verification job that was previously submitted for processing.
        /// </summary>
        /// <param name="verificationId">The unique identifier of the email verification job to be deleted.
        /// <remarks>This value is available by way of the <see cref="VerificationOverview.Id"/> property of <see cref="Verification.Overview"/>.</remarks>
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        Task DeleteAsync(string verificationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins listing email verification jobs according to the specified options and user permissions.
        /// <remarks>Use the <see cref="GetPageAsync(Verifalia.Api.Common.Models.ListingCursor,System.Threading.CancellationToken)"/> method to continue listing.</remarks>
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="VerificationOverviewPagedResult"/> with the requested data and an eventual <see cref="PagedResultMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="GetPageAsync(Verifalia.Api.Common.Models.ListingCursor,System.Threading.CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<VerificationOverviewPagedResult> GetPageAsync(VerificationOverviewListingOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Continues listing email verification jobs.
        /// <remarks>To begin a listing use the <see cref="GetPageAsync"/> method.</remarks>
        /// </summary>
        /// <param name="cursor">The cursor to use while traversing the list of verification jobs.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="VerificationOverviewPagedResult"/> with the requested data and an eventual <see cref="PagedResultMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="GetPageAsync(Verifalia.Api.Common.Models.ListingCursor,System.Threading.CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<VerificationOverviewPagedResult> GetPageAsync(ListingCursor cursor, CancellationToken cancellationToken = default);

#if HAS_ASYNC_ENUMERABLE_SUPPORT
        /// <summary>
        /// Lists email verification jobs according to the specified options and user permissions.
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An enumerable collection of <see cref="VerificationOverview"/> of the requested data.</returns>
        IAsyncEnumerable<VerificationOverview> ListAsync(VerificationOverviewListingOptions? options = null, CancellationToken cancellationToken = default);
#endif

        /// <summary>
        /// Begins listing the available quality levels.
        /// <remarks>Use the <see cref="GetQualityLevelsPageAsync(Verifalia.Api.Common.Models.ListingCursor,System.Threading.CancellationToken)"/> method to continue listing.</remarks>
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="QualityLevelPagedResult"/> with the requested data and an eventual <see cref="PagedResultMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="GetQualityLevelsPageAsync(Verifalia.Api.Common.Models.ListingCursor,System.Threading.CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListQualityLevelsAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<QualityLevelPagedResult> GetQualityLevelsPageAsync(ListingOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Continues listing the available quality levels.
        /// <remarks>To begin a listing use the <see cref="GetQualityLevelsPageAsync"/> method.</remarks>
        /// </summary>
        /// <param name="cursor">The cursor to use while traversing the list of available quality levels.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="QualityLevelPagedResult"/> with the requested data and an eventual <see cref="PagedResultMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="GetQualityLevelsPageAsync(Verifalia.Api.Common.Models.ListingCursor,System.Threading.CancellationToken)"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListQualityLevelsAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<QualityLevelPagedResult> GetQualityLevelsPageAsync(ListingCursor cursor, CancellationToken cancellationToken = default);

#if HAS_ASYNC_ENUMERABLE_SUPPORT
        /// <summary>
        /// Lists the available quality levels.
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An enumerable collection of <see cref="QualityLevel"/> of the requested data.</returns>
        IAsyncEnumerable<QualityLevel> ListQualityLevelsAsync(ListingOptions? options = null, CancellationToken cancellationToken = default);
#endif

        /// <summary>
        /// Begins listing the validated entries for a given email verification job.
        /// <remarks>Use the <see cref="GetEntriesPageAsync"/> method to continue listing.</remarks>
        /// </summary>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="VerificationEntryPagedResult"/> with the requested data and an eventual <see cref="PagedResultMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="GetEntriesPageAsync"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListEntriesAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<VerificationEntryPagedResult> GetEntriesPageAsync(string verificationId, VerificationEntryListingOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Continues listing the validated entries for a given email verification job.
        /// <remarks>To begin a listing use the <see cref="GetEntriesPageAsync(string,Verifalia.Api.EmailVerifications.Models.VerificationEntryListingOptions?,System.Threading.CancellationToken)"/> method.</remarks>
        /// </summary>
        /// <param name="cursor">The cursor to use while traversing the list of validated entries.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="VerificationEntryPagedResult"/> with the requested data and an eventual <see cref="PagedResultMeta.Cursor"/> to be
        /// used on subsequent listing calls to the <see cref="GetEntriesPageAsync"/> method.</returns>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListEntriesAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<VerificationEntryPagedResult> GetEntriesPageAsync(string verificationId, ListingCursor cursor, CancellationToken cancellationToken = default);

#if HAS_ASYNC_ENUMERABLE_SUPPORT
        /// <summary>
        /// Lists the entries for a given completed email verification job.
        /// </summary>
        /// <param name="verificationId">The unique ID of the verification job to list the entries for.</param>
        /// <param name="options">The options for the listing operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An enumerable collection of <see cref="VerificationEntry"/> of the requested email verification job.</returns>
        IAsyncEnumerable<VerificationEntry> ListEntriesAsync(string verificationId, VerificationEntryListingOptions? options = null, CancellationToken cancellationToken = default);
#endif

        /// <summary>
        /// Exports the results from a completed email verification job to a stream using the specified format.
        /// </summary>
        /// <param name="verificationId">The unique ID of the email verification job whose results you want to export.</param>
        /// <param name="format">Choose between CSV, XLS (Excel 97-2003), or XLSX (Excel 2007 and later) for your output format.</param>
        /// <param name="options">Provide optional listing options to further customize the data exported from the verification job. Leave blank for default settings.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A stream containing the results of the specified email verification job in the requested format.</returns>
        Task<Stream> ExportEntriesAsync(string verificationId, ExportedEntriesFormat format, VerificationEntryListingOptions? options = null, CancellationToken cancellationToken = default);
    }
}