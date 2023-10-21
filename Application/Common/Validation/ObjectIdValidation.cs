using FluentValidation;
using Sparkle.Application.Common.RegularExpressions;

namespace Sparkle.Application.Common.Validation
{
    public static class ObjectIdValidation
    {
        public static IRuleBuilderOptions<T, string> IsObjectId<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(Regexes.ObjectIdRegex)
                .WithMessage("{PropertyValue} is not ObjectId");
        }
    }
}
