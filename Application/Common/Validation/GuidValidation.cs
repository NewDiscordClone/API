using FluentValidation;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application.Common.Validation
{
    public static class GuidValidation
    {
        public static IRuleBuilderOptions<T, string> IsGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => Guid.TryParse(value, out _))
                .WithMessage("{PropertyValue} is not guid");
        }

        public static IRuleBuilderOptions<T, Guid> IsNotUser<T>(this IRuleBuilder<T, Guid> ruleBuilder,
            IAuthorizedUserProvider userProvider)
        {
            return ruleBuilder.NotEqual(userProvider.GetUserId());
        }
    }
}
