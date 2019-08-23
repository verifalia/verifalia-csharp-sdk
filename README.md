![Verifalia API](https://img.shields.io/badge/Verifalia%20API-v2.0-green)
[![NuGet](https://img.shields.io/nuget/v/Verifalia.svg)](https://www.nuget.org/packages/Verifalia)

Verifalia RESTful API - .NET SDK and helper library
===================================================

[Verifalia][0] provides a simple HTTPS-based API for validating email addresses in real-time and checking whether they are deliverable or not; this SDK library integrates with Verifalia and allows to [verify email addresses][0] under the following platforms:

- .NET Framework 4.5 (and higher)
- .NET Core 1.0 (and higher, including .NET Core 3.0)
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

Once you have your Verifalia credentials at hand, use them while creating a new instance of the `VerifaliaRestClient` type, which will be the starting point to every other operation against the Verifalia API:

```c#
using Verifalia.Api;

var verifalia = new VerifaliaRestClient("username", "password");
```

## Validating email addresses ##

Every operation related to verifying / validating email addresses is performed through the `EmailValidations` property exposed by the `VerifaliaRestClient` instance you created above. The property is filled with useful methods, each one having lots of overloads: in the next few paragraphs we are looking at the most used ones, so it is strongly advisable to explore the library and look at the embedded xmldoc help for other opportunities.

### How to validate an email address ###

To validate an email address from a .NET application you can invoke the `SubmitAsync()` method: it accepts one or more email addresses and any eventual verification options you wish to pass to Verifalia, including the expected results quality, deduplication preferences and processing priority.

In the next example, we are showing how to verify a single email address using this library; as the entire process is asynchronous, we are passing a `WaitingStrategy` value, asking `SubmitAsync()` to automatically wait for the job completion:

```c#
var validation = await verifalia
    .EmailValidations
    .SubmitAsync("batman@gmail.com", new WaitingStrategy(true));

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

### Don't forget to clean up, when you are done ###

Verifalia automatically deletes completed jobs after 30 days since their completion: deleting completed jobs is a best practice, for privacy and security reasons. To do that, you can invoke the `DeleteAsync()` method passing the job Id you wish to get rid of:

```c#
await verifalia
    .EmailValidations
    .DeleteAsync(validation.Id);
```

Once deleted, a job is gone and there is no way to retrieve its email validation(s).

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
// 20190801 - credit packs: 1965.68, free daily credits: 200
// 20190731 - credit packs: 0, free daily credits: 185.628
// 20190729 - credit packs: 15.32, free daily credits: 200
// ...
```

> The `ListDailyUsagesAsync()` method uses the *C# 8.0 async enumerable* feature; for previous language support please check the `ListDailyUsagesSegmentedAsync()` methods group.

[0]: https://verifalia.com
[1]: https://github.com/verifalia/verifalia-csharp-sdk/archive/master.zip
[2]: https://verifalia.com/developers#authentication
[3]: https://verifalia.com/client-area#/users/new
[4]: https://verifalia.com/sign-up
[5]: https://verifalia.com/client-area#/credits/add