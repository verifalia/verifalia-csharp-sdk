![Verifalia API](https://img.shields.io/badge/Verifalia%20API-v2.5-green)
[![NuGet](https://img.shields.io/nuget/v/Verifalia.svg)](https://www.nuget.org/packages/Verifalia)
![License](https://img.shields.io/badge/License-MIT-yellow.svg)

Verifalia RESTful API - .NET SDK and helper library
===================================================

[Verifalia][0] provides a simple HTTPS-based API for validating email addresses in real-time and checking whether they are deliverable or not; this SDK library integrates with Verifalia and allows to [verify email addresses][0] under the following platforms:

- .NET 5.0 and higher, including **.NET 8.0** ![new](https://img.shields.io/badge/new-green)
- .NET Core 1.0 (and higher)
- .NET Framework 4.5 (and higher)
- .NET Standard 1.3 (and higher)
  - Mono 4.6+
  - Xamarin.iOS 10.0+
  - Xamarin.Mac 3.0+
  - Xamarin.Android 7.0+
  - Universal Windows Platform 10.0+

To learn more about Verifalia please see [https://verifalia.com][0]

## Table of contents

- [Adding Verifalia REST API support to your .NET solution](#adding-verifalia-rest-api-support-to-your-net-solution)
  + [With Visual Studio IDE](#with-visual-studio-ide)
  + [Manual download and compilation](#manual-download-and-compilation)
  * [Authentication](#authentication)
    + [Authenticating via bearer token](#authenticating-via-bearer-token)
    + [Authenticating via X.509 client certificate (TLS mutual authentication)](#authenticating-via-x509-client-certificate--tls-mutual-authentication-)
- [Validating email addresses](#validating-email-addresses)
  * [How to validate an email address](#how-to-validate-an-email-address)
  * [How to validate a list of email addresses](#how-to-validate-a-list-of-email-addresses)
  * [How to import and submit a file for validation](#how-to-import-and-submit-a-file-for-validation)
  * [Processing options](#processing-options)
    + [Quality level](#quality-level)
    + [Deduplication mode](#deduplication-mode)
    + [Data retention](#data-retention)
  * [Wait options](#wait-options)
    + [Avoid waiting](#avoid-waiting)
    + [Progress tracking](#progress-tracking)
  * [Completion callbacks](#completion-callbacks)
  * [Retrieving jobs](#retrieving-jobs)
  * [Exporting email verification results in different output formats](#exporting-email-verification-results-in-different-output-formats)
  * [Don't forget to clean up, when you are done](#don-t-forget-to-clean-up--when-you-are-done)
  * [Iterating over your email validation jobs](#iterating-over-your-email-validation-jobs)
    * [Filtering email validation jobs](#filtering-email-validation-jobs)
- [Managing credits](#managing-credits)
  * [Getting the credits balance](#getting-the-credits-balance)
  * [Retrieving credits usage statistics](#retrieving-credits-usage-statistics)
- [Changelog / What's new](#changelog---what-s-new)

---

## Adding Verifalia REST API support to your .NET solution ##

The best and easiest way to add the Verifalia email verification SDK library to your .NET project is to use the NuGet package manager.

#### With Visual Studio IDE

From within Visual Studio, you can use the NuGet GUI to search for and install the Verifalia NuGet package. Or, as a shortcut, simply type the following command into the Package Manager Console:

    Install-Package Verifalia

#### Manual download and compilation
	
As an alternative way to add the Verifalia SDK to your .NET solution, you can [download the SDK source project from github][1], extract it to a folder of your choice and add a reference from your own project to the Verifalia SDK project. The SDK project is a C# project which can be referenced and used with any other .NET language too, including Visual Basic (VB.NET), C++/CLI, J#, IronPython, IronRuby, F# and PowerShell.

Learn more at [https://verifalia.com][0]

### Authentication ###

First things first: authentication to the Verifalia API is performed by way of either the credentials of your root Verifalia account or of one of its users (previously known as sub-accounts): if you don't have a Verifalia account, just [register for a free one][4]. For security reasons, it is always advisable to [create and use a dedicated user][3] for accessing the API, as doing so will allow to assign only the specific needed permissions to it.

Learn more about authenticating to the Verifalia API at [https://verifalia.com/developers#authentication][2]

Once you have your Verifalia credentials at hand, use them while creating a new instance of the `VerifaliaRestClient` type, which will be the starting point to every other operation against the Verifalia API: the supplied credentials will be automatically provided to the API using the HTTP Basic Auth method.

```c#
using Verifalia.Api;

var verifalia = new VerifaliaRestClient("username", "password");
```

In addition to the HTTP Basic Auth method, this SDK also supports other different ways to authenticate to the Verifalia API, as explained in the subsequent sections.

#### Authenticating via bearer token

Bearer authentication offers higher security over HTTP Basic Auth, as the latter requires sending the actual credentials on each API call, while the former only requires it on a first, dedicated authentication request. On the other side, the first authentication request needed by Bearer authentication takes a non-negligible time: if you need to perform only a single request, using HTTP Basic Auth provides the same degree of security and is also faster.

```c#
using Verifalia.Api;
using Verifalia.Api.Security;

var verifalia = new VerifaliaRestClient(new BearerAuthenticationProvider("username", "password"));
```

Handling multi-factor auth (MFA) is also possible by defining a custom implementation of the `ITotpTokenProvider` interface, which should be used to acquire the time-based one-time password from an external authenticator app or device: to add multi-factor auth to your root Verifalia account, [configure your security settings](https://verifalia.com/client-area#/account/security-settings).

```c#
using Verifalia.Api;
using Verifalia.Api.Security;

class MyTotpProvider : ITotpTokenProvider
{
	public Task<string> ProvideTotpTokenAsync(CancellationToken cancellationToken)
	{
		// Ask the user to type his or her TOTP token

		Console.WriteLine("Acquire your TOTP token and type it here:");
		var totpToken = Console.ReadLine();

		return Task.FromResult(totpToken);
	}
}

// ...

var verifalia = new VerifaliaRestClient(new BearerAuthenticationProvider("username", "password", new MyTotpProvider()));
```

#### Authenticating via X.509 client certificate (TLS mutual authentication)

This authentication method uses a cryptographic X.509 client certificate to authenticate against the Verifalia API, through the TLS protocol. This method, also called mutual TLS authentication (mTLS) or two-way authentication, offers the highest degree of security, as only a cryptographically-derived key (and not the actual credentials) is sent over the wire on each request.

```c#
using Verifalia.Api;
using Verifalia.Api.Security;

var verifalia = new VerifaliaRestClient(new X509Certificate2("mycertificate.pem"));
```

## Validating email addresses ##

Every operation related to verifying / validating email addresses is performed through the `EmailValidations` property exposed by the `VerifaliaRestClient` instance you created above. The property is filled with useful methods, each one having lots of overloads: in the next few paragraphs we are looking at the most used ones, so it is strongly advisable to explore the library and look at the embedded xmldoc help for other opportunities.

**The library automatically waits for the completion of email verification jobs**: if needed, it is possible
to adjust the wait options and have more control over the entire underlying polling process. Please refer to
the [Wait options](#wait-options) section below for additional details. 

### How to validate an email address ###

To validate an email address from a .NET application you can invoke the `SubmitAsync()` method: it accepts one or more email addresses and any eventual verification options you wish to pass to Verifalia, including the expected results quality, deduplication preferences, processing priority.

> **Note**
> In the event you need to verify a list of email addresses, it is advisable to submit them all at once through one
> of the dedicated `SubmitAsync()` method overloads (see the next sections), instead of iterating over the
> source set and submitting the addresses one by one. Not only the all-at-once method would be faster, it would
> also allow to detect and mark duplicated items - a feature which is unavailable while verifying the email addresses
> one by one.

In the following example, we verify an email address with this library, using the default options:

```c#
var job = await verifalia
    .EmailValidations
    .SubmitAsync("batman@gmail.com");

// At this point the address has been validated: let's print its email validation
// result to the console.

var entry = job.Entries[0];

Console.WriteLine($"Classification: {entry.Classification} (status: {entry.Status})");

// Classification: Deliverable (status: Success)
```

As you may expect, each entry may include various additional details about the verified email address:

| Property                      | Description                                                                                                                                                                                                                                                |
|-------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `AsciiEmailAddressDomainPart` | Gets the domain part of the email address, converted to ASCII if needed and with comments and folding white spaces stripped off.                                                                                                                           |
| `Classification`              | The `ValidationEntryClassification` value for this entry.                                                                                                                                                                                                  |
| `CompletedOn`                 | The date this entry has been completed, if available.                                                                                                                                                                                                      |
| `Custom`                      | A custom, optional string which is passed back upon completing the validation. To pass back and forth a custom value, use the `Custom` property of `ValidationRequestEntry`.                                                                               |
| `DuplicateOf`                 | The zero-based index of the first occurrence of this email address in the parent `Validation`, in the event the `Status` for this entry is `Duplicate`; duplicated items do not expose any result detail apart from this and the eventual `Custom` values. |
| `Index`                       | The index of this entry within its `Validation` container; this property is mostly useful in the event the API returns a filtered view of the items.                                                                                                       |
| `InputData`                   | The input string being validated.                                                                                                                                                                                                                          |
| `EmailAddress`                | Gets the email address, without any eventual comment or folding white space. Returns null if the input data is not a syntactically invalid e-mail address.                                                                                                 |
| `EmailAddressDomainPart`      | Gets the domain part of the email address, without comments and folding white spaces.                                                                                                                                                                      |
| `EmailAddressLocalPart`       | Gets the local part of the email address, without comments and folding white spaces.                                                                                                                                                                       |
| `HasInternationalDomainName`  | If true, the email address has an international domain name.                                                                                                                                                                                               |
| `HasInternationalMailboxName` | If true, the email address has an international mailbox name.                                                                                                                                                                                              |
| `IsDisposableEmailAddress`    | If true, the email address comes from a disposable email address (DEA) provider. <a href="https://verifalia.com/help/email-validations/what-is-a-disposable-email-address-dea">What is a disposable email address?</a>                                     |
| `IsFreeEmailAddress`          | If true, the email address comes from a free email address provider (e.g. gmail, yahoo, outlook / hotmail, ...).                                                                                                                                           |
| `IsRoleAccount`               | If true, the local part of the email address is a well-known role account.                                                                                                                                                                                 |
| `Status`                      | The `ValidationEntryStatus` value for this entry.                                                                                                                                                                                                          |
| `Suggestions`                 | The potential corrections for the input data, in the event Verifalia identified potential typos during the verification process.                                                                                                                           |
| `SyntaxFailureIndex`          | The position of the character in the email address that eventually caused the syntax validation to fail.                                                                                                                                                   |

Here is another example, showing some of the additional result details provided by Verifalia:

```c#
var job = await verifalia
    .EmailValidations
    .SubmitAsync("bat[man@gmal.com");

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

### How to validate a list of email addresses ###

To verify a list of email addresses - instead of a single address - it is possible to use the `SubmitAsync()` method
overload which accepts an `IEnumerable<string>`; if the email addresses to be verified are originally stored
in a file, it is also possible to simply upload the file and have Verifalia automatically import and verify
it - see the next section for the details.

Here is an example showing how to verify an array with some email addresses:

```c#
var job = await verifalia
    .EmailValidations
    .SubmitAsync(new[] {
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


### How to import and submit a file for validation

This library includes support for submitting and validating files with email addresses, including:

- **plain text files** (.txt), with one email address per line;
- **comma-separated values** (.csv), **tab-separated values** (.tsv) and other delimiter-separated values files;
- **Microsoft Excel spreadsheets** (.xls and .xlsx).

To submit and validate files, one can still use the `SubmitAsync()` method mentioned
above, passing either a `Stream` or a `FileInfo` instance or just a `byte[]` with the
file content. Along with that, it is also possible to specify the eventual starting
and ending rows to process, the column, the sheet index, the line ending and the
delimiter - depending of course on the nature of the submitted file (see
`FileValidationRequest` in the source to learn more).

Here is how to submit and verify an Excel file, for example:

```c#
var job = await verifalia
    .EmailValidations
    .SubmitAsync(new FileInfo("that-file.xslx"));
```

For more advanced options, just pass `FileValidationRequest` instance to the `SubmitAsync()`
method:

```c#
var job = await verifalia
    .EmailValidations
    .SubmitAsync(new FileValidationRequest(new FileInfo("that-file.xslx"))
        {
            Sheet = 3,
            StartingRow = 1,
            Column = 5
        },
        quality: QualityLevelName.High);
```

And here is another example, showing how to submit a `Stream` instance and specifying the
MIME content type of the file, which is automatically determined from the file extension in
the event you pass a `FileInfo` instance:

```c#
Stream inputStream = ...; // TODO: Acquire the input data somehow

var job = await verifalia
    .EmailValidations
    .SubmitAsync(inputStream,
        MediaTypeHeaderValue.Parse(WellKnownMimeContentTypes.TextPlain)); // text/plain
```

### Processing options

While submitting one or more email addresses for verification, it is possible to specify several
options which affect the behavior of the Verifalia processing engine as well as the verification flow
from the API consumer standpoint.

#### Quality level

Verifalia offers three distinct quality levels - namely, _Standard_, _High_ and _Extreme_  - which rule out how the email verification engine should
deal with temporary undeliverability issues, with slower mail exchangers and other potentially transient
problems which can affect the quality of the verification results. The `SubmitAsync()` method overloads accept a `quality` parameter which allows
to specify the desired quality level; here is an example showing how to verify an email address using
the _High_ quality level:

```c#
var job = await verifalia
    .EmailValidations
    .SubmitAsync("batman@gmail.com", quality: QualityLevelName.High);
```

#### Deduplication mode

The `SubmitAsync()` method overloads accepting multiple email addresses at once allow to specify how to
deal with duplicated entries pertaining to the same input set; Verifalia supports a _Safe_ deduplication
mode, which strongly adheres to the old IETF standards, and a _Relaxed_ mode which is more in line with
what can be found in the majority of today's mail exchangers configurations.

In the next example, we show how to import and verify a list of email addresses and mark duplicated
entries using the _Relaxed_ deduplication mode:

```c#
var job = await verifalia
    .EmailValidations
    .SubmitAsync(new FileInfo("that-file.xslx"), deduplication: DeduplicationMode.Relaxed);
```

#### Data retention

Verifalia automatically deletes completed email verification jobs according to the data retention
policy defined at the account level, which can be eventually overriden at the user level: one can
use the [Verifalia clients area](https://verifalia.com/client-area) to configure these settings.

It is also possible to specify a per-job data retention policy which govern the time to live of a submitted
email verification job; to do that, use the `SubmitAsync()` method overloads which either accepts
a `ValidationRequest` or a `FileValidationRequest` instance and initialize its `Retention` property
accordingly.

Here is how, for instance, one can set a data retention policy of 10 minutes while verifying
an email address:

```c#
var job = await verifalia
    .EmailValidations
    .SubmitAsync(new ValidationRequest(new[]
    {
        "batman@gmail.com"
    })
    {
        Retention = TimeSpan.FromMinutes(10)
    });
```

### Wait options

By default, the `SubmitAsync()` method overloads submit an email verification job to Verifalia and wait
for its completion; the entire process may require some time to complete depending on the plan of the
Verifalia account, the number of email addresses the submission contains, the specified quality level
and other network factors including the latency of the mail exchangers under test. 

In waiting for the completion of a given email verification job, the library automatically polls the
underlying Verifalia API until the results are ready; by default, it tries to take advantage of the long
polling mode introduced with the Verifalia API v2.4, which allows to minimize the number of requests
and get the verification results faster.

#### Avoid waiting

In certain scenarios (in a microservice architecture, for example), however, it may preferable to avoid
waiting for a job completion and ask the Verifalia API, instead, to just queue it: in that case, the library
would just return the job overview (and not its verification results) and it will be necessary to retrieve
the verification results using the `GetAsync()` method.

To do that, it is possible to specify the `WaitOptions.NoWait` as the value for the `waitOptions` parameter
of the `SubmitAsync()` method overloads, as shown in the next example:

```c#
var job = await verifalia
    .EmailValidations
    .SubmitAsync(new FileInfo("that-file.xslx"),
        waitOptions: WaitOptions.NoWait);

Console.WriteLine($"Status: {job.Overview.Status}");
// Status: InProgress
```

#### Progress tracking

For jobs with a large number of email addresses, it could be useful to track progress as they are processed
by the Verifalia email verification engine; to do that, it is possible to create an instance of the
`WaitOptions` class and provide an handler which eventually receives progress notifications through the
`Progress` property.

Here is how to define a progress notification handler which displays the progress percentage of a submitted
job to the console window:

```c#
var job = await verifalia
    .EmailValidations
    .SubmitAsync(new FileInfo("that-other-file.csv"),
        waitOptions: new WaitOptions
        {
            Progress = new Progress<ValidationOverview>(overview =>
            {
                Console.WriteLine(overview.Progress?.Percentage);
            })
        });
```

### Completion callbacks

Along with each email validation job, it is possible to specify an URL which
Verifalia will invoke (POST) once the job completes: this URL must use the HTTPS or HTTP
scheme and be publicly accessible over the Internet.
To learn more about completion callbacks, please see https://verifalia.com/developers#email-validations-completion-callback

To specify a completion callback URL, pass either a `ValidationRequest` or a `FileValidationRequest`
to the `SubmitAsync()` method and set its `CompletionCallback` property accordingly, as shown
in the example below:

```c#
await verifalia
    .EmailValidations
    .SubmitAsync(new ValidationRequest(new[] { "batman@gmail.com" })
    {
        CompletionCallback = new CompletionCallback("https://your-website-here/foo/bar")
    });
```

Note that completion callbacks are invoked asynchronously and it could take up to
several seconds for your callback URL to get invoked.

### Retrieving jobs

It is possible to retrieve a job through the `GetAsync()` and `GetOverviewAsync()` methods, which
return, respectively, a `Validation` instance or a `ValidationOverview` instance for the desired
email verification job. While doing that, the library automatically waits for the completion of
the job, and it is possible to adjust this behavior by passing to the aforementioned methods
a `waitOptions` parameter, in the exactly same fashion as described for the `SubmitAsync()` method
overloads; please see the [Wait options](#wait-options) section for additional details.

Here is an example showing how to retrieve a job, given its identifier:

```c#
var jobId = Guid.Parse("ec415ecd-0d0b-49c4-a5f0-f35c182e40ea");
var job = await verifalia.EmailValidations.GetAsync(jobId);
```

### Exporting email verification results in different output formats

This library also allows to export the entries of a completed email validation
job in different output formats through the `ExportEntriesAsync()` method, with the goal of generating a human-readable representation
of the verification results.

> **WARNING**: While the output schema (columns / labels / data format) is fairly
> complete, you should always consider it as subject to change: use the `GetAsync()` / `GetEntriesAsync()`
> methods instead if you need to rely on a stable output schema.

Here is an example showing how to export a given email verification job as a comma-separated values (CSV) file:

```c#
// Exports the validated entries for the job in the CSV format

var exportedStream = await verifalia
    .EmailValidations
    .ExportEntriesAsync(new Guid("722c2fd8-8837-449f-ad24-0330c597c993"),
        ExportedEntriesFormat.Csv);

// Creates the output file stream

var fileStream = new FileStream("my-list.csv", FileMode.Create);

// Copies the exported stream into the output file stream

await exportedStream.CopyToAsync(fileStream);
```

### Don't forget to clean up, when you are done

Verifalia automatically deletes completed jobs after a configurable
data-retention policy (see the related section) but it is strongly advisable that
you delete your completed jobs as soon as possible, for privacy and security reasons. To do that, you can invoke the `DeleteAsync()` method passing the job Id you wish to get rid of:

```c#
await verifalia
    .EmailValidations
    .DeleteAsync(job.Id);
```

Once deleted, a job is gone and there is no way to retrieve its email validation results.

### Iterating over your email validation jobs

For management and reporting purposes, you may want to obtain a detailed list of your past email validation jobs. This SDK library allows to do that through the `ListAsync()` method, which allows to iterate asynchronously over a collection of `ValidationOverview` instances (the same type of the `Overview` property of the results returned by `SubmitAsync()` and `GetAsync()`).

Here is how to iterate over your jobs, from the most recent to the oldest one:

```c#
var jobOverviews = verifalia
    .EmailValidations
    .ListAsync(new ValidationOverviewListingOptions
    {
        Direction = Direction.Backward
    });

await foreach (var jobOverview in jobOverviews)
{
    Console.WriteLine("Id: {0}, status: {2}, entries: {3}",
        jobOverview.Id,
        jobOverview.Status,
        jobOverview.NoOfEntries);
}

// Prints out something like:
// Id: a7784f9a-86d4-436c-b8e4-f72f2bd377ac, status: InProgress, entries: 9886
// Id: 86d57c00-147a-4736-88cc-c918260c67c6, status: Completed, entries: 1
// Id: 594bbb0f-6f12-481c-926f-606cfefc1cd5, status: Completed, entries: 1
// Id: a5c1cd5b-39cc-43bc-9a3a-ee4a0f80ee6d, status: InProgress, entries: 226
// Id: b6f69e30-60dd-4c21-b2cb-e73ba75fb278, status: Completed, entries: 12077
// Id: 5e5a97dc-459f-4edf-a607-47371c32aa94, status: Deleted, entries: 1009
// ...
```

> The `ListAsync()` method uses the *C# 8.0 async enumerable* feature; for previous language support please check the `ListSegmentedAsync()` methods group.

#### Filtering email validation jobs

The `ListAsync()` method also have the ability, by way of the same `options` argument, to filter the email
verification jobs returned by the Verifalia API: it is possible to filter by date of submission,
owner and status of the jobs.

Here is how to repeat the listing operation shown in the example above, this time returning only the jobs
of a given user and for a given date range:

```c#
var jobOverviews = verifalia
    .EmailValidations
    .ListAsync(new ValidationOverviewListingOptions
    {
        Direction = Direction.Backward,
        CreatedOn = new DateBetweenPredicate(new DateTime(2024, 1, 3),
            new DateTime(2024, 1, 7)),
        Owner = new StringEqualityPredicate("50173acd-9ed2-4298-ba7f-8ccaeed48deb")
    });

await foreach (var jobOverview in jobOverviews)
{
    // ...
}
```

## Managing credits ##

To manage the Verifalia credits for your account you can use the `Credits` property exposed by the `VerifaliaRestClient` instance created above. Like for the previous topic, in the next few paragraphs we are looking at the most used operations, so it is strongly advisable to explore the library and look at the embedded xmldoc help for other opportunities.

### Getting the credits balance ###

One of the most common tasks you may need to perform on your account is retrieving the available number of free daily credits and credit packs. To do that, you can use the `GetBalanceAsync()` method, which returns a `Balance` object, as shown in the next example:

```c#
var balance = await verifalia
    .Credits
    .GetBalanceAsync();

Console.WriteLine("Credit packs: {0}, free daily credits: {1} (will reset in {2})",
	balance.CreditPacks,
	balance.FreeCredits,
	balance.FreeCreditsResetIn);

// Prints out something like:
// Credit packs: 956.332, free daily credits: 128.66 (will reset in 09:08:23)
```

To add credit packs to your Verifalia account visit [https://verifalia.com/client-area#/credits/add][5].

### Retrieving credits usage statistics ###

As a way to monitor and forecast the credits consumption for your account, the method `ListDailyUsagesAsync()` allows to retrieve statistics about historical credits usage, returning an asynchronously iterable collection of `DailyUsage` instances. The method also allows to limit the period of interest by passing a `DailyUsageListingOptions` instance. Elements are returned only for the dates where consumption (either of free credits, credit packs or both) occurred.

Here is how to retrieve the daily credits consumption for the last thirty days:

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

await foreach (var dailyUsage in dailyUsages)
{
    Console.WriteLine("{0:yyyyMMdd} - credit packs: {1}, free daily credits: {2}",
        dailyUsage.Date,
        dailyUsage.CreditPacks,
        dailyUsage.FreeCredits);
}

// Prints out something like:
// 20240201 - credit packs: 1965.68, free daily credits: 200
// 20240126 - credit packs: 0, free daily credits: 185.628
// 20240125 - credit packs: 15.32, free daily credits: 200
// ...
```

> The `ListDailyUsagesAsync()` method uses the *C# 8.0 async enumerable* feature; for previous language support please check the `ListDailyUsagesSegmentedAsync()` methods group.

## Changelog / What's new

This section lists the changelog for the current major version of the library: for older versions,
please see the [project releases](https://github.com/verifalia/verifalia-csharp-sdk/releases).

### v4.2

Released on January 11<sup>th</sup>, 2024

- Added support for API v2.5
- Added support for classification override rules
- Added support for AI-powered suggestions
- Added support for .NET 8.0
- Bumped dependencies

### v4.1

Released on May 26<sup>th</sup>, 2023

- Added support for filters when listing email verification jobs
- Resolved an issue with the `ToAsyncEnumerableAsync()` method that previously resulted in incomplete listings in specific scenarios

### v4.0

Released on February 27<sup>th</sup>, 2023

- Added support for API v2.4
- Added support for .NET 7.0
- Added support for new completion callback options
- Added support for parked mail exchangers detection
- Added support for specifying a custom wait time while submitting and retrieving email verification jobs
- Added support for nullable annotations
- Breaking change: renamed `WaitingStrategy` into `WaitOptions` and refactored the latter so that it now allows to
adjust the underlying polling wait times
- Breaking change: the default job submission and retrieval behavior is now to wait for the completion
of jobs (but it is possible to change that through the new `WaitOptions` class)
- Breaking change: the `CompletionCallback` property of the `ValidationRequest` and `FileValidationRequest` classes
now points to a full fledged `CompletionCallback` class instead of a simple `Uri`
- Bumped dependencies (including Newtonsoft.Json and Flurl) 
- Improved documentation

[0]: https://verifalia.com
[1]: https://github.com/verifalia/verifalia-csharp-sdk/archive/master.zip
[2]: https://verifalia.com/developers#authentication
[3]: https://verifalia.com/client-area#/users/new
[4]: https://verifalia.com/sign-up
[5]: https://verifalia.com/client-area#/credits/add
