using FluentValidation;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public class AcceptFriendRequestCommandValidator : AbstractValidator<AcceptFriendRequestCommand>
    {
        public AcceptFriendRequestCommandValidator()
        {
            RuleFor(c => c.FriendId).NotEmpty();
        }
    }
}
