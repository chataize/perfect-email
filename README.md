# Perfect Email

Perfect Email is a fast, lightweight C# library for validating and normalizing email addresses, fixing common provider typos, and detecting disposable domains. It is designed for everyday form validation where you want simple, consistent rules and predictable results.

## Contents

- [Features](#features)
- [Design Goals](#design-goals)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [API Overview](#api-overview)
- [Choosing the Right API](#choosing-the-right-api)
- [Suggested Workflow](#suggested-workflow)
- [Null and Error Handling](#null-and-error-handling)
- [Behavior Examples](#behavior-examples)
- [Validation Rules](#validation-rules-emailvalidator)
- [Limitations and Non-goals](#limitations-and-non-goals)
- [Normalization Rules](#normalization-rules-emailnormalizer)
- [Domain Fixing](#domain-fixing-emailfixer)
- [Disposable Domain Detection](#disposable-domain-detection)
- [Performance Notes](#performance-notes)
- [Thread Safety](#thread-safety)
- [Target Framework](#target-framework)
- [License](#license)
- [Contributing](#contributing)

## Features

- Simple, consistent email validation rules (no regex, no RFC 5322 complexity)
- Normalization with trimming, lowercasing, and plus-tag removal
- Correction of common provider-domain typos (Gmail, Hotmail, iCloud, Outlook, Yahoo)
- Disposable email detection based on a curated public domain list
- Validation attribute for ASP.NET and DataAnnotations
- XML documentation output enabled for IntelliSense

## Design Goals

- Keep validation rules explicit and easy to reason about
- Avoid regex and heavy parsing to stay fast and predictable
- Provide safe normalization for storage and comparison
- Run entirely locally with no network calls

## Installation

### .NET CLI

```bash
dotnet add package ChatAIze.PerfectEmail
```

### Package Manager Console

```powershell
Install-Package ChatAIze.PerfectEmail
```

## Quick Start

```cs
using ChatAIze.PerfectEmail;

var isValid = EmailValidator.IsValidEmail("someone@example.com");
Console.WriteLine(isValid); // True

var normalized = EmailNormalizer.NormalizeEmail("  Name+tag@Example.com ");
Console.WriteLine(normalized); // name@example.com

var fixedEmail = EmailFixer.FixEmail("User@GmAiL.CoM");
Console.WriteLine(fixedEmail); // user@gmail.com

var isDisposable = DisposableEmailDetector.IsDisposableEmail("someone@0-mail.com");
Console.WriteLine(isDisposable); // True
```

## API Overview

### EmailValidator

```cs
bool isValid = EmailValidator.IsValidEmail("someone@example.com");
```

Validates email addresses using a lightweight rule set optimized for common forms. The input is evaluated as-is; trim whitespace before calling if needed.

### EmailNormalizer

```cs
string? normalized = EmailNormalizer.NormalizeEmail(" Name+tag@Example.com ");
```

Returns a normalized email or `null` when the input is null, whitespace, or invalid. Normalization trims whitespace, lowercases, and removes the `+tag` part from the local segment. It does not do provider-specific canonicalization such as Gmail dot removal.

### EmailFixer

```cs
string fixedEmail = EmailFixer.FixEmail("user@gmial.com");
```

Normalizes, validates, and corrects common domain typos for popular providers. Unknown domains are not corrected beyond normalization. Throws when the input is null, whitespace, or invalid. It does not remove plus tags; use `EmailNormalizer` if you need that.

### DisposableEmailDetector

```cs
bool isDisposableEmail = DisposableEmailDetector.IsDisposableEmail("someone@0-mail.com");
bool isDisposableDomain = DisposableEmailDetector.IsDisposableEmailDomain("0-mail.com");
```

Uses an embedded domain list sourced from https://github.com/disposable-email-domains/disposable-email-domains. Matching is a case-insensitive exact lookup. No DNS or MX checks are performed. Update the package to refresh the embedded list.

### EmailAttribute

```cs
using ChatAIze.PerfectEmail;

public sealed class AccountCreationRequest
{
    [Email]
    public required string Email { get; init; }

    public required string Password { get; init; }
}
```

Validates the property value using `EmailValidator`. This attribute does not check for disposable domains.

#### ASP.NET Example

```cs
using ChatAIze.PerfectEmail;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public sealed class AccountsController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(AccountCreationRequest request)
    {
        if (DisposableEmailDetector.IsDisposableEmail(request.Email))
        {
            return BadRequest("Disposable emails are not allowed.");
        }

        return NoContent();
    }
}
```

With `[ApiController]`, invalid models automatically return HTTP 400.

## Choosing the Right API

- Use `EmailValidator` when you only need a pass/fail check.
- Use `EmailNormalizer` when you want a canonical form for comparisons or storage.
- Use `EmailFixer` when you want to correct common provider-domain typos.
- Use `DisposableEmailDetector` when you want to block disposable domains.

## Suggested Workflow

```cs
var normalized = EmailNormalizer.NormalizeEmail(input);
if (normalized is null)
{
    return false; // invalid format
}

if (DisposableEmailDetector.IsDisposableEmail(normalized))
{
    return false; // block disposable domains
}

// store or use normalized
```

To auto-correct common provider typos, call `EmailFixer.FixEmail` instead and handle `ArgumentException` for invalid input.

## Null and Error Handling

- `EmailValidator.IsValidEmail` returns `false` for `null`.
- `EmailNormalizer.NormalizeEmail` returns `null` for `null`, whitespace, or invalid input.
- `EmailFixer.FixEmail` throws `ArgumentNullException` for `null`, and `ArgumentException` for whitespace or invalid input.
- `DisposableEmailDetector` methods return `false` for `null`.

## Behavior Examples

| Input | API | Result |
| --- | --- | --- |
| `"  Name+tag@Example.com "` | `EmailNormalizer` | `"name@example.com"` |
| `"user@gmial.com"` | `EmailFixer` | `"user@gmail.com"` |
| `"a@a.a1"` | `EmailValidator` | `false` |
| `"a..b@a.com"` | `EmailValidator` | `false` |
| `"a_b@a.com"` | `EmailValidator` | `true` |
| `"someone@0-mail.com"` | `DisposableEmailDetector.IsDisposableEmail` | `true` |

## Validation Rules (EmailValidator)

These rules are intentionally simpler than full RFC 5322 validation:

- Length must be 6-100 characters
- The first character cannot be `+` or `-`
- Exactly one `@`, with at least one `.` after it
- Allowed characters: letters, digits, `.`, `-`, `+`, `@`, `_`
- `+` is allowed only in the local part
- `_` is allowed only in the local part and must be between alphanumerics
- Dots must be between alphanumerics
- In the domain, `.` and `-` cannot be consecutive (`..`, `--`, `.-`, `-.`)
- Final domain segment length must be at least 2 and contain no digits or `-`

If you need full RFC validation (quoted local parts, comments, IDN, etc.), this library is intentionally not that.

## Limitations and Non-goals

- No RFC 5322 edge cases (quoted local parts, comments, display names)
- ASCII-only addresses (no internationalized domains or Unicode local parts)
- No DNS or MX checks; deliverability is out of scope
- No provider-specific canonicalization beyond the explicit typo list and plus-tag removal

## Normalization Rules (EmailNormalizer)

- Trims leading/trailing whitespace
- Lowercases the entire address, including the local part (case is not preserved)
- Removes the `+tag` portion from the local part
- Only the portion after the first `+` is removed
- Returns `null` if the email is invalid by `EmailValidator` rules

Example:

```cs
EmailNormalizer.NormalizeEmail("  Name+promo@Example.com ");
// name@example.com
```

## Domain Fixing (EmailFixer)

EmailFixer corrects common typos for these providers:

- gmail.com
- hotmail.com
- icloud.com
- outlook.com
- yahoo.com

It uses a known-typo list first, then a conservative fuzzy match limited to a single edit (substitution, adjacent transposition, insertion, or deletion). This keeps corrections safe and predictable.

All validation rules from `EmailValidator` apply before correction.

## Disposable Domain Detection

- The list is embedded at build time and loaded into a `FrozenSet` for fast lookups
- Input is trimmed and lowercased before matching
- If an `@` is present, the portion after the last `@` is treated as the domain
- No email format validation is performed; only the extracted domain is checked

## Performance Notes

- No regex is used in validation
- The validator scans once and uses constant memory
- Disposable domain checks are O(1) lookups
- EmailFixer normalizes with Trim/ToLowerInvariant, uses span comparisons for canonical domains, and
  allocates only when a corrected address is produced or a known-typo lookup is performed

## Thread Safety

- All APIs are stateless and safe to call concurrently.

## Target Framework

- net10.0

## License

GPL-3.0-or-later. See `LICENSE` for details.

## Contributing

Issues and pull requests are welcome. Please include tests for behavioral changes.
