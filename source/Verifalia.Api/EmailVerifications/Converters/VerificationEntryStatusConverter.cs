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
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Verifalia.Api.EmailVerifications.Models;

namespace Verifalia.Api.EmailVerifications.Converters
{
    internal class VerificationEntryStatusConverter : JsonConverter
    {
        internal static readonly Dictionary<string, VerificationEntryStatus> Mappings = new(StringComparer.OrdinalIgnoreCase)
        {
            ["AtSignNotFound"] = VerificationEntryStatus.AtSignNotFound,
            ["CatchAllConnectionFailure"] = VerificationEntryStatus.CatchAllConnectionFailure,
            ["CatchAllValidationTimeout"] = VerificationEntryStatus.CatchAllValidationTimeout,
            ["DnsConnectionFailure"] = VerificationEntryStatus.DnsConnectionFailure,
            ["DnsQueryTimeout"] = VerificationEntryStatus.DnsQueryTimeout,
            ["DomainDoesNotExist"] = VerificationEntryStatus.DomainDoesNotExist,
            ["DomainHasNullMx"] = VerificationEntryStatus.DomainHasNullMx,
            ["DomainIsMisconfigured"] = VerificationEntryStatus.DomainIsMisconfigured,
            ["DomainIsWellKnownDea"] = VerificationEntryStatus.DomainIsWellKnownDea,
            ["DomainPartCompliancyFailure"] = VerificationEntryStatus.DomainPartCompliancyFailure,
            ["DoubleDotSequence"] = VerificationEntryStatus.DoubleDotSequence,
            ["Duplicate"] = VerificationEntryStatus.Duplicate,
            ["InvalidAddressLength"] = VerificationEntryStatus.InvalidAddressLength,
            ["InvalidCharacterInSequence"] = VerificationEntryStatus.InvalidCharacterInSequence,
            ["InvalidEmptyQuotedWord"] = VerificationEntryStatus.InvalidEmptyQuotedWord,
            ["InvalidFoldingWhiteSpaceSequence"] = VerificationEntryStatus.InvalidFoldingWhiteSpaceSequence,
            ["InvalidLocalPartLength"] = VerificationEntryStatus.InvalidLocalPartLength,
            ["InvalidWordBoundaryStart"] = VerificationEntryStatus.InvalidWordBoundaryStart,
            ["IspSpecificSyntaxFailure"] = VerificationEntryStatus.IspSpecificSyntaxFailure,
            ["LocalEndPointRejected"] = VerificationEntryStatus.LocalEndPointRejected,
            ["LocalPartIsWellKnownRoleAccount"] = VerificationEntryStatus.LocalPartIsWellKnownRoleAccount,
            ["LocalSenderAddressRejected"] = VerificationEntryStatus.LocalSenderAddressRejected,
            ["MailboxConnectionFailure"] = VerificationEntryStatus.MailboxConnectionFailure,
            ["MailboxDoesNotExist"] = VerificationEntryStatus.MailboxDoesNotExist,
            ["MailboxHasInsufficientStorage"] = VerificationEntryStatus.MailboxHasInsufficientStorage,
            ["MailboxIsDea"] = VerificationEntryStatus.MailboxIsDea,
            ["MailboxTemporarilyUnavailable"] = VerificationEntryStatus.MailboxTemporarilyUnavailable,
            ["MailboxValidationTimeout"] = VerificationEntryStatus.MailboxValidationTimeout,
            ["MailExchangerIsHoneypot"] = VerificationEntryStatus.MailExchangerIsHoneypot,
            ["MailExchangerIsParked"] = VerificationEntryStatus.MailExchangerIsParked,
            ["MailExchangerIsWellKnownDea"] = VerificationEntryStatus.MailExchangerIsWellKnownDea,
            ["OverrideMatch"] = VerificationEntryStatus.OverrideMatch,
            ["ServerDoesNotSupportInternationalMailboxes"] = VerificationEntryStatus.ServerDoesNotSupportInternationalMailboxes,
            ["ServerIsCatchAll"] = VerificationEntryStatus.ServerIsCatchAll,
            ["ServerTemporaryUnavailable"] = VerificationEntryStatus.ServerTemporaryUnavailable,
            ["SmtpConnectionFailure"] = VerificationEntryStatus.SmtpConnectionFailure,
            ["SmtpConnectionTimeout"] = VerificationEntryStatus.SmtpConnectionTimeout,
            ["SmtpDialogError"] = VerificationEntryStatus.SmtpDialogError,
            ["Success"] = VerificationEntryStatus.Success,
            ["UnacceptableDomainLiteral"] = VerificationEntryStatus.UnacceptableDomainLiteral,
            ["UnbalancedCommentParenthesis"] = VerificationEntryStatus.UnbalancedCommentParenthesis,
            ["UnexpectedQuotedPairSequence"] = VerificationEntryStatus.UnexpectedQuotedPairSequence,
            ["UnhandledException"] = VerificationEntryStatus.UnhandledException,
            ["UnmatchedQuotedPair"] = VerificationEntryStatus.UnmatchedQuotedPair,
        };

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            foreach (var mapping in Mappings)
            {
                if (mapping.Value == (VerificationEntryStatus) value)
                {
                    writer.WriteValue(mapping.Key);
                    return;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported validation entry status.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = Convert.ToString(reader.Value, CultureInfo.InvariantCulture);

            return Mappings.TryGetValue(value, out var mappedStatus)
                ? mappedStatus
                : VerificationEntryStatus.Unknown;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(VerificationEntryStatus);
        }
    }
}