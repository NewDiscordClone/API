using FluentValidation;
using Sparkle.Application.Common.Constants;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeTextStatusCommandValidator : AbstractValidator<ChangeTextStatusCommand>
    {
        public ChangeTextStatusCommandValidator()
        {
            RuleFor(c => c.TextStatus).MaximumLength(Constants.User.UserTextStatusMaxLength);
        }
    }
}
