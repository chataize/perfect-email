namespace ChatAIze.PerfectEmail;

public static class EmailValidator
{
    private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-+@";

    public static bool IsValidEmail(string? email)
    {
        if (email == null || email.Length < 6 || email.Length > 100 || email[0] == '-' || email[0] == '+')
        {
            return false;
        }

        var atIndex = -1;
        var lastDotIndex = -1;
        var isLastCharDigit = false;

        for (int i = 0; i < email.Length; i++)
        {
            var currentChar = email[i];

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
                if (atIndex != -1 && (email[i - 1] == '.' || email[i - 1] == '@'))
                {
                    return false;
                }

                lastDotIndex = i;
                isLastCharDigit = false;
                continue;
            }

            if (atIndex != -1 && (currentChar == '-' || currentChar == '+'))
            {
                return false;
            }

            if (!ValidCharacters.Contains(currentChar))
            {
                return false;
            }

            if (char.IsDigit(currentChar))
            {
                isLastCharDigit = true;
            }
        }

        return atIndex != -1 && lastDotIndex > atIndex && email.Length - lastDotIndex >= 3 && !isLastCharDigit;
    }
}
