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

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// Provides enumerated values for the supported validation statuses for a <see cref="ValidationEntry"/>.
    /// </summary>
    public enum ValidationEntryStatus
    {
        /// <summary>
        /// Special unknown validation status, due to a value reported by the API which is missing in this SDK.
        /// </summary>
        Unknown = 0,
        
        /// <summary>
        /// The at sign symbol (@), used to separate the local part from the domain part of the address, has not been found.
        /// </summary>
        AtSignNotFound,

        /// <summary>
        /// A connection error occurred while verifying the external mail exchanger rejects nonexistent email addresses.
        /// </summary>
        CatchAllConnectionFailure,
        
        /// <summary>
        /// A timeout occurred while verifying fake e-mail address rejection for the mail server.
        /// </summary>
        CatchAllValidationTimeout,

        /// <summary>
        /// Verification failed because of a socket connection error occurred while querying the DNS server.
        /// </summary>
        DnsConnectionFailure,

        /// <summary>
        /// A timeout has occurred while querying the DNS server(s) for records about the email address domain.
        /// </summary>
        DnsQueryTimeout,

        /// <summary>
        /// The domain of the email address does not exist.
        /// </summary>
        DomainDoesNotExist,

        /// <summary>
        /// The domain has a NULL MX (RFC 7505) resource record and can't thus accept email messages.
        /// </summary>
        DomainHasNullMx,

        /// <summary>
        /// The domain of the email address does not have any valid DNS record and couldn't accept messages from another
        /// host on the Internet.
        /// </summary>
        DomainIsMisconfigured,
        
        /// <summary>
        /// The email address is provided by a well-known disposable email address provider (DEA).
        /// </summary>
        DomainIsWellKnownDea,

        /// <summary>
        /// The domain part of the email address is not compliant with the IETF standards.
        /// </summary>
        DomainPartCompliancyFailure,
        
        /// <summary>
        /// An invalid sequence of two adjacent dots has been found.
        /// </summary>
        DoubleDotSequence,

        /// <summary>
        /// The item is a duplicate of another email address in the list.
        /// </summary>
        /// <remarks>To find out the entry this item is a duplicate of, check the <see cref="ValidationEntry.DuplicateOf"/> property for the <see cref="ValidationEntry"/>
        /// instance which exposes this status code</remarks>
        Duplicate,

        /// <summary>
        /// The email address has an invalid total length.
        /// </summary>
        InvalidAddressLength,

        /// <summary>
        /// An invalid character has been detected in the provided sequence.
        /// </summary>
        InvalidCharacterInSequence,

        /// <summary>
        /// An invalid quoted word with no content has been found.
        /// </summary>
        InvalidEmptyQuotedWord,

        /// <summary>
        /// An invalid folding white space (FWS) sequence has been found.
        /// </summary>
        InvalidFoldingWhiteSpaceSequence,

        /// <summary>
        /// The local part of the e-mail address has an invalid length.
        /// </summary>
        InvalidLocalPartLength,

        /// <summary>
        /// A new word boundary start has been detected at an invalid position.
        /// </summary>
        InvalidWordBoundaryStart,

        /// <summary>
        /// The email address is not compliant with the additional syntax rules of the email service provider
        /// which should eventually manage it.
        /// </summary>
        IspSpecificSyntaxFailure,

        /// <summary>
        /// The external mail exchanger responsible for the email address under test rejected the local endpoint, probably because
        /// of its own policy rules.
        /// </summary>
        LocalEndPointRejected,

        /// <summary>
        /// The local part of the email address is a well-known role account.
        /// </summary>
        LocalPartIsWellKnownRoleAccount,

        /// <summary>
        /// The external mail exchanger rejected the validation request.
        /// </summary>
        LocalSenderAddressRejected,

        /// <summary>
        /// A connection error occurred while validating the mailbox for the e-mail address.
        /// </summary>
        MailboxConnectionFailure,

        /// <summary>
        /// The mailbox for the e-mail address does not exist.
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
        /// The mail exchanger responsible for the email address is parked / inactive.
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
        /// The external mail exchanger accepts fake, non existent, email addresses; therefore the provided email address MAY be nonexistent too.
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
        /// A timeout has occurred while connecting to the mail exchanger which serves the email address domain.
        /// </summary>
        SmtpConnectionTimeout,

        /// <summary>
        /// The mail exchanger responsible for the email address under test replied one or more non-standard SMTP replies which
        /// caused the SMTP session to be aborted.
        /// </summary>
        SmtpDialogError,

        /// <summary>
        /// The email address has been successfully validated.
        /// </summary>
        Success,

        /// <summary>
        /// The domain literal of the email address couldn't accept messages from the Internet.
        /// </summary>
        UnacceptableDomainLiteral,

        /// <summary>
        /// The number of parenthesis used to open comments is not equal to the one used to close them.
        /// </summary>
        UnbalancedCommentParenthesis,

        /// <summary>
        /// An unexpected quoted pair sequence has been found within a quoted word.
        /// </summary>
        UnexpectedQuotedPairSequence,

        /// <summary>
        /// One or more unhandled exceptions have been thrown during the verification process and something went wrong
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