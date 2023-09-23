using FluentValidation;

namespace Sparkle.Application.Roles.Queries.RoleDetails
{
    public class RoleDetailsQueryValidator : AbstractValidator<RoleDetailsQuery>
    {
        public RoleDetailsQueryValidator()
        {
            RuleFor(q => q.RoleId).NotNull();
        }
    }
}
