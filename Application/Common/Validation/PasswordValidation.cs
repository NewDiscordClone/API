using FluentValidation;
using Sparkle.Application.Common.RegularExpressions;

namespace Sparkle.Application.Common.Validation
{
    public static partial class PasswordValidation
    {
        public static IRuleBuilderOptions<T, string> HasUppercase<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(Regexes.Password.HasUpperCase())
                .WithMessage("Password must contain at least one uppercase letter");
        }

        public static IRuleBuilderOptions<T, string> HasLowercase<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(Regexes.Password.HasLowerCase())
                .WithMessage("Password must contain at least one lowercase letter");
        }

        public static IRuleBuilderOptions<T, string> HasDigit<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(Regexes.Password.HasDigit())
                .WithMessage("Password must contain at least one digit");
        }

        public static IRuleBuilderOptions<T, string> HasSpecialCharacter<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(Regexes.Password.HasSpecialCharacter())
                .WithMessage("Password must contain at least one special character");
        }


        public static IRuleBuilderOptions<T, string> IsPassword<T>(this IRuleBuilder<T, string> ruleBuilder, int miniMumLength = 6)
        {
            return ruleBuilder
                .MinimumLength(miniMumLength)
                .HasUppercase()
                .HasLowercase()
                .HasDigit()
                .HasSpecialCharacter();
        }
    }
}
