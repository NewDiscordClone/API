using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;
using Sparkle.Application.Servers.Roles.Common.Validation;

namespace Sparkle.Application.Servers.Roles.Commands.Create
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(c => c.Name).RequiredMaximumLength(
                Constants.Roles.RoleNameMaxLength)
                .NotDefaultRoleName();

            RuleFor(c => c.Color).NotNull().NotEmpty();
            RuleFor(c => c.Color).Color();

            RuleFor(c => c.ServerId).NotNull().IsObjectId();

            RuleFor(c => c.Claims).NotNull();
            RuleForEach(c => c.Claims).SetValidator(new ClaimValidator());
        }
    }
}
