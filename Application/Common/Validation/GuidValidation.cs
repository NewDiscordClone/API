using FluentValidation;

namespace BuberDinner.Application.Common.Validation
{
    public static class GuidValidation
    {
        public static IRuleBuilderOptions<T, string> IsGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => Guid.TryParse(value, out _))
                .WithMessage("{PropertyValue} is not guid");
        }
    }
}
