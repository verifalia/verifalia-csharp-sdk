![Verifalia API](https://img.shields.io/badge/Verifalia%20API-v2.7-green)
[![NuGet](https://img.shields.io/nuget/v/Verifalia.svg)](https://www.nuget.org/packages/Verifalia)
![License](https://img.shields.io/badge/License-MIT-yellow.svg)

# Verifalia API - .NET SDK and Helper Library

[Verifalia](https://verifalia.com/) provides a fast and accurate API for verifying email addresses in real-time to determine their deliverability. This SDK library integrates seamlessly with Verifalia and allows you to [verify email addresses](https://verifalia.com/) and manage most account settings across the following platforms:

- .NET 5.0 and higher, including **.NET 10.0**
- .NET Core 1.0 and higher
- .NET Framework 4.5 and higher
- .NET Standard 1.3 and higher
  - Mono 4.6+
  - Xamarin.iOS 10.0+
  - Xamarin.Mac 3.0+
  - Xamarin.Android 7.0+
  - Universal Windows Platform 10.0+

## ⭐️⭐️⭐️ Enjoying This Library? Please Star Our Repo! ⭐️⭐️⭐️

If this SDK helps you ship features faster, we'd love your support! By starring our [GitHub repository](https://github.com/verifalia/verifalia-csharp-sdk/), you help us gauge how widely it's used and which features and improvements to prioritize next. Every star makes a difference — thank you for helping steer the future of this SDK!

To learn more about Verifalia, please visit [https://verifalia.com](https://verifalia.com/).

---

# Table of Contents

<!-- TOC -->
* [Adding Verifalia API Support to Your .NET Solution](#adding-verifalia-api-support-to-your-net-solution)
  * [From an IDE](#from-an-ide)
    * [Rider](#rider)
    * [Visual Studio](#visual-studio)
    * [Visual Studio Code](#visual-studio-code)
  * [Manual Download and Compilation](#manual-download-and-compilation)
* [Authentication](#authentication)
  * [Basic Authentication](#basic-authentication)
  * [Bearer Token Authentication](#bearer-token-authentication)
  * [X.509 Client Certificate (TLS Mutual Authentication)](#x509-client-certificate-tls-mutual-authentication)
* [Email Verifications](#email-verifications)
  * [How to Verify an Email Address](#how-to-verify-an-email-address)
  * [How to Verify a List of Email Addresses](#how-to-verify-a-list-of-email-addresses)
  * [How to Import and Verify a File of Email Addresses](#how-to-import-and-verify-a-file-of-email-addresses)
  * [Processing Options](#processing-options)
    * [Quality Level](#quality-level)
    * [Deduplication Mode](#deduplication-mode)
    * [Data Retention](#data-retention)
  * [Wait Options](#wait-options)
    * [Avoid Waiting](#avoid-waiting)
    * [Progress Tracking](#progress-tracking)
  * [Completion Callbacks](#completion-callbacks)
  * [Retrieving Jobs](#retrieving-jobs)
  * [Exporting Email Verification Results in Different Output Formats](#exporting-email-verification-results-in-different-output-formats)
  * [Don't Forget to Clean Up When You're Done](#dont-forget-to-clean-up-when-youre-done)
  * [Iterating Over Your Email Verification Jobs](#iterating-over-your-email-verification-jobs)
    * [Filtering Email Verification Jobs](#filtering-email-verification-jobs)
* [Managing Credits](#managing-credits)
  * [Getting the Credits Balance](#getting-the-credits-balance)
  * [Retrieving Credits Usage Statistics](#retrieving-credits-usage-statistics)
* [Users and Team Management](#users-and-team-management)
  * [Listing Users](#listing-users)
  * [Retrieving User Details](#retrieving-user-details)
  * [Creating Users](#creating-users)
  * [Updating Users](#updating-users)
  * [Deleting Users](#deleting-users)
* [X.509 Client Certificates](#x509-client-certificates)
  * [Listing Certificates](#listing-certificates)
  * [Creating Certificates](#creating-certificates)
  * [Deleting Certificates](#deleting-certificates)
* [Contact Methods](#contact-methods)
  * [Listing Contact Methods](#listing-contact-methods)
  * [Retrieving Contact Methods](#retrieving-contact-methods)
  * [Creating Contact Methods](#creating-contact-methods)
    * [Activating Contact Methods](#activating-contact-methods)
  * [Updating Contact Methods](#updating-contact-methods)
  * [Deleting Contact Methods](#deleting-contact-methods)
* [Changelog / What's New](#changelog--whats-new)
  * [v5.1](#v51)
  * [v5.0](#v50)
<!-- TOC -->

# Adding Verifalia API Support to Your .NET Solution

This SDK is available as a NuGet package, the package manager for .NET. The best and easiest way to add the SDK library to your .NET project is to use the NuGet package manager in your favorite IDE.

## From an IDE

### Rider

From within JetBrains Rider, you can use the NuGet window or NuGet Quick List to search for and install the **Verifalia** NuGet package.

### Visual Studio

From within Visual Studio, you can use the NuGet GUI to search for and install the **Verifalia** NuGet package. Alternatively, as a shortcut, simply type the following command into the Package Manager Console:

```
Install-Package Verifalia
```

### Visual Studio Code

From within Visual Studio Code, you can install the **Verifalia** NuGet package using the C# Dev Kit Solution Explorer or the Command Palette.

## Manual Download and Compilation
	
As an alternative to using NuGet, you can [download the SDK source project from GitHub](https://github.com/verifalia/verifalia-csharp-sdk/archive/master.zip), extract it to a folder of your choice, and add a reference from your project to the Verifalia SDK project. The SDK project is written in C# but can be referenced and used with any other .NET language, including Visual Basic (VB.NET), C++/CLI, J#, IronPython, IronRuby, F#, and PowerShell.

Learn more at [https://verifalia.com](https://verifalia.com/).

# Authentication

Authentication to the Verifalia API is performed using the credentials of a Verifalia user account. While you can use the same credentials you used to register your Verifalia account for API access, for security reasons, we strongly recommend [creating and using a dedicated user account](https://app.verifalia.com/#/users/new) for API operations. This approach allows you to assign only the specific permissions needed for your application.

If you don't have a Verifalia account, you can [register for a free one](https://verifalia.com/sign-up). 

The Verifalia API supports multiple authentication methods:
- **Basic authentication**: Ideal for server-to-server communications
- **Bearer token authentication**: Useful for browser apps and client-to-server communications  
- **X.509 client certificate**: Provides the highest level of security

The sections below describe each authentication method in detail. For more information about authenticating to the Verifalia API, see the API reference documentation at [https://verifalia.com/developers#authentication](https://verifalia.com/developers/authentication).

## Basic Authentication

Once you have your Verifalia credentials, use them to create a new instance of the `VerifaliaClient` class, which serves as the entry point for all operations against the Verifalia API. The supplied credentials are automatically provided to the API using HTTP Basic Authentication.

```c#
using Verifalia.Api;

var verifalia = new VerifaliaClient("username", "password");
```

## Bearer Token Authentication

Bearer authentication offers enhanced security over HTTP Basic Authentication. While Basic Auth requires sending actual credentials with each API call, Bearer authentication only requires credentials for an initial, dedicated authentication request. However, the initial authentication request required by Bearer authentication takes additional time. If you need to perform only a single request, HTTP Basic Authentication provides the same degree of security and is faster.

```c#
using Verifalia.Api;
using Verifalia.Api.Security;

var bearerAuthProvider = new BearerAuthenticationProvider("username", "password");
var verifalia = new VerifaliaClient(bearerAuthProvider);
```

Multi-factor authentication (MFA) is supported by implementing a custom `ITotpTokenProvider` interface, which acquires the time-based one-time password from an external authenticator app or device. To enable multi-factor authentication for your Verifalia account, configure your security settings in the [client area](https://app.verifalia.com/).

```c#
using Verifalia.Api;
using Verifalia.Api.Security;

class MyTotpProvider : ITotpTokenProvider
{
    public Task<string> ProvideTotpTokenAsync(CancellationToken cancellationToken)
    {
        // Prompt the user to enter their TOTP token
        Console.WriteLine("Enter your TOTP token:");
        var totpToken = Console.ReadLine();

        return Task.FromResult(totpToken);
    }
}

// ...

var totpTokenProvider = new MyTotpProvider();
var bearerAuthProvider = new BearerAuthenticationProvider("username", "password", totpTokenProvider);
var verifalia = new VerifaliaClient(bearerAuthProvider);
```

## X.509 Client Certificate (TLS Mutual Authentication)

This authentication method uses a cryptographic X.509 client certificate to authenticate against the Verifalia API through the TLS protocol. This method, also called mutual TLS authentication (mTLS) or two-way authentication, offers the highest degree of security because only a cryptographically-derived key (not the actual credentials) is sent over the wire with each request.

```c#
using System.Security.Cryptography.X509Certificates;
using Verifalia.Api;

var certificate = new X509Certificate2("mycertificate.pem");
var verifalia = new VerifaliaClient(certificate);
```

# Email Verifications

All operations related to verifying and validating email addresses are performed through the `EmailVerifications` property exposed by the `VerifaliaClient` instance you created above. This property provides useful methods and overloads. In the following sections, we'll examine the most commonly used ones. We strongly recommend exploring the library and consulting the embedded XML documentation for additional capabilities.

**Important**: The library automatically waits for email verification jobs to complete. If needed, you can adjust the wait options and have more control over the underlying polling process. Please refer to the [Wait Options](#wait-options) section below for additional details.

## How to Verify an Email Address

To validate an email address from a .NET application, invoke the `RunAsync()` method. It accepts one or more email addresses and any verification options you wish to pass to Verifalia, including expected result quality, deduplication preferences, and processing priority.

> **Note**  
> If you need to verify a list of email addresses, we recommend submitting them all at once using one of the dedicated `RunAsync()` method overloads (see the following sections) rather than iterating over the source set and submitting addresses individually. The batch approach is not only faster but also allows detection and marking of duplicate items—a feature unavailable when verifying email addresses one by one.

In the following example, we verify an email address using the default options:

```c#
var job = await verifalia
    .EmailVerifications
    .RunAsync("batman@gmail.com");

// At this point the address has been verified: let's print its email verification
// result to the console.

var entry = job.Entries[0];

Console.WriteLine($"Classification: {entry.Classification} (status: {entry.Status})");

// Classification: Deliverable (status: Success)
```

As expected, each entry includes various additional details about the verified email address:

| Property                      | Description                                                                                                                                                                                                                                                  |
|-------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `AsciiEmailAddressDomainPart` | The domain part of the email address, converted to ASCII if needed, with comments and folding white spaces stripped off.                                                                                                                             |
| `Classification`              | The `VerificationEntryClassification` value for this entry.                                                                                                                                                                                                  |
| `CompletedOn`                 | The date this entry was completed, if available.                                                                                                                                                                                                        |
| `Custom`                      | A custom, optional string that is passed back upon verification completion. To pass a custom value back and forth, use the `Custom` property of `VerificationRequestEntry`.                                                                             |
| `DuplicateOf`                 | The zero-based index of the first occurrence of this email address in the parent `Verification`, when the `Status` for this entry is `Duplicate`. Duplicated items only expose this value and any `Custom` values. |
| `EmailAddress`                | The email address without any comments or folding white space. Returns null if the input data is syntactically invalid.                                                                                                   |
| `EmailAddressDomainPart`      | The domain part of the email address, without comments and folding white spaces.                                                                                                                                                                        |
| `EmailAddressLocalPart`       | The local part of the email address, without comments and folding white spaces.                                                                                                                                                                         |
| `HasInternationalDomainName`  | True if the email address has an international domain name.                                                                                                                                                                                                 |
| `HasInternationalMailboxName` | True if the email address has an international mailbox name.                                                                                                                                                                                                |
| `Index`                       | The index of this entry within its `Verification` container. This property is primarily useful when the API returns a filtered view of the items.                                                                                                       |
| `InputData`                   | The input string being validated.                                                                                                                                                                                                                            |
| `IsDisposableEmailAddress`    | True if the email address comes from a disposable email address (DEA) provider. <a href="https://verifalia.com/help/email-validations/what-is-a-disposable-email-address-dea">What is a disposable email address?</a>                                       |
| `IsFreeEmailAddress`          | True if the email address comes from a free email address provider (e.g., Gmail, Yahoo, Outlook/Hotmail).                                                                                                                                             |
| `IsRoleAccount`               | True if the local part of the email address is a well-known role account.                                                                                                                                                                                   |
| `Status`                      | The `VerificationEntryStatus` value for this entry.                                                                                                                                                                                                          |
| `Suggestions`                 | Potential corrections for the input data, when Verifalia identifies possible typos during the verification process.                                                                                                                             |
| `SyntaxFailureIndex`          | The position of the character in the email address that caused syntax validation to fail, if applicable.                                                                                                                                                     |

Here's another example showing some of the additional result details provided by Verifalia:

```c#
var job = await verifalia
    .EmailVerifications
    .RunAsync("bat[man@gmal.com");

var entry = job.Entries[0];

Console.WriteLine($"Classification: {entry.Classification}");
Console.WriteLine($"Status: {entry.Status}");
Console.WriteLine($"Syntax failure index: {entry.SyntaxFailureIndex}");

if (entry.Suggestions != null)
{
    Console.WriteLine("Suggestions:");

    foreach (var suggestion in entry.Suggestions)
    {
        Console.WriteLine($"- {suggestion}");
    }
}

// Classification: Undeliverable
// Status: InvalidCharacterInSequence
// Syntax failure index: 3
// Suggestions:
// - batman@gmail.com
```

## How to Verify a List of Email Addresses

To verify a list of email addresses instead of a single address, use the `RunAsync()` method overload that accepts an `IEnumerable<string>`. If the email addresses are originally stored in a file, you can simply upload the file and have Verifalia automatically import and verify it—see the next section for details.

Here's an example showing how to verify an array of email addresses:

```c#
var job = await verifalia
    .EmailVerifications
    .RunAsync(new[] {
        "batman@gmail.com",
        "steve.vai@best.music",
        "samantha42@yahoo.it"
    });

Console.WriteLine($"Job ID: {job.Overview.Id}");

foreach (var entry in job.Entries)
{
    Console.WriteLine($"- {entry.InputData} => {entry.Classification} ({entry.Status})");
}

// Job Id: 290b5146-eeac-4a2b-a9c1-61c7e715f2e9
// - batman@gmail.com => Deliverable (Success)
// - steve.vai@best.music => Undeliverable (DomainIsMisconfigured)
// - samantha42@yahoo.it => Deliverable (Success)
```

## How to Import and Verify a File of Email Addresses

This library includes support for submitting and validating files containing email addresses, including:

- **Plain text files** (.txt), with one email address per line
- **Comma-separated values** (.csv), **tab-separated values** (.tsv), and other delimiter-separated value files
- **Microsoft Excel spreadsheets** (.xls and .xlsx)

To upload and verify a file, use the `RunAsync()` method mentioned above, passing either a `Stream`, `FileInfo` instance, or `byte[]` with the file content. You can also specify the starting and ending rows to process, the column, sheet index, line ending, and delimiter—depending on the nature of the submitted file. See `FileVerificationRequest` in the source code to learn more.

For example, here's how to verify a text file containing one email address per line:

```c#
var job = await verifalia
    .EmailVerifications
    .RunAsync(new FileInfo("emails.txt"));
```

To set more advanced options, including those governing the import process of workbooks with multiple sheets and columns, pass a `FileVerificationRequest` instance to the `RunAsync()` method and specify the sheet to process and the range of data to import:

```c#
var job = await verifalia
    .EmailVerifications
    .RunAsync(new FileVerificationRequest(new FileInfo("that-file.xlsx"))
        {
            Sheet = 3,
            StartingRow = 1,
            Column = 5
        },
        quality: QualityLevelName.High);
```

Here's another example showing how to submit a `Stream` instance while specifying the MIME content type of the file. The content type is automatically determined from the file extension when you pass a `FileInfo` instance:

```c#
Stream inputStream = ...; // TODO: Acquire the input data somehow

var job = await verifalia
    .EmailVerifications
    .RunAsync(inputStream,
        MediaTypeHeaderValue.Parse(WellKnownMimeContentTypes.TextPlain)); // text/plain
```

## Processing Options

When submitting one or more email addresses for verification, you can specify several options that affect both the behavior of the Verifalia processing engine and the verification flow from the API consumer's perspective.

### Quality Level

Verifalia offers three distinct quality levels—**Standard**, **High**, and **Extreme**—which determine how the email verification engine handles temporary undeliverability issues, slower mail exchangers, and other potentially transient problems that can affect verification result quality. The `RunAsync()` method overloads accept a `quality` parameter that allows you to specify the desired quality level. Here's an example showing how to verify an email address using the **High** quality level:

```c#
var job = await verifalia
    .EmailVerifications
    .RunAsync("batman@gmail.com", quality: QualityLevelName.High);
```

### Deduplication Mode

The `RunAsync()` method overloads that accept multiple email addresses allow you to specify how to handle duplicate entries within the same input set. Verifalia supports a **Safe** deduplication mode, which strictly adheres to older IETF standards, and a **Relaxed** mode that aligns with what's commonly found in today's mail server configurations.

In this example, we show how to import and verify a list of email addresses while marking duplicate entries using the **Relaxed** deduplication mode:

```c#
var job = await verifalia
    .EmailVerifications
    .RunAsync(new FileInfo("that-file.xlsx"), deduplication: DeduplicationMode.Relaxed);
```

### Data Retention

Verifalia automatically deletes completed email verification jobs according to the data retention policy defined at the account level, which can be overridden at the user level. You can configure these settings in the [Verifalia client area](https://app.verifalia.com).

You can also specify a per-job data retention policy that governs the time-to-live of a submitted email verification job. To do this, use the `RunAsync()` method overloads that accept either a `VerificationRequest` or `FileVerificationRequest` instance and initialize its `Retention` property accordingly.

Here's how to set a data retention policy of 10 minutes while verifying an email address:

```c#
var job = await verifalia
    .EmailVerifications
    .RunAsync(new VerificationRequest(new[]
    {
        "batman@gmail.com"
    })
    {
        Retention = TimeSpan.FromMinutes(10)
    });
```

## Wait Options

By default, the `RunAsync()` method overloads submit an email verification job to Verifalia and wait for its completion. The entire process may take some time depending on your Verifalia account plan, the number of email addresses in the submission, the specified quality level, and other network factors including the latency of mail exchangers under test.

While waiting for a verification job to complete, the library automatically polls the underlying Verifalia API until results are ready. By default, it leverages the long polling mode introduced with Verifalia API v2.4, which minimizes the number of requests and delivers verification results faster.

### Avoid Waiting

In certain scenarios (such as microservice architectures), it may be preferable to avoid waiting for job completion and simply queue the job instead. In this case, the library would return just the job overview (not its verification results), and you would need to retrieve the verification results using the `GetAsync()` method.

To do this, specify `WaitOptions.NoWait` as the value for the `waitOptions` parameter of the `RunAsync()` method overloads:

```c#
var job = await verifalia
    .EmailVerifications
    .RunAsync(new FileInfo("that-file.xlsx"),
        waitOptions: WaitOptions.NoWait);

Console.WriteLine($"Status: {job.Overview.Status}");
// Status: InProgress
```

### Progress Tracking

For jobs with a large number of email addresses, it can be useful to track progress as addresses are processed by the Verifalia email verification engine. To do this, create an instance of the `WaitOptions` class and provide a handler that receives progress notifications through the `Progress` property.

Here's how to define a progress notification handler that displays the progress percentage of a submitted job to the console:

```c#
var job = await verifalia
    .EmailVerifications
    .RunAsync(new FileInfo("that-other-file.csv"),
        waitOptions: new WaitOptions
        {
            Progress = new Progress<VerificationOverview>(overview =>
            {
                Console.WriteLine(overview.Progress?.Percentage);
            })
        });
```

## Completion Callbacks

Along with each email verification job, you can specify a URL that Verifalia will invoke (POST) once the job completes. This URL must use the HTTPS or HTTP scheme and be publicly accessible over the Internet. To learn more about completion callbacks, please see https://verifalia.com/developers#email-validations-completion-callback.

To specify a completion callback URL, pass either a `VerificationRequest` or `FileVerificationRequest` to the `RunAsync()` method and set its `CompletionCallback` property accordingly:

```c#
await verifalia
    .EmailVerifications
    .RunAsync(new VerificationRequest(new[] { "batman@gmail.com" })
    {
        CompletionCallback = new CompletionCallback("https://your-website-here/foo/bar")
    });
```

Note that completion callbacks are invoked asynchronously, and it may take several seconds for your callback URL to be invoked.

## Retrieving Jobs

You can retrieve an email verification job using the `GetAsync()` and `GetOverviewAsync()` methods, which return a `Verification` instance or `VerificationOverview` instance, respectively, for the desired email verification job. While doing this, the library automatically waits for job completion. You can adjust this behavior by passing a `waitOptions` parameter to these methods in exactly the same fashion as described for the `RunAsync()` method overloads. Please see the [Wait Options](#wait-options) section for additional details.

Here's an example showing how to retrieve a job by its identifier:

```c#
var job = await verifalia
    .EmailVerifications
    .GetAsync("ec415ecd-0d0b-49c4-a5f0-f35c182e40ea");
```

## Exporting Email Verification Results in Different Output Formats

This library allows you to export the entries of a completed email verification job in different output formats through the `ExportEntriesAsync()` method. This generates a human-readable representation of the verification results.

> **WARNING**: While the output schema (columns/labels/data format) is fairly complete, you should always consider it subject to change. Use the `GetAsync()` / `GetEntriesAsync()` methods instead if you need to rely on a stable output schema.

Here's an example showing how to export a given email verification job as a comma-separated values (CSV) file:

```c#
// Export the validated entries for the job in CSV format
var exportedStream = await verifalia
    .EmailVerifications
    .ExportEntriesAsync("722c2fd8-8837-449f-ad24-0330c597c993",
        ExportedEntriesFormat.Csv);

// Create the output file stream
var fileStream = new FileStream("my-list.csv", FileMode.Create);

// Copy the exported stream into the output file stream
await exportedStream.CopyToAsync(fileStream);
```

## Don't Forget to Clean Up When You're Done

Verifalia automatically deletes completed jobs according to a configurable data-retention policy (see the related section), but we strongly recommend that you delete completed jobs as soon as possible for privacy and security reasons. To do this, invoke the `DeleteAsync()` method, passing the job ID you wish to remove:

```c#
await verifalia
    .EmailVerifications
    .DeleteAsync(job.Overview.Id);
```

Once deleted, a job is permanently removed and there is no way to retrieve its email verification results.

## Iterating Over Your Email Verification Jobs

For management and reporting purposes, you may want to obtain a detailed list of your past email verification jobs. This SDK library allows you to do this through the `ListAsync()` method, which allows asynchronous iteration over a collection of `VerificationOverview` instances (the same type as the `Overview` property of results returned by `RunAsync()` and `GetAsync()`).

Here's how to iterate over your jobs from most recent to oldest:

```c#
var jobs = verifalia
    .EmailVerifications
    .ListAsync(new VerificationOverviewListingOptions
    {
        Direction = Direction.Backward
    });

await foreach (var job in jobs)
{
    Console.WriteLine($"Id: {job.Id}, status: {job.Status}, entries: {job.NoOfEntries}");
}

// Prints out something like:
//
// Id: a7784f9a-86d4-436c-b8e4-f72f2bd377ac, status: InProgress, entries: 9886
// Id: 86d57c00-147a-4736-88cc-c918260c67c6, status: Completed, entries: 1
// Id: 594bbb0f-6f12-481c-926f-606cfefc1cd5, status: Completed, entries: 1
// Id: a5c1cd5b-39cc-43bc-9a3a-ee4a0f80ee6d, status: InProgress, entries: 226
// Id: b6f69e30-60dd-4c21-b2cb-e73ba75fb278, status: Completed, entries: 12077
// Id: 5e5a97dc-459f-4edf-a607-47371c32aa94, status: Deleted, entries: 1009
// ...
```

> The `ListAsync()` method uses the *C# 8.0 async enumerable* feature. For previous language version support, please check the `GetPageAsync()` methods group.

### Filtering Email Verification Jobs

The `ListAsync()` method also allows you to filter the email verification jobs returned by the Verifalia API through the same `options` argument. You can filter by submission date, owner, and job status.

Here's how to repeat the listing operation shown above, this time returning only jobs for a specific user and date range:

```c#
var jobs = verifalia
    .EmailVerifications
    .ListAsync(new VerificationOverviewListingOptions
    {
        Direction = Direction.Backward,
        CreatedOn = new DateBetweenPredicate(new DateTime(2025, 7, 1),
            new DateTime(2025, 7, 9)),
        Owner = new StringEqualityPredicate("50173acd-9ed2-4298-ba7f-8ccaeed48deb")
    });

await foreach (var job in jobs)
{
    // ...
}
```

# Managing Credits

To manage Verifalia credits for your account, use the `Credits` property exposed by the `VerifaliaClient` instance created above. Like the previous topic, in the following sections we'll examine the most common operations. We strongly recommend exploring the library and consulting the embedded XML documentation for other capabilities.

## Getting the Credits Balance

One of the most common tasks you may need to perform is retrieving the available number of free daily credits and credit packs. To do this, use the `GetBalanceAsync()` method, which returns a `Balance` object:

```c#
var balance = await verifalia
    .Credits
    .GetBalanceAsync();

Console.WriteLine($"Credit packs: {balance.CreditPacks}");
Console.WriteLine($"Free daily credits: {balance.FreeCredits} (will reset in {balance.FreeCreditsResetIn})");

// Prints out something like:
//
// Credit packs: 956.332
// Free daily credits: 128.66 (will reset in 09:08:23)
```

To add credit packs to your Verifalia account, visit [https://app.verifalia.com/#/credits/add](https://app.verifalia.com/#/credits/add).

## Retrieving Credits Usage Statistics

To monitor and forecast credit consumption for your account, the `ListDailyUsagesAsync()` method allows you to retrieve statistics about historical credit usage, returning an asynchronously iterable collection of `DailyUsage` instances. The method also allows you to limit the period of interest by passing a `DailyUsageListingOptions` instance. Elements are returned only for dates where consumption (of free credits, credit packs, or both) occurred.

Here's how to retrieve daily credit consumption for the last thirty days:

```c#
var dailyUsages = verifalia
    .Credits
    .ListDailyUsagesAsync(new DailyUsageListingOptions
	{
        DateFilter = new DateBetweenPredicate
        {
            Since = DateTime.Now.AddDays(-30)
        }
    });

await foreach (var usage in dailyUsages)
{
    Console.WriteLine($"{usage.Date:yyyyMMdd}");
    Console.WriteLine($" - credit packs: {usage.CreditPacks}");
    Console.WriteLine($" - free daily credits: {usage.FreeCredits}");
}

// Prints out something like:
//
// 20250623
// - credit packs: 1965.68
// - free daily credits: 200
// 20260621
// - credit packs: 0
// - free daily credits: 185.628
// 20250620
// - credit packs: 15.32
// - free daily credits: 200
// ...
```

> The `ListDailyUsagesAsync()` method uses the *C# 8.0 async enumerable* feature. For previous language version support, please check the `GetDailyUsagesPageAsync()` methods group.

# Users and Team Management

The Verifalia API provides the ability to manage users in your Verifalia account, as well as their security and configuration settings. Each user has unique login credentials and can submit, view, and manage their own private email-verification jobs while sharing the same account balance. This makes it easy to separate activities for team members, clients (if you're reselling our email verification service), or other API consumers (such as apps and websites using our email verification widget).

To manage users for your Verifalia account, use the `Users` property exposed by the VerifaliaClient instance created above.

Each user can be one of these types:

- **Administrator**: Has complete, unrestricted access to the Verifalia account
- **Standard user**: Has flexible, granular permissions, ideal for coworkers or API access
- **Browser app**: Designed for public-facing web applications and our embeddable widget; uses passwordless authentication and has fixed, limited permissions restricted to email verification only

Where applicable, the API also allows configuring specific settings for each user, including authentication methods and secondary authentication factors, permissions, firewall rules, CAPTCHA/bot detection settings, throttling rules, data retention settings, and trusted origins.

## Listing Users

This SDK allows you to list the users in your Verifalia account by invoking the `ListAsync()` method, which returns an `IAsyncEnumerable` collection of `UserOverview` objects containing the basic information for each user.

```csharp
await foreach (var user in verifalia.Users.ListAsync())
{
    Console.WriteLine($"User ID: {user.Id}");
    Console.WriteLine($"- display name: {user.DisplayName}");
    Console.WriteLine($"- type: {user.Type}");
}

// Prints out something like:
//
// User ID: ed49bbce-77d1-4af3-8a15-f25e20fff123
// - display name: Walter White
// - type: Administrator
// User ID: 8eaadc82-6bf5-46b2-8ac3-f62dddfeb47e
// - display name: Alvaro Vitali
// - type: Standard
```

> The `ListAsync()` method uses the *C# 8.0 async enumerable* feature. For previous language version support, please check the `GetPageAsync()` methods group.

## Retrieving User Details

To retrieve the complete configuration details for each user, invoke the `GetAsync()` method exposed by the `Users` property mentioned earlier, which returns an instance of the `User` type. This type exposes several properties that allow you to manage any configuration setting.

In the example below, we retrieve a user by their ID, then print some of their configuration settings to the console:

```csharp
var user = await verifalia.Users.GetAsync("8eaadc82-6bf5-46b2-8ac3-f62dddfeb47e");

// Active / Inactive status
Console.WriteLine($"IsActive: {user.IsActive}");

// Default settings
Console.WriteLine($"Default data retention period: {user.Defaults?.Retention}");

// Throttling rules
Console.WriteLine("Throttling rules:");

foreach (var throttlingRule in user.Throttling?.Rules ?? [])
{
    Console.WriteLine($"- {throttlingRule.Limit} per {throttlingRule.Period}, scope: {throttlingRule.Scope}");
}

// Prints out something like:
//
// IsActive: true
// Default data retention period: 05:30:00
// Throttling rules:
// - 3 per Minute, scope: IPAddress
// - 100 per Day, scope: Global
```

We strongly recommend exploring the `User` type and examining the exposed properties for other configuration settings.

## Creating Users

To create a new user, invoke the `CreateAsync()` method exposed by the `Users` property, passing a `User` instance with the configuration settings for the user you want to add.

In the example below, we add a new standard user named James McGill who has username-password authentication and can read the account balance and operate on his own email verifications:

```csharp
var user = await verifalia
    .Users
    .CreateAsync(new User
    {
        Type = UserType.Standard,
        DisplayName = "James McGill",
        Authentication = new AuthenticationSettings
        {
            UsernamePasswordAuthentication = new  UsernamePasswordAuthentication
            {
                IsEnabled = true,
                Username = "jimmy",
                Password = "5058425662"
            }
        },
        Authorization = new AuthorizationSettings
        {
            Rules =
            [
                "credits:read-balance",
                "email-verifications:*:own" 
            ]
        }
    });
```

## Updating Users

To update a user, invoke the `UpdateAsync()` method of the `Users` property and specify a LINQ expression tree with the changes to apply.

For instance, here's how to update the display name of the user we created in the previous section:

```csharp
await verifalia
    .Users
    .UpdateAsync(user.Id,
        _ => new User
        {
            DisplayName = "Saul Goodman"
        });
```

To avoid conflicts, you can specify the ETag of the user so that the Verifalia API can detect mid-air edit collisions and avoid updating the target user if it has a newer version. To do this, specify the ETag value as the `ifMatch` parameter:

```csharp
await verifalia
    .Users
    .UpdateAsync(user.Id,
        _ => new User
        {
            Defaults = new DefaultSettings
            {
                Retention = TimeSpan.FromMinutes(30),
            }
        },
        user.Etag);
```

## Deleting Users

Deleting a user is as simple as invoking the `DeleteAsync()` method of the `Users` property, specifying the ID of the user to remove:

```csharp
await verifalia
    .Users
    .DeleteAsync(user.Id);
```

# X.509 Client Certificates

Each standard user can have one or more X.509 client certificates associated with them. These certificates can be used for TLS mutual authentication (also known as Client Certificate Authentication).

Similar to the objects mentioned above, to manage client certificates for your Verifalia account, use the `ClientCertificates` property exposed by the VerifaliaClient instance created above.

## Listing Certificates

To list the X.509 client certificates associated with a specific user, invoke the `ListAsync()` method of the `ClientCertificates` property to retrieve an `IAsyncEnumerable` collection of `ClientCertificate` objects containing information about each certificate:

```csharp
await foreach (var certificate in verifalia.ClientCertificates.ListAsync(user.Id))
{
    Console.WriteLine($"{certificate.Subject} - {certificate.Thumbprint}");
    Console.WriteLine($"\tValidity: {certificate.NotBefore} - {certificate.NotAfter}");
}

// Prints out something like:
//
// O=Gray Matter, S=NM, C=US - 74c0fa6f23e34efc945cb96511487b0764e0e29d
//   Validity: 6/10/2025 5:33:56 PM - 6/10/2026 5:33:56 PM
```

> The `ListAsync()` method uses the *C# 8.0 async enumerable* feature. For previous language version support, please check the `GetPageAsync()` methods group.

## Creating Certificates

To upload and bind a new certificate to a user, invoke one of the methods in the `CreateAsync()` group exposed by the `ClientCertificates` property. The overloads accept a `FileInfo`, `Stream`, or `byte[]` with the content of the certificate file to upload, which must be either a base64-encoded format (commonly with .pem, .crt, or .cer extensions) or a binary (DER) format (commonly with .der or .cer extensions).

For enhanced security and compliance with RFC 5280, Verifalia only accepts X.509 client certificates that include the Extended Key Usage extension **id-kp-clientAuth** (OID 1.3.6.1.5.5.7.3.2).

The methods return an instance of the `ClientCertificate` type, which contains details about the created certificate:

```csharp
var certificate = await verifalia
    .ClientCertificates
    .CreateAsync(user.Id, new FileInfo("./certificate.pem"));

Console.WriteLine($"ID: {certificate.Id}");
Console.WriteLine($"\tPublic key: {certificate.PublicKey}");
Console.WriteLine($"\tValidity: {certificate.NotBefore} - {certificate.NotAfter}");

// Prints out something like:
//
// ID: 3997d2fe-69af-43f7-9667-91204a8e7f8c
//   Public key: 87185105bfad934cf445a0f77a41a73b20309e8da9cb417771d488444fed55ec
//   Validity: 1/1/2025 10:00:00 AM - 1/1/2028 10:00:00 AM
```

## Deleting Certificates

To delete a client certificate from Verifalia, invoke the `DeleteAsync()` method of the `ClientCertificates` property:

```csharp
await verifalia
    .ClientCertificates
    .DeleteAsync(user.Id, certificate.Id);
```

# Contact Methods

A contact method in Verifalia is a way for the system to send notifications to a user. Notifications cover everything from system alerts (such as expiring client certificates) to commercial document updates and news.

To manage contact methods for your Verifalia account, use the `ContactMethods` property exposed by the VerifaliaClient instance created above.

## Listing Contact Methods

To list the contact methods associated with a specific user, invoke the `ListAsync()` method of the `ContactMethods` property to retrieve an `IAsyncEnumerable` collection of `ContactMethod` objects containing information about each contact method:

```csharp
await foreach (var contactMethod in verifalia.ContactMethods.ListAsync(user.Id))
{
    Console.WriteLine($"{contactMethod.DisplayName} (ID: {contactMethod.Id})");

    if (contactMethod.Type == ContactMethodType.Email)
    {
        Console.WriteLine($"Email: {contactMethod.EmailAddress}");
    }
}

// Prints out something like:
//
// Walter White (ID: 04715cde-7910-4445-8d87-b85809267928)
// Email: walt@a1acarwash.com
```

> The `ListAsync()` method uses the *C# 8.0 async enumerable* feature. For previous language version support, please check the `GetPageAsync()` methods group.

## Retrieving Contact Methods

To retrieve a single contact method, invoke the `GetAsync()` method of the `ContactMethods` property, which accepts the ID of the user and the ID of the contact method, and returns an instance of the `ContactMethod` type:

```csharp
var contactMethod = await verifalia.ContactMethods.GetAsync(userId, contactMethodId);
```

## Creating Contact Methods

To create a contact method for a user, invoke the `CreateAsync()` method of the `ContactMethods` property and specify the configuration details for the new object. The method returns a `ContactMethod` object with details about the newly created item:

```csharp
var contactMethod = await verifalia
    .ContactMethods
    .CreateAsync(user.Id,
        new ContactMethod
        {
            DisplayName = "Luigi Schroeder",
            EmailAddress = "reder28@gmail.com",
            Type = ContactMethodType.Email,
        });
```

Once a contact method is created, it is inactive and needs to be activated before being available for use. The activation process involves a manual step where the recipient receives an activation code that must be provided to the Verifalia API.

### Activating Contact Methods

To activate a contact method, invoke the `ActivateAsync()` method of the `ContactMethods` property, specifying the target user ID, the ID of the contact method you wish to activate, and the activation code automatically received when the contact method was created:

```csharp
await verifalia
    .ContactMethods
    .ActivateAsync("450478c3-a235-47d9-bfbb-0411d71c93eb",
        "3c4c6f8b-fdd9-4d2a-900c-1373b6e01bda",
        "abcde12345");
```

## Updating Contact Methods

To update a contact method, invoke the `UpdateAsync()` method of the `ContactMethods` property and specify a LINQ expression tree with the changes to apply.

For instance, here's how to update the display name of the contact method we created in the previous section:

```csharp
await verifalia
    .ContactMethods
    .UpdateAsync(user.Id,
        contactMethod.Id,
        _ => new ContactMethod
        {
            DisplayName = "Gigi Reder"
        });
```

To avoid conflicts, you can specify the ETag of the contact method so that the Verifalia API can detect mid-air edit collisions and avoid updating the target contact method if it has a newer version. To do this, specify the ETag value as the `ifMatch` parameter:

```csharp
await verifalia
    .ContactMethods
    .UpdateAsync(user.Id,
        contactMethod.Id,
        _ => new ContactMethod
        {
            DisplayName = "Gigi Reder"
        },
        contactMethod.Etag);
```

## Deleting Contact Methods

To delete a contact method, invoke the `DeleteAsync()` method of the `ContactMethods` property, specifying the ID of the user and the ID of the contact method to remove:

```csharp
await verifalia
    .ContactMethods
    .DeleteAsync(user.Id, contactMethod.Id);
```

# Changelog / What's New

This section lists the changelog for the current major version of the library. For older versions, please see the [project releases](https://github.com/verifalia/verifalia-csharp-sdk/releases). For clarity, logs for build and revision updates are excluded.

## v5.1

Released on April 2026

- Added support for .NET 10.0
- Fixed an issue where `ExportEntriesAsync()` didn’t respect the specified options
- Improved documentation across the library

## v5.0

Released on July 2025

- Added support for API v2.7 (users management and related features)
- Internalized dependencies (Newtonsoft.Json, Flurl) for better version compatibility
- Added support for .NET 9.0
- Added support for `problem+json` error format
- **Breaking change**: Renamed `VerifaliaRestClient` to `VerifaliaClient`
- **Breaking change**: Renamed `SubmitAsync()` to `RunAsync()` in `VerifaliaClient.EmailVerifications`
- **Breaking change**: Renamed namespace `EmailValidation` to `EmailVerification` and related types
- **Breaking change**: Renamed `*Validation*` classes to `*Verification*` throughout the library
- **Breaking change**: Renamed `ListSegment` to `PagedResult` and related types
- Fixed cancellation token handling during email verification retrieval when issues occur
- Improved documentation across the library
- Updated code samples
