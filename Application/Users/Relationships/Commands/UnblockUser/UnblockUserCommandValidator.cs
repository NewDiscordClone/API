using FluentValidation;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public class UnblockUserCommandValidator : AbstractValidator<UnblockUserCommand>
    {
        public UnblockUserCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}
