using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.UnbanUser
{
    public class UnbanUserCommandValidator : AbstractValidator<UnbanUserCommand>
    {
        public UnbanUserCommandValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
            RuleFor(x => x.UserId).NotNull();
        }
    }
}
