namespace Verifalia.Api.EmailAddresses.Models
{
    /// <summary>
    /// Provides enumerated values that specify the supported statuses of a single email address validation entry.
    /// </summary>
    public enum ValidationEntryStatus
    {
        /// <summary>
        /// Unknown validation status, due to a value reported by the API which is missing in this SDK.
        /// </summary>
        Unknown,

        /// <summary>
        /// The email address has been successfully validated.
        /// </summary>
        Success,

        #region Syntax related failures

        /// <summary>
        /// A quoted pair within a quoted word is not closed properly.
        /// </summary>
        UnmatchedQuotedPair,

        /// <summary>
        /// An unexpected quoted pair sequence has been found within a quoted word.
        /// </summary>
        UnexpectedQuotedPairSequence,

        /// <summary>
        /// A new word boundary start has been detected at an invalid position.
        /// </summary>
        InvalidWordBoundaryStart,

        /// <summary>
        /// An invalid character has been detected in the provided sequence.
        /// </summary>
        InvalidCharacterInSequence,

        /// <summary>
        /// The number of parenthesis used to open comments is not equal to the one used to close them.
        /// </summary>
        UnbalancedCommentParenthesis,

        /// <summary>
        /// An invalid sequence of two adjacent dots has been found.
        /// </summary>
        DoubleDotSequence,

        /// <summary>
        /// The local part of the e-mail address has an invalid length.
        /// </summary>
        InvalidLocalPartLength,

        /// <summary>
        /// An invalid folding white space (FWS) sequence has been found.
        /// </summary>
        InvalidFoldingWhiteSpaceSequence,

        /// <summary>
        /// The at sign symbol (@), used to separate the local part from the domain part of the address, has not been found.
        /// </summary>
        AtSignNotFound,

        /// <summary>
        /// An invalid quoted word with no content has been found.
        /// </summary>
        InvalidEmptyQuotedWord,

        /// <summary>
        /// The email address has an invalid total length.
        /// </summary>
        InvalidAddressLength,

        /// <summary>
        /// The domain part of the email address is not compliant with the IETF standards.
        /// </summary>
        DomainPartCompliancyFailure,

        /// <summary>
        /// The email address is not compliant with the additional syntax rules of the ISP which should eventually manage it.
        /// </summary>
        IspSpecificSyntaxFailure,

        #endregion

        /// <summary>
        /// The local part of the email address is a well-known role account.
        /// </summary>
        LocalPartIsWellKnownRoleAccount,

        #region DNS failures

        /// <summary>
        /// A timeout has occured while querying the DNS server(s) for records about the email address domain.
        /// </summary>
        DnsQueryTimeout,

        /// <summary>
        /// Verification failed because of a socket connection error occured while querying the DNS server.
        /// </summary>
        DnsConnectionFailure,

        /// <summary>
        /// The domain of the email address does not exist.
        /// </summary>
        DomainDoesNotExist,

        /// <summary>
        /// The domain of the email address does not have any valid DNS record and couldn't accept messages from another
        /// host on the Internet.
        /// </summary>
        DomainIsMisconfigured,

        #endregion

        #region SMTP failures

        /// <summary>
        /// A timeout has occured while connecting to the mail exchanger which serves the email address domain.
        /// </summary>
        SmtpConnectionTimeout,

        /// <summary>
        /// A socket connection error occured while connecting to the mail exchanger which serves the email address domain.
        /// </summary>
        SmtpConnectionFailure,

        #endregion

        #region Mailbox verification failures

        /// <summary>
        /// The mailbox for the e-mail address does not exist.
        /// </summary>
        MailboxDoesNotExist,

        /// <summary>
        /// A connection error occurred while validating the mailbox for the e-mail address.
        /// </summary>
        MailboxConnectionFailure,

        /// <summary>
        /// The external mail exchanger rejected the validation request.
        /// </summary>
        LocalSenderAddressRejected,

        /// <summary>
        /// A timeout occured while verifying the existence of the mailbox.
        /// </summary>
        MailboxValidationTimeout,

        /// <summary>
        /// The requested mailbox is temporarily unavailable; it could be experiencing technical issues or some other transient problem
        /// (could be over quota, for example).
        /// </summary>
        MailboxTemporarilyUnavailable,

        /// <summary>
        /// The external mail exchanger does not support international mailbox names. To support this feature, mail exchangers must comply with
        /// <a href="http://www.ietf.org/rfc/rfc5336.txt">RFC 5336</a> and support and announce both the 8BITMIME and the UTF8SMTP protocol extensions.
        /// </summary>
        ServerDoesNotSupportInternationalMailboxes,

        #endregion

        #region Catch-all rejection failures

        /// <summary>
        /// A timeout occured while verifying fake e-mail address rejection for the mail server.
        /// </summary>
        CatchAllValidationTimeout,

        /// <summary>
        /// The external mail exchanger accepts fake, non existent, email addresses; therefore the provided email address MAY be inexistent too.
        /// </summary>
        ServerIsCatchAll,

        /// <summary>
        /// A connection error occurred while verifying the external mail exchanger rejects inexistent email addresses.
        /// </summary>
        CatchAllConnectionFailure,

        #endregion

        #region HTTP failures

        /// <summary>
        /// A timeout has occured while connecting to the HTTP server (webmail) which serves the e-mail address domain.
        /// </summary>
        HttpConnectionTimeout,

        /// <summary>
        /// A socket connection error occured while connecting to the HTTP server (webmail) which serves the e-mail address domain.
        /// </summary>
        HttpConnectionFailure,

        #endregion

        /// <summary>
        /// The mail exchanger is temporarily unavailable.
        /// </summary>
        ServerTemporaryUnavailable,

        /// <summary>
        /// The external mail exchanger replied one or more non-standard SMTP lines and caused the SMTP session to be aborted.
        /// </summary>
        SmtpDialogError,

        /// <summary>
        /// The external mail exchanger rejected the local endpoint, probably because of its own policy rules.
        /// </summary>
        LocalEndPointRejected,

        /// <summary>
        /// One or more unhandled exceptions have been thrown during the verification process and something went wrong
        /// on the Verifalia side.
        /// </summary>
        UnhandledException
    }
}
