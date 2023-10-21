using FluentValidation;
using Sparkle.Application.Common.RegularExpressions;

namespace Sparkle.Application.Users.Common.Validation
{
    public static class UserValidators
    {
        public static IRuleBuilderOptions<T, string> Username<T>(this IRuleBuilder<T, string> builder)
        {
            return builder.Matches(Regexes.UserNameRegex)
                .WithMessage("The provided username contains invalid characters." +
                " Please use only numbers, lowercase letters, '_', and '.'.");
        }
    }
}
