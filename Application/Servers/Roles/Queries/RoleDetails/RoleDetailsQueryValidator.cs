using FluentValidation;

namespace Sparkle.Application.Servers.Roles.Queries.RoleDetails
{
    public class RoleDetailsQueryValidator : AbstractValidator<RoleDetailsQuery>
    {
        public RoleDetailsQueryValidator()
        {
            RuleFor(q => q.RoleId).NotNull();
        }
    }
}
