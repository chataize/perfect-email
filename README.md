# Perfect Email
An easy-to-use, high-performance email validator for C# .NET 8.0 with custom attribute and disposable domain detection.

## Installation
### .NET CLI
```bash
dotnet add package ChatAIze.PerfectEmail
```
### Package Manager Console
```powershell
Install-Package ChatAIze.PerfectEmail
```

## Usage
```cs
using ChatAIze.PerfectEmail;

bool isValidEmail = EmailValidator.IsValidEmail("someone@example.com");
Console.WriteLine(isValidEmail); // true

bool isDisposableEmail = DisposableEmailDetector.IsDisposableEmail("someone@0-mail.com");
Console.WriteLine(isDisposableEmail); // true

bool isDisposableEmailDomain = DisposableEmailDetector.IsDisposableEmailDomain("0-mail.com");
Console.WriteLine(isDisposableEmailDomain); // true
```

## Attribute Usage
The `Email` attribute performs address validation when the model is submitted. However, it does not check for disposable domains. An HTTP `400 Bad Request` status code is returned by default.
```cs
using ChatAIze.PerfectEmail;

namespace ChatAIze.ExampleAPI.Models;

public record AccountCreationRequest
{
    [Email]
    public required string Email { get; init; }

    public required string Password { get; init; }
}
```
```cs
using ChatAIze.ExampleAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatAIze.ExampleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AccountsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateAccount(AccountCreationRequest request)
    {
        Console.WriteLine($"Creating account for {request.Email}");
        return NoContent();
    }
}
```
