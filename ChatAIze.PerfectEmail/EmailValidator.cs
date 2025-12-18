namespace ChatAIze.PerfectEmail;

public static class EmailValidator
{
    private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-+@_";

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
