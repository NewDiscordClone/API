using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;
using Sparkle.Application.Servers.Roles.Common.Validation;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeName
{
    public class ChangeRoleNameCommandValidator : AbstractValidator<ChangeRoleNameCommand>
    {
        public ChangeRoleNameCommandValidator()
        {
            RuleFor(c => c.RoleId).NotDefaultRoleId();

            RuleFor(c => c.Name).RequiredMaximumLength(Constants.Roles.RoleNameMaxLength)
                .NotDefaultRoleName();
        }
    }
}
