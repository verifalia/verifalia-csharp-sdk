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
using System.IO;
using System.Threading;
using System.Linq;
using System.Net.Http.Headers;

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// Represents an email validation request of a file to be submitted against the Verifalia API.
    /// </summary>
    /// <remarks>Once initialized, pass the instance of <see cref="FileValidationRequest"/> to the
    /// <see cref="IEmailValidationsRestClient.SubmitAsync(FileValidationRequest,WaitingStrategy,CancellationToken)"/> method or one of its
    /// overloads.</remarks>
    public sealed class FileValidationRequest : ValidationRequestBase, IDisposable
    {
        private readonly bool _leaveOpen;
    
        /// <summary>
        /// The file stream which will be submitted for validation. 
        /// </summary>
        public Stream File { get; }

        /// <summary>
        /// The media type of the <see cref="File"/> stream data.
        /// </summary>
        public MediaTypeHeaderValue ContentType { get; }

        /// <summary>
        /// An optional, zero-based index of the first row to import and process. If not specified, Verifalia will start
        /// processing files from the first (0) row.
        /// </summary>
        public int? StartingRow { get; set; }
        
        /// <summary>
        /// An optional, zero-based index of the last row to import and process. If not specified, Verifalia will process
        /// rows until the end of the file.
        /// </summary>
        public int? EndingRow { get; set; }

        /// <summary>
        /// An optional zero-based index of the column to import; applies to comma-separated (.csv), tab-separated (.tsv)
        /// and other delimiter-separated values files, and Excel files. If not specified, Verifalia will use the first
        /// (0) column.
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// An optional zero-based index of the worksheet to import; applies to Excel files only. If not specified,
        /// Verifalia will use the first (0) worksheet.
        /// </summary>
        public int? Sheet { get; set; }

        /// <summary>
        /// Allows to specify the line ending sequence of the provided file; applies to plain-text files, comma-separated
        /// (.csv), tab-separated (.tsv) and other delimiter-separated values files.
        /// </summary>
        public LineEndingMode? LineEnding { get; set; }

        /// <summary>
        /// An optional string with the column delimiter sequence of the file; applies to comma-separated (.csv),
        /// tab-separated (.tsv) and other delimiter-separated values files. If not specified, Verifalia will use the ,
        /// (comma) symbol for CSV files and the \t (tab) symbol for TSV files.
        /// </summary>
        public string Delimiter { get; set; }

        /// <summary>
        /// Initializes a <see cref="FileValidationRequest"/> to be submitted to the Verifalia email validation engine.
        /// </summary>
        /// <param name="file">A <see cref="Stream"/> with the file content to submit.</param>
        /// <param name="contentType">The MIME content type of the file.</param>
        /// <param name="quality">An optional <see cref="QualityLevelName"/> referring to the expected results quality for the request.</param>
        /// <param name="deduplication">An optional <see cref="DeduplicationMode"/> to use while determining which email addresses are duplicates.</param>
        /// <param name="priority">An optional <see cref="ValidationPriority"/> (speed) of a validation job, relative to the parent Verifalia account.
        /// <remarks>Setting this value is useful only in the event there are multiple active concurrent validation jobs for the calling Verifalia
        /// account and the current request should be treated differently than the others, with regards to the processing speed.</remarks>
        /// </param>
        /// <param name="leaveOpen">True to leave the file object open after FileValidationRequest is disposed, false otherwise.</param>
        public FileValidationRequest(Stream file, MediaTypeHeaderValue contentType, QualityLevelName quality = default, DeduplicationMode deduplication = default, ValidationPriority priority = default, bool leaveOpen = default)
        {
            File = file ?? throw new ArgumentNullException(nameof(file));
            ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
            Quality = quality;
            Deduplication = deduplication;
            Priority = priority;
            _leaveOpen = leaveOpen;
        }
        
        /// <summary>
        /// Initializes a <see cref="FileValidationRequest"/> to be submitted to the Verifalia email validation engine.
        /// </summary>
        /// <param name="path">The path of the file to submit.</param>
        /// <param name="contentType">The MIME content type of the file.
        /// <remarks>If <see langword="null" /> (default value), the library attempts to guess the content type of the file based on its extension.</remarks>
        /// </param>
        /// <param name="quality">An optional <see cref="QualityLevelName"/> referring to the expected results quality for the request.</param>
        /// <param name="deduplication">An optional <see cref="DeduplicationMode"/> to use while determining which email addresses are duplicates.</param>
        /// <param name="priority">An optional <see cref="ValidationPriority"/> (speed) of a validation job, relative to the parent Verifalia account.
        /// <remarks>Setting this value is useful only in the event there are multiple active concurrent validation jobs for the calling Verifalia
        /// account and the current request should be treated differently than the others, with regards to the processing speed.</remarks>
        /// </param>
        public FileValidationRequest(string path, MediaTypeHeaderValue contentType = default, QualityLevelName quality = default, DeduplicationMode deduplication = default, ValidationPriority priority = default)
            : this(new FileInfo(path),
                contentType,
                quality,
                deduplication,
                priority)
        {
        }

        /// <summary>
        /// Initializes a <see cref="FileValidationRequest"/> to be submitted to the Verifalia email validation engine.
        /// </summary>
        /// <param name="fileInfo">A <see cref="FileInfo"/> instance pointing to the file to submit.</param>
        /// <param name="contentType">The MIME content type of the file.
        /// <remarks>If <see langword="null" /> (default value), the library attempts to guess the content type of the file based on its extension.</remarks>
        /// </param>
        /// <param name="quality">An optional <see cref="QualityLevelName"/> referring to the expected results quality for the request.</param>
        /// <param name="deduplication">An optional <see cref="DeduplicationMode"/> to use while determining which email addresses are duplicates.</param>
        /// <param name="priority">An optional <see cref="ValidationPriority"/> (speed) of a validation job, relative to the parent Verifalia account.
        /// <remarks>Setting this value is useful only in the event there are multiple active concurrent validation jobs for the calling Verifalia
        /// account and the current request should be treated differently than the others, with regards to the processing speed.</remarks>
        /// </param>
        public FileValidationRequest(FileInfo fileInfo, MediaTypeHeaderValue contentType = default, QualityLevelName quality = default, DeduplicationMode deduplication = default, ValidationPriority priority = default)
        {
            if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));

            // Guess the content type, if no value has been specified

            contentType ??= TryGuessContentTypeFromFileExtension(fileInfo.Extension);
            ContentType = contentType ?? throw new ArgumentException($"Can't automatically guess the content type for the specified file extension '{fileInfo.Extension}', please pass it manually.", nameof(fileInfo));

            File = fileInfo.Open(FileMode.Open, FileAccess.Read);
            Quality = quality;
            Deduplication = deduplication;
            Priority = priority;
            _leaveOpen = false;
        }
        
        private MediaTypeHeaderValue TryGuessContentTypeFromFileExtension(string extension)
        {
            // TODO: Cache the following MediaTypeHeaderValue instances

            switch (extension)
            {
                case ".txt": return MediaTypeHeaderValue.Parse(WellKnownMimeContentTypes.TextPlain);
                case ".csv": return MediaTypeHeaderValue.Parse(WellKnownMimeContentTypes.TextCsv);
                case ".tsv": return MediaTypeHeaderValue.Parse(WellKnownMimeContentTypes.TextTsv);
                case ".tab": return MediaTypeHeaderValue.Parse(WellKnownMimeContentTypes.TextTsv);
                case ".xls": return MediaTypeHeaderValue.Parse(WellKnownMimeContentTypes.ExcelXls);
                case ".xlsx": return MediaTypeHeaderValue.Parse(WellKnownMimeContentTypes.ExcelXlsx);
                default: return null;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (!_leaveOpen)
            {
                File.Dispose();
            }
        }
    }
}