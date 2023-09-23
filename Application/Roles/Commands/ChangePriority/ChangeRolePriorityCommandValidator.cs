using FluentValidation;

namespace Sparkle.Application.Roles.Commands.ChangePriority
{
    public class ChangeRolePriorityCommandValidator : AbstractValidator<ChangeRolePriorityCommand>
    {
        public ChangeRolePriorityCommandValidator()
        {
            RuleFor(c => c.RoleId).NotNull();

            RuleFor(c => c.Priority).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
