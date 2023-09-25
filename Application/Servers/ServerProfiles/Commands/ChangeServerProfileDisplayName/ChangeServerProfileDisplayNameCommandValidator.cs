using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileDisplayName
{
    public class ChangeServerProfileDisplayNameCommandValidator : AbstractValidator<ChangeServerProfileDisplayNameCommand>
    {
        public ChangeServerProfileDisplayNameCommandValidator()
        {
            RuleFor(x => x.NewDisplayName).RequiredMaximumLength(Constants.ServerProfile.DisplayNameMaxLength);
            RuleFor(x => x.ProfileId).NotNull();
        }
    }
}
