using FluentValidation;
using Sparkle.Application.Common.RegularExpressions;

namespace Sparkle.Application.Roles.Common.Validation
{
    public static class ColorValidator
    {
        public static IRuleBuilderOptions<T, string> IsColor<T>(
                       this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(Regexes.ColorRegex())
                .WithMessage("Color must be in #RRGGBB format");
        }
    }
}
