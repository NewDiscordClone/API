using FluentValidation;

namespace Sparkle.Application.Users.Commands.AcceptFriendRequest
{
    public class AcceptFriendRequestCommandValidator : AbstractValidator<AcceptFriendRequestCommand>
    {
        public AcceptFriendRequestCommandValidator()
        {
            RuleFor(c => c.UserId).NotNull();
        }
    }
}
