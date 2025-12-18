using System.ComponentModel.DataAnnotations;

namespace ChatAIze.PerfectEmail;

/// <summary>
/// Validation attribute that checks email addresses using <see cref="EmailValidator"/>.
/// </summary>
/// <remarks>
/// The value must be a string and is validated as-is (no trimming or normalization). This attribute
/// does not check for disposable domains.
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class EmailAttribute : ValidationAttribute
{
    /// <summary>
    /// Determines whether the specified value is a valid email string.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>True when the value is a non-null string that passes validation; otherwise false.</returns>
    /// <remarks>Non-string values are treated as invalid.</remarks>
    public override bool IsValid(object? value)
    {
        if (value is not string email)
        {
            return false;
        }

        return EmailValidator.IsValidEmail(email);
    }
}
