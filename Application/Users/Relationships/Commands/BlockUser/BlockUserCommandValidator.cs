using FluentValidation;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public class BlockUserCommandValidator : AbstractValidator<BlockUserCommand>
    {
        public BlockUserCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}
