using FluentValidation;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeRangePriority
{
    public class ChangeRangePriorityCommandValidator : AbstractValidator<ChangeRangePriorityCommand>
    {
        public ChangeRangePriorityCommandValidator()
        {
            RuleForEach(c => c.Priorities)
                .Must(p => p.Key != Guid.Empty)
                .WithMessage("Id must not be empty.")
                .Must(p => p.Value > 0 && p.Value < 100)
                .WithMessage("Priority must be between 1 and 99.");

            RuleFor(c => c.Priorities)
                .Must(BeUniquePriorities)
                .WithMessage("Priorities must be unique.");
        }

        private bool BeUniquePriorities(Dictionary<Guid, int> priorities)
        {
            HashSet<int> uniquePriorities = new();
            foreach ((Guid _, int priority) in priorities)
            {
                if (!uniquePriorities.Add(priority))
                {
                    return false;
                }
            }
            return true;
        }
    }

}
