namespace ChatAIze.PerfectEmail;

/// <summary>
/// Validates email addresses using lightweight rules suitable for common forms.
/// </summary>
/// <remarks>
/// These checks are intentionally simpler than full RFC 5322 validation and focus on:
/// <list type="bullet">
/// <item><description>Length 6-100 and not starting with '+' or '-'.</description></item>
/// <item><description>Exactly one '@' with at least one '.' after it.</description></item>
/// <item><description>Allowed characters: letters, digits, '.', '-', '+', '@', '_'.</description></item>
/// <item><description>'+' allowed only in the local part; '_' allowed only in the local part and must be between alphanumerics.</description></item>
/// <item><description>Dots must be between alphanumerics; in the domain, '.' and '-' cannot be adjacent.</description></item>
/// <item><description>Final domain segment length &gt;= 2 and contains no digits or '-'.</description></item>
/// </list>
/// </remarks>
public static class EmailValidator
{
    private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-+@_";

    /// <summary>
    /// Determines whether the provided email is considered valid by the library's rules.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>True when the email passes validation; otherwise false.</returns>
    /// <remarks>The input is evaluated as-is; trim whitespace before calling if needed.</remarks>
    public static bool IsValidEmail(string? email)
    {
        // Quick rejects for null, length, or invalid starting characters.
        if (email == null || email.Length < 6 || email.Length > 100 || email[0] == '-' || email[0] == '+')
        {
            return false;
        }

        var atIndex = -1;
        var lastDotIndex = -1;
        var isLastPartInvalid = false;

        // Single pass enforces one '@', placement rules, and allowed characters.
        for (var i = 0; i < email.Length; i++)
        {
            var currentChar = email[i];

            if (atIndex != -1 && currentChar == '+')
            {
                // '+' is allowed only in the local part.
                return false;
            }

            if (atIndex != -1 && (currentChar == '-' || currentChar == '.') && (email[i - 1] == '-' || email[i - 1] == '.'))
            {
                // After '@', disallow consecutive '.' and/or '-'.
                return false;
            }

            if (currentChar == '@')
            {
                if (i == 0 || atIndex != -1)
                {
                    return false;
                }

                atIndex = i;
                continue;
            }

            if (currentChar == '.')
            {
                // Dots must be between alphanumerics; in the domain, track the last dot for TLD checks.
                if (i == 0 || i == email.Length - 1 || !char.IsLetterOrDigit(email[i - 1]) || !char.IsLetterOrDigit(email[i + 1]))
                {
                    return false;
                }

                if (atIndex != -1)
                {
                    lastDotIndex = i;
                    isLastPartInvalid = false;
                }

                continue;
            }

            if (currentChar == '_' && (atIndex != -1 || i == 0 || i == email.Length - 1 || !char.IsLetterOrDigit(email[i - 1]) || !char.IsLetterOrDigit(email[i + 1])))
            {
                // '_' is allowed only in the local part and must be surrounded by alphanumerics.
                return false;
            }

            if (!ValidCharacters.Contains(currentChar))
            {
                return false;
            }

            if (currentChar == '-' || char.IsDigit(currentChar))
            {
                // Track digits/hyphens; cleared on domain dots and used to validate the final segment.
                isLastPartInvalid = true;
            }
        }

        // Require a single '@', at least one dot after it, TLD length >= 2, and no digits/hyphens in the final segment.
        return atIndex != -1 && lastDotIndex > atIndex && email.Length - lastDotIndex >= 3 && !isLastPartInvalid;
    }
}
