![Verifalia API](https://img.shields.io/badge/Verifalia%20API-v2.2-green)
[![NuGet](https://img.shields.io/nuget/v/Verifalia.svg)](https://www.nuget.org/packages/Verifalia)

Verifalia RESTful API - .NET SDK and helper library
===================================================

[Verifalia][0] provides a simple HTTPS-based API for validating email addresses in real-time and checking whether they are deliverable or not; this SDK library integrates with Verifalia and allows to [verify email addresses][0] under the following platforms:

- .NET 5.0 and higher, including .NET 6.0 ![new](https://img.shields.io/badge/new-green)
- .NET Core 1.0 (and higher)
- .NET Framework 4.5 (and higher)
- .NET Standard 1.3 (and higher)
  - Mono 4.6+
  - Xamarin.iOS 10.0+
  - Xamarin.Mac 3.0+
  - Xamarin.Android 7.0+
  - Universal Windows Platform 10.0+

To learn more about Verifalia please see [https://verifalia.com][0]

## Adding Verifalia REST API support to your .NET solution ##

The best and easiest way to add the Verifalia email verification SDK library to your .NET project is to use the NuGet package manager.

#### With Visual Studio IDE

From within Visual Studio, you can use the NuGet GUI to search for and install the Verifalia NuGet package. Or, as a shortcut, simply type the following command into the Package Manager Console:

    Install-Package Verifalia

#### Manual download and compilation
	
As an alternative way to add the Verifalia SDK to your .NET solution, you can [download the SDK source project from github][1], extract it to a folder of your choice and add a reference from your own project to the Verifalia SDK project. The SDK project is a C# project with support for Visual Studio 2019, which can be referenced and used with any other .NET language too, including Visual Basic (VB.NET), C++/CLI, J#, IronPython, IronRuby, F# and PowerShell.

Learn more at [https://verifalia.com][0].

### Authentication ###

First things first: authentication to the Verifalia API is performed by way of either the credentials of your root Verifalia account or of one of its users (previously known as sub-accounts): if you don't have a Verifalia account, just [register for a free one][4]. For security reasons, it is always advisable to [create and use a dedicated user][3] for accessing the API, as doing so will allow to assign only the specific needed permissions to it.

Learn more about authenticating to the Verifalia API at [https://verifalia.com/developers#authentication][2]

Once you have your Verifalia credentials at hand, use them while creating a new instance of the `VerifaliaRestClient` type, which will be the starting point to every other operation against the Verifalia API: the supplied credentials will be automatically provided to the API using the HTTP Basic Auth method.

```c#
using Verifalia.Api;

var verifalia = new VerifaliaRestClient("username", "password");
```

In addition to the HTTP Basic Auth method, this SDK also supports other different ways to authenticate to the Verifalia API, as explained in the subsequent paragraphs.

#### Authenticating via bearer token

Bearer authentication offers higher security over HTTP Basic Auth, as the latter requires sending the actual credentials on each API call, while the former only requires it on a first, dedicated authentication request. On the other side, the first authentication request needed by Bearer authentication takes a non-negligible time: if you need to perform only a single request, using HTTP Basic Auth provides the same degree of security and is the faster option too.

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

### How to validate an email address ###

To validate an email address from a .NET application you can invoke the `SubmitAsync()` method: it accepts one or more email addresses and any eventual verification options you wish to pass to Verifalia, including the expected results quality, deduplication preferences and processing priority.

In the next example, we are showing how to verify a single email address using this library; as the entire process is asynchronous, we are passing a `WaitingStrategy` value, asking `SubmitAsync()` to automatically wait for the job completion:

```c#
var validation = await verifalia
    .EmailValidations
    .SubmitAsync("batman@gmail.com", waitingStrategy: new WaitingStrategy(true));

// At this point the address has been validated: let's print
// its email validation result to the console.

var entry = validation.Entries[0];

Console.WriteLine("{0} => Classification: {1}, Status: {1}",
	entry.InputData,
	entry.Classification,
	entry.Status);

// Prints out something like:
// batman@gmail.com => Classification: Deliverable, Status: Success
```

### How to validate a list of email addresses ###

As an alternative to method above, you can avoid automatically waiting and retrieve the email validation results at a later time; this is preferred in the event you are verifying a list of email addresses, which could take minutes or even hours to complete.

Here is how to do that:

```c#
var validation = await verifalia
    .EmailValidations
    .SubmitAsync(new[] {
      "batman@gmail.com",
      "steve.vai@best.music",
      "samantha42@yahoo.de"
    });

Console.WriteLine("Job Id: {validation.Overview.Id}");
Console.WriteLine("Status: {validation.Overview.Status}");

// Prints out something like:
// Job Id: 290b5146-eeac-4a2b-a9c1-61c7e715f2e9
// Status: InProgress
```

Once you have an email validation job Id, which is always returned by `SubmitAsync()` as part of the validation's `Overview` property, you can retrieve the job data using the `GetAsync()` method. Similarly to the submission process, you can either wait for the completion of the job or just retrieve the current job snapshot to get its progress, using an instance of the same `WaitingStrategy` type mentioned above. Only completed jobs have their `Entries` filled with the email validation results, however.

In the following example, we are requesting the current snapshot of a given email validation job back from Verifalia:

```c#
var validation = await verifalia
    .EmailValidations
    .GetAsync(Guid.Parse("290b5146-eeac-4a2b-a9c1-61c7e715f2e9"));

if (validation.Overview.Status == ValidationStatus.Completed)
{
	// validation.Entries will have the validation results!
}
else
{
	// What about having a coffee?
}
```

And here is how to request the same job, asking the SDK to automatically wait for us until the job is completed (that is, _joining_ the job):

```c#
var validation = await verifalia
    .EmailValidations
    .GetAsync(Guid.Parse("290b5146-eeac-4a2b-a9c1-61c7e715f2e9"),
        new WaitingStrategy(true));
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

Here is how to submit an Excel file, for example (pass a `WaitingStrategy` to actually
wait for its results - see the sections above for more details):

```c#
var validation = await verifalia
    .EmailValidations
    .SubmitAsync(new FileInfo("that-file.xslx"));
```

For more advanced options, just pass `FileValidationRequest` instance to the `SubmitAsync()`
method:

```c#
var validation = await verifalia
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

var validation = await verifalia
    .EmailValidations
    .SubmitAsync(inputStream,
        MediaTypeHeaderValue.Parse(WellKnownMimeContentTypes.TextPlain)); // text/plain
```

### Completion callbacks ###

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
        CompletionCallback = new Uri("https://your-website-here/foo/bar"),
        // TODO: Other settings, if needed
    });
```

Note that completion callbacks are invoked asynchronously and it could take up to
several seconds for your callback URL to get invoked.

### How to export validated entries in different output formats ###

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

### Don't forget to clean up, when you are done ###

Verifalia automatically deletes completed jobs after a configurable
data-retention period (minimum 5 minutes, maximum 30 days) but it is strongly advisable that
you delete your completed jobs as soon as possible, for privacy and security reasons. To do that, you can invoke the `DeleteAsync()` method passing the job Id you wish to get rid of:

```c#
await verifalia
    .EmailValidations
    .DeleteAsync(validation.Id);
```

Once deleted, a job is gone and there is no way to retrieve its email validation results.

### Iterating over your email validation jobs ###

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
	Console.WriteLine("Id: {0}, submitted: {1}, status: {2}, entries: {3}",
		jobOverview.Id,
		jobOverview.SubmittedOn,
		jobOverview.Status,
		jobOverview.NoOfEntries);
}

// Prints out something like:
// Id: a7784f9a-86d4-436c-b8e4-f72f2bd377ac, submitted: 8/2/2019 10:27:29 AM, status: InProgress, entries: 9886
// Id: 86d57c00-147a-4736-88cc-c918260c67c6, submitted: 8/2/2019 10:27:29 AM, status: Completed, entries: 1
// Id: 594bbb0f-6f12-481c-926f-606cfefc1cd5, submitted: 8/2/2019 10:27:28 AM, status: Completed, entries: 1
// Id: a5c1cd5b-39cc-43bc-9a3a-ee4a0f80ee6d, submitted: 8/2/2019 10:27:26 AM, status: InProgress, entries: 226
// Id: b6f69e30-60dd-4c21-b2cb-e73ba75fb278, submitted: 8/2/2019 10:27:21 AM, status: Completed, entries: 12077
// Id: 5e5a97dc-459f-4edf-a607-47371c32aa94, submitted: 8/2/2019 10:27:18 AM, status: Deleted, entries: 1009
// ...
```

> The `ListAsync()` method uses the *C# 8.0 async enumerable* feature; for previous language support please check the `ListSegmentedAsync()` methods group.

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
// 20200301 - credit packs: 1965.68, free daily credits: 200
// 20200226 - credit packs: 0, free daily credits: 185.628
// 20200225 - credit packs: 15.32, free daily credits: 200
// ...
```

> The `ListDailyUsagesAsync()` method uses the *C# 8.0 async enumerable* feature; for previous language support please check the `ListDailyUsagesSegmentedAsync()` methods group.

## Changelog / What's new

This section lists the changelog for the current major version of the library: for older versions,
please see the [project releases](https://github.com/verifalia/verifalia-csharp-sdk/releases).

### v3.1

Released on December 2<sup>nd</sup>, 2021

- Added support for API v2.3, including the ability to specify a completion callback URL
and support for exporting validated entries in multiple output formats
- Added support for .NET 6.0
- Improved documentation in code 

### v3.0

Released on January 22<sup>nd</sup>, 2021

- Breaking change: IRestClient.InvokeAsync() now accepts a factory of HttpContent, which allows to work-around an issue with certain versions of .NET Standard and .NET Framework. The issue has been fixed in .NET Core since then, but one of our dependencies targets .NET Standard.
If you don't implement or use IRestClient directly in your code (which should be super rare) then you will not be affected by this change.
- Fixed an issue which prevented MultiplexedRestClient to properly retry HTTP invocations on failures.
- Fixed an issue with IAsyncEnumerable support on .NET Core 3.1 (was mistakenly disabled in previous releases).
- Improved the way we throw OperationCanceledExceptions in several code paths.
- Improved unit tests.

### v2.4

Released on November 20<sup>th</sup>, 2020

- Added support for submitting files (plain text, CSV/TSV, Excel .xls / .xlsx) for validation
- Improved .NET 5.0 support

### v2.3

Released on November 13<sup>th</sup>, 2020

- Added support for .NET 5.0
- Added support for API v2.2
- Added a missing validation entry status

### v2.2

Released on February 21<sup>st</sup>, 2020

- Added support for API v2.1:
    - configurable data retention period
    - new `Relaxed` deduplication algorithm
    - new `DomainHasNullMx` status code
    
### v2.1

Released on November 22<sup>nd</sup>, 2019

- Added support for bearer authentication
- Added support for TOTP multi-factor auth
- Added specific package build for net48 
- Improved code comments and documentation

### v2.0

Released on August 2<sup>nd</sup>, 2019

- Added support for API v2.0
- Improved documentation
- Fixed an issue which may cause an unnecessary delay while querying for jobs results

[0]: https://verifalia.com
[1]: https://github.com/verifalia/verifalia-csharp-sdk/archive/master.zip
[2]: https://verifalia.com/developers#authentication
[3]: https://verifalia.com/client-area#/users/new
[4]: https://verifalia.com/sign-up
[5]: https://verifalia.com/client-area#/credits/add
