using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.KickUser
{
    public class KickUserCommandValidator : AbstractValidator<KickUserCommand>
    {
        public KickUserCommandValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
            RuleFor(x => x.UserId).NotNull();
        }
    }
}
