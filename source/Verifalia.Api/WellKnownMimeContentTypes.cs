﻿/*
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

namespace Verifalia.Api
{
    /// <summary>
    /// Defines the MIME content types understood by the Verifalia API.
    /// </summary>
    public static class WellKnownMimeContentTypes
    {
        /// <summary>
        /// application/json MIME content type.
        /// </summary>
        public const string ApplicationJson = "application/json";

        /// <summary>
        /// Plain-text files (.txt), with one email address per line.
        /// </summary>
        public const string TextPlain = "text/plain";
        
        /// <summary>
        /// Comma-separated values (.csv).
        /// </summary>
        public const string TextCsv = "text/csv";
        
        /// <summary>
        /// Tab-separated values (usually coming with the .tsv extension).
        /// </summary>
        public const string TextTsv = "text/tab-separated-values";

        /// <summary>
        /// Microsoft Excel 97-2003 Worksheet (.xls).
        /// </summary>
        public const string ExcelXls = "application/vnd.ms-excel";

        /// <summary>
        /// Microsoft Excel workbook (.xslx).
        /// </summary>
        public const string ExcelXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}