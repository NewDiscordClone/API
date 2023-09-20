using FluentValidation;

namespace Sparkle.Application.Roles.Commands.UpdateClaims
{
    public class UpdateRoleClaimsCommandValidator : AbstractValidator<UpdateRoleClaimsCommand>
    {
        public UpdateRoleClaimsCommandValidator()
        {
            RuleFor(c => c.RoleId).NotNull();

            RuleFor(c => c.Claims).NotNull();
        }
    }
}
