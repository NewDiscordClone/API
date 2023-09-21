using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.BanUser
{
    public class BanUserCommandValidator : AbstractValidator<BanUserCommand>
    {
        public BanUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.ServerId).IsObjectId();
        }
    }
}
