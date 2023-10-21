using FluentValidation;
using Sparkle.Application.Users.Common.Validation;
using static Sparkle.Application.Common.Constants.Constants;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeUserNameCommandValidator : AbstractValidator<ChangeUserNameCommand>
    {
        public ChangeUserNameCommandValidator()
        {
            RuleFor(x => x.Username).NotNull().Username().
                MaximumLength(User.UserNameMaxLength);
        }
    }
}
