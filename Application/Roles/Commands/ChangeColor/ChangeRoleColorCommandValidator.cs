using FluentValidation;
using Sparkle.Application.Roles.Common.Validation;

namespace Sparkle.Application.Roles.Commands.ChangeColor
{
    public class ChangeRoleColorCommandValidator : AbstractValidator<ChangeRoleColorCommand>
    {
        public ChangeRoleColorCommandValidator()
        {
            RuleFor(c => c.RoleId).NotNull();

            RuleFor(c => c.Color).NotNull().Color();
        }
    }
}
