using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.LeaveServer
{
    public class LeaveServerCommandValidator : AbstractValidator<LeaveServerCommand>
    {
        public LeaveServerCommandValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
        }
    }
}
