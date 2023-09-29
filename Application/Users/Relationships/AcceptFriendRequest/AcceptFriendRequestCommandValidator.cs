using FluentValidation;

namespace Sparkle.Application.Users.Relationships.AcceptFriendRequest
{
    public class AcceptFriendRequestCommandValidator : AbstractValidator<AcceptFriendRequestCommand>
    {
        public AcceptFriendRequestCommandValidator()
        {
            RuleFor(c => c.FriendId).NotNull();
        }
    }
}
