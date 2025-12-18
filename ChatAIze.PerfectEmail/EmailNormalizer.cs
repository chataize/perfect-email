namespace ChatAIze.PerfectEmail;

public static class EmailNormalizer
{
    public static string? NormalizeEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        // Normalize case/whitespace so validation and output are consistent.
        email = email.Trim().ToLowerInvariant();

        if (!EmailValidator.IsValidEmail(email))
        {
            return null;
        }

        var parts = email.Split('@');
        var localPart = parts[0];
        var domainPart = parts[1];
        var plusIndex = localPart.IndexOf('+');

        if (plusIndex != -1)
        {
            // Strip sub-addressing (e.g., "name+tag") for canonical comparisons.
            localPart = localPart[..plusIndex];
        }

        return $"{localPart}@{domainPart}";
    }
}
