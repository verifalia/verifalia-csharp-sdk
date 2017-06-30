Verifalia REST API - .NET SDK and helper library
================================================

Verifalia provides a simple HTTPS-based API for validating email addresses in real-time and checking whether or not they are deliverable. Learn more at http://verifalia.com

## Adding Verifalia REST API support to your .NET solution ##

The best and easiest way to add the Verifalia libraries to your .NET project is to use the NuGet package manager.

#### With Visual Studio IDE

From within Visual Studio, you can use the NuGet GUI to search for and install the Verifalia NuGet package. Or, as a shortcut, simply type the following command into the Package Manager Console:

    Install-Package Verifalia

#### Manual download and compilation
	
As an alternative way to add Verifalia to your .NET solution, you can [download the SDK source project from github][1], extract it to a folder of your choice and add a reference from your own project to the Verifalia SDK project. The SDK project is a C# project with support for Visual Studio 2015, which can be referenced and used with any other .NET language too, including Visual Basic (VB.NET), C++/CLI, J#, IronPython, IronRuby, F# and PowerShell.

Learn more at [http://verifalia.com][0].

### How to validate an email address ###

The C# example below shows how to have your .NET application validate a couple of email addresses using the Verifalia .NET helper library. While the example uses the Task-based
Asynchronous Pattern (TAP), the SDK also exposes synchronous (not async/await) alternatives for backward compatibility.

```c#
using Verifalia.Api;

var restClient = new VerifaliaRestClient("YOUR-ACCOUNT-SID", "YOUR-AUTH-TOKEN");

var result = await restClient
	.EmailValidations
	.SubmitAsync(new[]
		{
			"alice@example.com",
			"bob@example.net",
			"carol@example.org"
		},
		ResultPollingOptions.WaitUntilCompleted);
	
// At this point the addresses have been validated and we can show the results

foreach (var entry in result.Entries)
{
	Console.WriteLine("Address: {0} => Result: {1}",
		entry.InputData,
		entry.Status);
}
```

#### Alternative with manual polling ####

Internally, when the `ResultPollingOptions.WaitUntilCompleted` option is passed to `SubmitAsync()`, this method sends the email addresses to the
Verifalia servers and then polls them until the validations complete.
Instead of relying on this automatic polling behavior, the library also allows to manually submit your email addresses and then query the Verifalia
servers for their results by way of the `ResultPollingOptions.NoPolling` option and the `QueryAsync()` method, as shown below:

```c#
var result = await restClient
	.EmailValidations
	.SubmitAsync(new[]
		{
			"alice@example.com",
			"bob@example.net",
			"carol@example.org"
		},
		ResultPollingOptions.NoPolling));

// Manual polling of the results
		
while (result.Status != ValidationStatus.Completed)
{
	result = await restClient
		.EmailValidations
		.QueryAsync(result.UniqueID, ResultPollingOptions.NoPolling);

	await Task.Delay(TimeSpan.FromSeconds(5));
}

// At this point the addresses have been validated and we can show the results

foreach (var entry in result.Entries)
{
	Console.WriteLine("Address: {0} => Result: {1}",
		entry.InputData,
		entry.Status);
}
```

[0]: http://verifalia.com
[1]: https://github.com/verifalia/verifalia-csharp-sdk/archive/master.zip