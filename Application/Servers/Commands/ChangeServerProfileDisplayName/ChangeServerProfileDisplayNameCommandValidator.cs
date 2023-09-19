using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.ChangeServerProfileDisplayName
{
    public class ChangeServerProfileDisplayNameCommandValidator : AbstractValidator<ChangeServerProfileDisplayNameCommand>
    {
        public ChangeServerProfileDisplayNameCommandValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
            RuleFor(x => x.NewDisplayName).RequiredMaximumLength(Constants.ServerProfile.DisplayNameMaxLength);
            RuleFor(x => x.UserId).NotNull();
        }
    }
}
