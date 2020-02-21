/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2020 Cobisi Research
*
* Cobisi Research
* Via Prima Strada, 35
* 35129, Padova
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
using Verifalia.Api.EmailValidations.Models;

namespace Verifalia.Api.EmailValidations.Converters
{
    internal class ValidationEntryStatusConverter : JsonConverter
    {
        internal static readonly Dictionary<string, ValidationEntryStatus> Mappings = new Dictionary<string, ValidationEntryStatus>(StringComparer.OrdinalIgnoreCase)
        {
            ["AtSignNotFound"] = ValidationEntryStatus.AtSignNotFound,
            ["CatchAllConnectionFailure"] = ValidationEntryStatus.CatchAllConnectionFailure,
            ["CatchAllValidationTimeout"] = ValidationEntryStatus.CatchAllValidationTimeout,
            ["DnsConnectionFailure"] = ValidationEntryStatus.DnsConnectionFailure,
            ["DnsQueryTimeout"] = ValidationEntryStatus.DnsQueryTimeout,
            ["DomainDoesNotExist"] = ValidationEntryStatus.DomainDoesNotExist,
            ["DomainIsMisconfigured"] = ValidationEntryStatus.DomainIsMisconfigured,
            ["DomainIsWellKnownDea"] = ValidationEntryStatus.DomainIsWellKnownDea,
            ["DomainPartCompliancyFailure"] = ValidationEntryStatus.DomainPartCompliancyFailure,
            ["DoubleDotSequence"] = ValidationEntryStatus.DoubleDotSequence,
            ["Duplicate"] = ValidationEntryStatus.Duplicate,
            ["InvalidAddressLength"] = ValidationEntryStatus.InvalidAddressLength,
            ["InvalidCharacterInSequence"] = ValidationEntryStatus.InvalidCharacterInSequence,
            ["InvalidEmptyQuotedWord"] = ValidationEntryStatus.InvalidEmptyQuotedWord,
            ["InvalidFoldingWhiteSpaceSequence"] = ValidationEntryStatus.InvalidFoldingWhiteSpaceSequence,
            ["InvalidLocalPartLength"] = ValidationEntryStatus.InvalidLocalPartLength,
            ["InvalidWordBoundaryStart"] = ValidationEntryStatus.InvalidWordBoundaryStart,
            ["IspSpecificSyntaxFailure"] = ValidationEntryStatus.IspSpecificSyntaxFailure,
            ["LocalEndPointRejected"] = ValidationEntryStatus.LocalEndPointRejected,
            ["LocalPartIsWellKnownRoleAccount"] = ValidationEntryStatus.LocalPartIsWellKnownRoleAccount,
            ["LocalSenderAddressRejected"] = ValidationEntryStatus.LocalSenderAddressRejected,
            ["MailboxConnectionFailure"] = ValidationEntryStatus.MailboxConnectionFailure,
            ["MailboxDoesNotExist"] = ValidationEntryStatus.MailboxDoesNotExist,
            ["MailboxIsDea"] = ValidationEntryStatus.MailboxIsDea,
            ["MailboxTemporarilyUnavailable"] = ValidationEntryStatus.MailboxTemporarilyUnavailable,
            ["MailboxValidationTimeout"] = ValidationEntryStatus.MailboxValidationTimeout,
            ["MailExchangerIsHoneypot"] = ValidationEntryStatus.MailExchangerIsHoneypot,
            ["MailExchangerIsWellKnownDea"] = ValidationEntryStatus.MailExchangerIsWellKnownDea,
            ["ServerDoesNotSupportInternationalMailboxes"] = ValidationEntryStatus.ServerDoesNotSupportInternationalMailboxes,
            ["ServerIsCatchAll"] = ValidationEntryStatus.ServerIsCatchAll,
            ["ServerTemporaryUnavailable"] = ValidationEntryStatus.ServerTemporaryUnavailable,
            ["SmtpConnectionFailure"] = ValidationEntryStatus.SmtpConnectionFailure,
            ["SmtpConnectionTimeout"] = ValidationEntryStatus.SmtpConnectionTimeout,
            ["SmtpDialogError"] = ValidationEntryStatus.SmtpDialogError,
            ["Success"] = ValidationEntryStatus.Success,
            ["UnacceptableDomainLiteral"] = ValidationEntryStatus.UnacceptableDomainLiteral,
            ["UnbalancedCommentParenthesis"] = ValidationEntryStatus.UnbalancedCommentParenthesis,
            ["UnexpectedQuotedPairSequence"] = ValidationEntryStatus.UnexpectedQuotedPairSequence,
            ["UnhandledException"] = ValidationEntryStatus.UnhandledException,
            ["UnmatchedQuotedPair"] = ValidationEntryStatus.UnmatchedQuotedPair
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            foreach (var mapping in Mappings)
            {
                if (mapping.Value == (ValidationEntryStatus) value)
                {
                    writer.WriteRawValue(mapping.Key);
                    return;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported validation entry status.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = Convert.ToString(reader.Value, CultureInfo.InvariantCulture);

            return Mappings.TryGetValue(value, out var mappedStatus)
                ? mappedStatus
                : ValidationEntryStatus.Unknown;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ValidationEntryStatus);
        }
    }
}