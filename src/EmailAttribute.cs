using System.ComponentModel.DataAnnotations;

namespace ChatAIze.PerfectEmail;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class EmailAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not string email)
        {
            return false;
        }

        return EmailValidator.IsValidEmail(email);
    }
}
