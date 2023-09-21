using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;
using Sparkle.Application.Roles.Common.Validation;

namespace Sparkle.Application.Roles.Commands.Update
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(c => c.Id).NotNull();

            RuleFor(c => c.Name).RequiredMaximumLength(
               Constants.ServerProfile.RoleNameMaxLength);

            RuleFor(c => c.Color).NotNull().NotEmpty();
            RuleFor(c => c.Color).IsColor();

            RuleFor(c => c.Priority).NotNull().GreaterThanOrEqualTo(0);

            RuleFor(c => c.Claims).NotNull();
            RuleForEach(c => c.Claims).SetValidator(new ClaimValidator());
        }
    }
}
