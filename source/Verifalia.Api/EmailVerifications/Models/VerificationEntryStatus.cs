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
    /// Provides enumerated values for the supported email verification statuses for a <see cref="VerificationEntry"/>.
    /// </summary>
    public enum VerificationEntryStatus
    {
        /// <summary>
        /// An unknown status due to a value reported by the API that is missing from this SDK.
        /// </summary>
        Unknown = 0,
        
        /// <summary>
        /// The at sign symbol (@), used to separate the local part from the domain part of the address, was not found.
        /// </summary>
        AtSignNotFound,

        /// <summary>
        /// A connection error occurred while verifying that the external mail exchanger rejects nonexistent email addresses.
        /// </summary>
        CatchAllConnectionFailure,
        
        /// <summary>
        /// A timeout occurred while verifying fake email address rejection for the mail server.
        /// </summary>
        CatchAllValidationTimeout,

        /// <summary>
        /// A socket connection error occurred while querying the DNS server, preventing successful email verification.
        /// </summary>
        DnsConnectionFailure,

        /// <summary>
        /// The request timed out while querying the DNS server(s) for records about the email address domain.
        /// </summary>
        DnsQueryTimeout,

        /// <summary>
        /// The domain of the email address does not exist.
        /// </summary>
        DomainDoesNotExist,

        /// <summary>
        /// The domain has a NULL MX (RFC 7505) resource record and cannot accept email messages.
        /// </summary>
        DomainHasNullMx,

        /// <summary>
        /// The domain of the email address does not have any valid DNS records, preventing successful verification.
        /// </summary>
        DomainIsMisconfigured,
        
        /// <summary>
        /// The email address is provided by a well-known disposable email address provider (DEA).
        /// </summary>
        DomainIsWellKnownDea,

        /// <summary>
        /// The domain part of the email address does not comply with IETF standards.
        /// </summary>
        DomainPartCompliancyFailure,
        
        /// <summary>
        /// An invalid sequence of two adjacent dots was found, indicating an incorrect email format.
        /// </summary>
        DoubleDotSequence,

        /// <summary>
        /// The item is a duplicate of another email address in the list, as indicated by the <see cref="VerificationEntry.DuplicateOf"/> property.
        /// </summary>
        Duplicate,

        /// <summary>
        /// The email address has an invalid total length, making it impossible to verify.
        /// </summary>
        InvalidAddressLength,

        /// <summary>
        /// An invalid character was detected in the provided sequence, indicating an incorrect email format.
        /// </summary>
        InvalidCharacterInSequence,

        /// <summary>
        /// An invalid quoted word with no content was found, indicating an incorrect email format.
        /// </summary>
        InvalidEmptyQuotedWord,

        /// <summary>
        /// An invalid folding white space (FWS) sequence was found, indicating an incorrect email format.
        /// </summary>
        InvalidFoldingWhiteSpaceSequence,

        /// <summary>
        /// The local part of the e-mail address has an invalid length, making it impossible to verify.
        /// </summary>
        InvalidLocalPartLength,

        /// <summary>
        /// A new word boundary start was detected at an invalid position, indicating an incorrect email format.
        /// </summary>
        InvalidWordBoundaryStart,

        /// <summary>
        /// The email address does not comply with the additional syntax rules of the email service provider that manages it.
        /// </summary>
        IspSpecificSyntaxFailure,

        /// <summary>
        /// The external mail exchanger responsible for the email address under test rejected the local endpoint, likely
        /// due to policy rules.
        /// </summary>
        LocalEndPointRejected,

        /// <summary>
        /// The local part of the email address is a well-known role account.
        /// </summary>
        LocalPartIsWellKnownRoleAccount,

        /// <summary>
        /// The external mail exchanger rejected the verification request.
        /// </summary>
        LocalSenderAddressRejected,

        /// <summary>
        /// A connection error occurred while validating the mailbox for the email address.
        /// </summary>
        MailboxConnectionFailure,

        /// <summary>
        /// The mailbox for the email address does not exist.
        /// </summary>
        MailboxDoesNotExist,

        /// <summary>
        /// The requested mailbox is currently over quota.
        /// </summary>
        MailboxHasInsufficientStorage,

        /// <summary>
        /// While both the domain and the mail exchanger for the email address being tested are not from a well-known
        /// disposable email address provider (DEA), the mailbox is actually disposable.
        /// </summary>
        MailboxIsDea,

        /// <summary>
        /// The requested mailbox is temporarily unavailable; it could be experiencing technical issues or some other transient problem.
        /// </summary>
        MailboxTemporarilyUnavailable,

        /// <summary>
        /// A timeout occurred while verifying the existence of the mailbox.
        /// </summary>
        MailboxValidationTimeout,

        /// <summary>
        /// The mail exchanger responsible for the email address under test hides a honeypot / spam trap.
        /// </summary>
        MailExchangerIsHoneypot,
        
        /// <summary>
        /// The mail exchanger responsible for the email address is parked or inactive.
        /// </summary>
        MailExchangerIsParked,

        /// <summary>
        /// The mail exchanger being tested is a well-known disposable email address provider (DEA).
        /// </summary>
        MailExchangerIsWellKnownDea,

        /// <summary>
        /// The external mail exchanger does not support international mailbox names. To support this feature, mail exchangers must comply with
        /// <a href="http://www.ietf.org/rfc/rfc5336.txt">RFC 5336</a> and support and announce both the 8BITMIME and the UTF8SMTP protocol extensions.
        /// </summary>
        ServerDoesNotSupportInternationalMailboxes,
        
        /// <summary>
        /// The external mail exchanger accepts fake, nonexistent email addresses; therefore the provided email address may be nonexistent too.
        /// </summary>
        ServerIsCatchAll,

        /// <summary>
        /// The mail exchanger responsible for the email address under test is temporarily unavailable.
        /// </summary>
        ServerTemporaryUnavailable,

        /// <summary>
        /// A socket connection error occurred while connecting to the mail exchanger which serves the email address domain.
        /// </summary>
        SmtpConnectionFailure,

        /// <summary>
        /// A timeout has occurred while connecting to the mail exchanger that serves the email address domain.
        /// </summary>
        SmtpConnectionTimeout,

        /// <summary>
        /// The mail exchanger responsible for the email address under test replied with one or more non-standard SMTP responses that
        /// caused the SMTP session to be aborted.
        /// </summary>
        SmtpDialogError,

        /// <summary>
        /// The email address has been successfully verified: it is deliverable and can accept email messages.
        /// </summary>
        Success,

        /// <summary>
        /// The domain literal of the email address cannot accept messages from the Internet.
        /// </summary>
        UnacceptableDomainLiteral,

        /// <summary>
        /// The number of parentheses used to open comments is not equal to the number used to close them.
        /// </summary>
        UnbalancedCommentParenthesis,

        /// <summary>
        /// An unexpected quoted pair sequence has been found within a quoted word.
        /// </summary>
        UnexpectedQuotedPairSequence,

        /// <summary>
        /// One or more unhandled exceptions were thrown during the verification process and something went wrong
        /// on the Verifalia side.
        /// </summary>
        UnhandledException,

        /// <summary>
        /// A quoted pair within a quoted word is not closed properly.
        /// </summary>
        UnmatchedQuotedPair,
        
        /// <summary>
        /// The system assigned a user-defined classification because the input data met the criteria specified in a
        /// custom classification override rule.
        /// </summary>
        OverrideMatch,
    }
}