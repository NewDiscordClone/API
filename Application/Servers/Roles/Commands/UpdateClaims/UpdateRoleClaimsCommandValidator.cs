using FluentValidation;
using Sparkle.Application.Servers.Roles.Common.Validation;

namespace Sparkle.Application.Servers.Roles.Commands.UpdateClaims
{
    public class UpdateRoleClaimsCommandValidator : AbstractValidator<UpdateRoleClaimsCommand>
    {
        public UpdateRoleClaimsCommandValidator()
        {
            RuleFor(c => c.RoleId).NotNull();

            RuleFor(c => c.Claims).NotNull();
            RuleForEach(c => c.Claims).SetValidator(new ClaimValidator());
        }
    }
}
