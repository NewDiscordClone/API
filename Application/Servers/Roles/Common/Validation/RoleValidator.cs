using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.RegularExpressions;

namespace Sparkle.Application.Servers.Roles.Common.Validation
{
    public static class RoleValidator
    {
        public static IRuleBuilderOptions<T, string> IsColor<T>(
                       this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(Regexes.ColorRegex())
                .WithMessage("Color must be in #RRGGBB format");
        }

        public static IRuleBuilder<T, Guid> NotDefaultRole<T>(
            this IRuleBuilder<T, Guid> ruleBuilder)
        {
            return ruleBuilder.Must(id => !Constants.Roles.DefaultRoleIds.Contains(id))
               .WithMessage("Cannot delete or edit default role");
        }
    }
}
