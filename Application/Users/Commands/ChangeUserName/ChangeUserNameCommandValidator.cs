using FluentValidation;
using Sparkle.Application.Users.Common.Validation;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeUserNameCommandValidator : AbstractValidator<ChangeUserNameCommand>
    {
        public ChangeUserNameCommandValidator()
        {
            RuleFor(x => x.Username).NotNull().Username();
        }
    }
}
