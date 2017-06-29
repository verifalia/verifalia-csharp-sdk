using System;

namespace Verifalia.Api.EmailAddresses.Models
{
    /// <summary>
    /// Represents a single validated entry within an email validation batch.
    /// </summary>
    public class ValidationEntry
    {
        /// <summary>
        /// The input string to validate.
        /// </summary>
        public string InputData { get; set; }

        /// <summary>
        /// A custom, optional string which is passed back upon completing the validation.
        /// </summary>
        public string Custom { get; set; }

        /// <summary>
        /// The date this entry has been completed.
        /// </summary>
        public DateTime? CompletedOn { get; set; }

        /// <summary>
        /// Gets the email address, without comments and folding white spaces.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets the domain part of the email address, converted to ASCII if needed and with comments and folding
        /// white spaces stripped off.
        /// <remarks>The ASCII encoding is performed using the standard <see cref="http://en.wikipedia.org/wiki/Punycode">punycode algorithm</see>.</remarks>
        /// </summary>
        public string AsciiEmailAddressDomainPart { get; set; }

        /// <summary>
        /// Gets the local part of the email address, without comments and folding white spaces.
        /// </summary>
        public string EmailAddressLocalPart { get; set; }
        
        /// <summary>
        /// Gets the domain part of the email address, without comments and folding white spaces.
        /// </summary>
        public string EmailAddressDomainPart { get; set; }

        /// <summary>
        /// Gets a logical value indicating whether the email address has an international domain name or not.
        /// </summary>
        public bool? HasInternationalDomainName { get; set; }

        /// <summary>
        /// Gets a logical value indicating whether the email address has an international mailbox name or not.
        /// </summary>
        public bool? HasInternationalMailboxName { get; set; }

        /// <summary>
        /// Returns a value indicating whether the email address comes from a disposable email address provider or not.
        /// </summary>
        public bool? IsDisposableEmailAddress { get; set; }

        /// <summary>
        /// Returns a value indicating whether the email address comes from a free email address provider (e.g. gmail, yahoo, outlook / hotmail, ...) or not.
        /// </summary>
        public bool? IsFreeEmailAddress { get; set; }

        /// <summary>
        /// Returns a value indicating whether the local part of the email address is a well-known role account or not.
        /// </summary>
        public bool? IsRoleAccount { get; set; }

        /// <summary>
        /// Gets detailed status information for the validation result.
        /// </summary>
        public ValidationEntryStatus Status { get; set; }

        /// <summary>
        /// Gets the position of the character in the email address that eventually caused the syntax validation to fail.
        /// </summary>
        /// <remarks>This property returns <see langword="null">null</see> when there is not a <see cref="IsSyntaxFailure">syntax failure</see>.</remarks>
        public int? SyntaxFailureIndex { get; set; }

        /// <summary>
        /// The zero-based index of the first occurrence of this email address in this validation job, in the event the Status
        /// for this entry equals to Duplicated; duplicated items do not expose any result detail apart from this pointer to the
        /// aforementioned first occurrence entry and the eventual custom state.
        /// </summary>
        public int? DuplicateOf { get; set; }

        #region Shortcut Is* properties

        /// <summary>
        /// Gets a value indicating whether a problem with the fake address rejection validation occurred, including:
        /// <list type="bullet">
        ///     <item><see cref="ValidationEntryStatus.CatchAllConnectionFailure">CatchAllConnectionFailure</see></item>
        ///     <item><see cref="ValidationEntryStatus.CatchAllValidationTimeout">CatchAllValidationTimeout</see></item>
        ///     <item><see cref="ValidationEntryStatus.ServerIsCatchAll">ServerIsCatchAll</see></item>
        /// </list>
        /// </summary>
        public bool? IsCatchAllFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether the eemail address verification succeded or failed. In the latter case,
        /// detailed failure information is available through <see cref="Status">Status</see> property value.
        /// </summary>
        public bool? IsSuccess { get; set; }

        /// <summary>
        /// Gets a value indicating whether a timeout occured while verifying the email address, including:
        /// <list type="bullet">
        ///     <item><see cref="ValidationEntryStatus.CatchAllValidationTimeout">CatchAllValidationTimeout</see></item>
        ///     <item><see cref="ValidationEntryStatus.DnsQueryTimeout">DnsQueryTimeout</see></item>
        ///     <item><see cref="ValidationEntryStatus.HttpConnectionTimeout">HttpConnectionTimeout</see></item>
        ///     <item><see cref="ValidationEntryStatus.MailboxValidationTimeout">MailboxValidationTimeout</see></item>
        ///     <item><see cref="ValidationEntryStatus.SmtpConnectionTimeout">SmtpConnectionTimeout</see></item>
        /// </list>
        /// </summary>
        public bool? IsTimeoutFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether a failure in the network connection occured while verifying the email address, including:
        /// <list type="bullet">
        ///     <item><see cref="ValidationEntryStatus.SmtpConnectionFailure">SmtpConnectionFailure</see></item>
        ///     <item><see cref="ValidationEntryStatus.MailboxConnectionFailure">MailboxConnectionFailure</see></item>
        ///     <item><see cref="ValidationEntryStatus.DnsConnectionFailure">DnsConnectionFailure</see></item>
        ///     <item><see cref="ValidationEntryStatus.CatchAllConnectionFailure">CatchAllConnectionFailure</see></item>
        ///     <item><see cref="ValidationEntryStatus.HttpConnectionFailure">HttpConnectionFailure</see></item>
        /// </list>
        /// </summary>
        public bool? IsNetworkFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether a syntax error in the email address has been found, including:
        /// <list type="bullet">
        ///     <item><see cref="ValidationEntryStatus.AtSignNotFound">AtSignNotFound</see></item>
        ///     <item><see cref="ValidationEntryStatus.DomainPartCompliancyFailure">DomainPartCompliancyFailure</see></item>
        ///     <item><see cref="ValidationEntryStatus.DoubleDotSequence">DoubleDotSequence</see></item>
        ///     <item><see cref="ValidationEntryStatus.InvalidAddressLength">InvalidAddressLength</see></item>
        ///     <item><see cref="ValidationEntryStatus.InvalidCharacterInSequence">InvalidCharacterInSequence</see></item>
        ///     <item><see cref="ValidationEntryStatus.InvalidEmptyQuotedWord">InvalidEmptyQuotedWord</see></item>
        ///     <item><see cref="ValidationEntryStatus.InvalidFoldingWhiteSpaceSequence">InvalidFoldingWhiteSpaceSequence</see></item>
        ///     <item><see cref="ValidationEntryStatus.InvalidLocalPartLength">InvalidLocalPartLength</see></item>
        ///     <item><see cref="ValidationEntryStatus.InvalidWordBoundaryStart">InvalidWordBoundaryStart</see></item>
        ///     <item><see cref="ValidationEntryStatus.UnbalancedCommentParenthesis">UnbalancedCommentParenthesis</see></item>
        ///     <item><see cref="ValidationEntryStatus.UnexpectedQuotedPairSequence">UnexpectedQuotedPairSequence</see></item>
        ///     <item><see cref="ValidationEntryStatus.UnmatchedQuotedPair">UnmatchedQuotedPair</see></item>
        /// </list>
        /// </summary>
        public bool? IsSyntaxFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether a DNS-related issue occurred, including:
        /// <list type="bullet">
        ///     <item><see cref="ValidationEntryStatus.DnsQueryTimeout">DnsQueryTimeout</see></item>
        ///     <item><see cref="ValidationEntryStatus.DomainDoesNotExist">DomainDoesNotExist</see></item>
        ///     <item><see cref="ValidationEntryStatus.DnsConnectionFailure">DnsConnectionFailure</see></item>
        /// </list>
        /// </summary>
        public bool? IsDnsFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether a problem with the SMTP validation of the email address occurred, including:
        /// <list type="bullet">
        ///     <item><see cref="ValidationEntryStatus.SmtpConnectionFailure">SmtpConnectionFailure</see></item>
        ///     <item><see cref="ValidationEntryStatus.SmtpConnectionTimeout">SmtpConnectionTimeout</see></item>
        /// </list>
        /// </summary>
        public bool? IsSmtpFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether a problem with the mailbox validation of the email address occurred, including:
        /// <list type="bullet">
        ///     <item><see cref="ValidationEntryStatus.MailboxConnectionFailure">MailboxConnectionFailure</see></item>
        ///     <item><see cref="ValidationEntryStatus.MailboxDoesNotExist">MailboxDoesNotExist</see></item>
        ///     <item><see cref="ValidationEntryStatus.MailboxTemporarilyUnavailable">MailboxTemporarilyUnavailable</see></item>
        ///     <item><see cref="ValidationEntryStatus.MailboxValidationTimeout">MailboxValidationTimeout</see></item>
        ///     <item><see cref="ValidationEntryStatus.ServerDoesNotSupportInternationalMailboxes">ServerDoesNotSupportInternationalMailboxes</see></item>
        /// </list>
        /// </summary>
        public bool? IsMailboxFailure { get; set; }

        #endregion
    }
}
