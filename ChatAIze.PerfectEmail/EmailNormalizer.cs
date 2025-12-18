namespace ChatAIze.PerfectEmail;

/// <summary>
/// Provides simple normalization for email addresses.
/// </summary>
/// <remarks>
/// This does not attempt provider-specific canonicalization (for example, Gmail dot removal).
/// </remarks>
public static class EmailNormalizer
{
    /// <summary>
    /// Normalizes an email by trimming, lowercasing, and removing '+' tags from the local part.
    /// </summary>
    /// <param name="email">The email address to normalize.</param>
    /// <returns>The normalized email, or null when the input is null, whitespace, or invalid.</returns>
    /// <remarks>
    /// Only the portion after the first '+' in the local part is removed; the domain is kept
    /// intact aside from trimming and lowercasing.
    /// </remarks>
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
