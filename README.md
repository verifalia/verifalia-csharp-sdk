Verifalia REST API - .NET SDK and helper library
================================================

Verifalia provides a simple HTTPS-based API for validating email addresses and checking whether or not they are deliverable. Learn more at http://verifalia.com

## Adding Verifalia REST API support to your .NET solution ##

The easiest way to add support for the Verifalia REST API to your .NET solution is to [download the SDK source project from github][1], eventually extract
it to a folder of your choice and add a reference from your own solution to the SDK project. The SDK project is a C# project with support for Visual Studio
2010, 2012 and 2013, which can be referenced and used with any other .NET language too, including Visual Basic (VB.NET), C++/CLI, J#, IronPython, IronRuby, F# and PowerShell.
Learn more at [http://verifalia.com][0].

### Sample usage ###

The example below shows how to have your application initiate and validate a couple of email addresses using the Verifalia .NET helper library:

```c#
using Verifalia.Api;

var restClient = new VerifaliaRestClient("YOUR-ACCOUNT-SID", "YOUR-AUTH-TOKEN");

var result = restClient.EmailValidations.Submit(new[]
	{
		"alice@example.com",
		"bob@example.net",
		"carol@example.org"
	},
	new WaitForCompletionOptions(TimeSpan.FromMinutes(1)));

if (result != null) // Result is null if timeout expires
{
	foreach (var entry in result.Entries)
	{
		Console.WriteLine("Address: {0} => Result: {1}",
			entry.InputData,
			entry.Status);
	}
}
```

Internally, the `Submit()` method sends the email addresses to the Verifalia servers and then polls them until the validations complete.
Instead of relying on this automatic polling behavior, you may even manually query the Verifalia servers by way of the `Query()` method, as shown below:

```c#
var result = restClient.EmailValidations.Submit(new[]
	{
		"alice@example.com",
		"bob@example.net",
		"carol@example.org"
	},
	WaitForCompletionOptions.DontWait));

while (result.Status != ValidationStatus.Completed)
{
	result = restClient.EmailValidations.Query(result.UniqueID, WaitForCompletionOptions.DontWait);
	Thread.Sleep(5000);
}

// TODO: Display the validation results
```

[0]: http://verifalia.com
[1]: https://github.com/verifalia/verifalia-csharp-sdk/archive/master.zip