using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeName
{
    public class ChangeRoleNameCommandValidator : AbstractValidator<ChangeRoleNameCommand>
    {
        public ChangeRoleNameCommandValidator()
        {
            RuleFor(c => c.RoleId).NotNull();

            RuleFor(c => c.Name).RequiredMaximumLength(Constants.ServerProfile.RoleNameMaxLength);
        }
    }
}
