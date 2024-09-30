namespace ChatAIze.PerfectEmail;

public static class EmailNormalizer
{
    public static string NormalizeEmail(string email)
    {
        if (!EmailValidator.IsValidEmail(email))
        {
            throw new ArgumentException("Invalid email address.", nameof(email));
        }

        email = email.Trim().ToLowerInvariant();

        var parts = email.Split('@');
        var localPart = parts[0];
        var domainPart = parts[1];
        var plusIndex = localPart.IndexOf('+');

        if (plusIndex != -1)
        {
            localPart = localPart[..plusIndex];
        }

        return $"{localPart}@{domainPart}";
    }
}
