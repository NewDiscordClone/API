using FluentValidation;
using Sparkle.Application.Common.RegularExpressions;

namespace Sparkle.Application.Common.Validation
{
    public static class MediaUrlValidation
    {
        public static IRuleBuilderOptions<T, string> IsMedia<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(Regexes.AvatarUrlRegex())
                .WithMessage("{PropertyValue} is not Media url");
        }
    }
}
