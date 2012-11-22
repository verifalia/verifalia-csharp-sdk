using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verifalia.Api.Models
{
    /// <summary>
    /// Represents a single validated entry within an email validation batch.
    /// </summary>
    public class EmailValidationEntry
    {
        /// <summary>
        /// The input string to validate.
        /// </summary>
        public string InputData { get; set; }

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
        /// Gets detailed status information for the validation result.
        /// </summary>
        public EmailValidationEntryStatus Status { get; set; }

        /// <summary>
        /// Gets the position of the character in the email address that eventually caused the syntax validation to fail.
        /// </summary>
        /// <remarks>This property returns <see langword="null">null</see> when there is not a <see cref="IsSyntaxFailure">syntax failure</see>.</remarks>
        public int? SyntaxFailureIndex { get; set; }


        #region Shortcut Is* properties

        /// <summary>
        /// Gets a value indicating whether a problem with the fake address rejection validation occurred, including:
        /// <list type="bullet">
        ///     <item><see cref="EmailValidationEntryStatus.CatchAllConnectionFailure">CatchAllConnectionFailure</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.CatchAllValidationTimeout">CatchAllValidationTimeout</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.ServerIsCatchAll">ServerIsCatchAll</see></item>
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
        ///     <item><see cref="EmailValidationEntryStatus.CatchAllValidationTimeout">CatchAllValidationTimeout</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.DnsQueryTimeout">DnsQueryTimeout</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.HttpConnectionTimeout">HttpConnectionTimeout</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.MailboxValidationTimeout">MailboxValidationTimeout</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.ProxyConnectionTimeout">ProxyConnectionTimeout</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.SmtpConnectionTimeout">SmtpConnectionTimeout</see></item>
        /// </list>
        /// </summary>
        public bool? IsTimeoutFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether a failure in the network connection occured while verifying the email address, including:
        /// <list type="bullet">
        ///     <item><see cref="EmailValidationEntryStatus.SmtpConnectionFailure">SmtpConnectionFailure</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.MailboxConnectionFailure">MailboxConnectionFailure</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.DnsConnectionFailure">DnsConnectionFailure</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.CatchAllConnectionFailure">CatchAllConnectionFailure</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.HttpConnectionFailure">HttpConnectionFailure</see></item>
        /// </list>
        /// </summary>
        public bool? IsNetworkFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether a syntax error in the email address has been found, including:
        /// <list type="bullet">
        ///     <item><see cref="EmailValidationEntryStatus.AtSignNotFound">AtSignNotFound</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.DomainPartCompliancyFailure">DomainPartCompliancyFailure</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.DoubleDotSequence">DoubleDotSequence</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.InvalidAddressLength">InvalidAddressLength</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.InvalidCharacterInSequence">InvalidCharacterInSequence</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.InvalidEmptyQuotedWord">InvalidEmptyQuotedWord</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.InvalidFoldingWhiteSpaceSequence">InvalidFoldingWhiteSpaceSequence</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.InvalidLocalPartLength">InvalidLocalPartLength</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.InvalidWordBoundaryStart">InvalidWordBoundaryStart</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.UnbalancedCommentParenthesis">UnbalancedCommentParenthesis</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.UnexpectedQuotedPairSequence">UnexpectedQuotedPairSequence</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.UnmatchedQuotedPair">UnmatchedQuotedPair</see></item>
        /// </list>
        /// </summary>
        public bool? IsSyntaxFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether a DNS-related issue occurred, including:
        /// <list type="bullet">
        ///     <item><see cref="EmailValidationEntryStatus.DnsQueryTimeout">DnsQueryTimeout</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.DomainDoesNotExist">DomainDoesNotExist</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.DnsConnectionFailure">DnsConnectionFailure</see></item>
        /// </list>
        /// </summary>
        public bool? IsDnsFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether the email address has been provided by a well known
        /// disposable e-mail address (DEA) provider.
        /// </summary>
        public bool? IsDisposableEmailAddressFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether a problem with the SMTP validation of the email address occurred, including:
        /// <list type="bullet">
        ///     <item><see cref="EmailValidationEntryStatus.SmtpConnectionFailure">SmtpConnectionFailure</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.SmtpConnectionTimeout">SmtpConnectionTimeout</see></item>
        /// </list>
        /// </summary>
        public bool? IsSmtpFailure { get; set; }

        /// <summary>
        /// Gets a value indicating whether a problem with the mailbox validation of the email address occurred, including:
        /// <list type="bullet">
        ///     <item><see cref="EmailValidationEntryStatus.MailboxConnectionFailure">MailboxConnectionFailure</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.MailboxDoesNotExist">MailboxDoesNotExist</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.MailboxTemporarilyUnavailable">MailboxTemporarilyUnavailable</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.MailboxValidationTimeout">MailboxValidationTimeout</see></item>
        ///     <item><see cref="EmailValidationEntryStatus.ServerDoesNotSupportInternationalMailboxes">ServerDoesNotSupportInternationalMailboxes</see></item>
        /// </list>
        /// </summary>
        public bool? IsMailboxFailure { get; set; }

        #endregion
    }
}
