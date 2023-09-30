using FluentValidation;
using Sparkle.Application.Common.Constants;

namespace Sparkle.Application.Users.Commands.ChangeDisplayName
{
    public class ChangeDisplayNameCommandValidator : AbstractValidator<ChangeDisplayNameCommand>
    {
        public ChangeDisplayNameCommandValidator()
        {
            RuleFor(x => x.DisplayName).MaximumLength(Constants.User.UserNameMaxLength);
        }
    }
}
