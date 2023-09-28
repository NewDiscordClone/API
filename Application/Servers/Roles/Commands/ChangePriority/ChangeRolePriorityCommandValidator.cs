using FluentValidation;
using Sparkle.Application.Servers.Roles.Common.Validation;

namespace Sparkle.Application.Servers.Roles.Commands.ChangePriority
{
    public class ChangeRolePriorityCommandValidator : AbstractValidator<ChangeRolePriorityCommand>
    {
        public ChangeRolePriorityCommandValidator()
        {
            RuleFor(c => c.RoleId).NotDefaultRole();

            RuleFor(c => c.Priority).NotNull().GreaterThan(0).LessThan(100);
        }
    }
}
