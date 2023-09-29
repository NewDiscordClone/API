using FluentValidation;
using Sparkle.Application.Common.Constants;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileDisplayName
{
    public class ChangeServerProfileDisplayNameCommandValidator : AbstractValidator<ChangeServerProfileDisplayNameCommand>
    {
        public ChangeServerProfileDisplayNameCommandValidator()
        {
            RuleFor(x => x.NewDisplayName).MaximumLength(Constants.ServerProfile.DisplayNameMaxLength);
            RuleFor(x => x.ProfileId).NotNull();
        }
    }
}
