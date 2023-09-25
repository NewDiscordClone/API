using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.BanUser
{
    public class BanUserCommandValidator : AbstractValidator<BanUserCommand>
    {
        public BanUserCommandValidator()
        {
            RuleFor(x => x.ProfileId).NotNull();
            RuleFor(x => x.ServerId).IsObjectId();
        }
    }
}
