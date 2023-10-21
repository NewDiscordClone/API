using FluentValidation;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public class CancelFriendRequestCommandValidator : AbstractValidator<CancelFriendRequestCommand>
    {
        public CancelFriendRequestCommandValidator()
        {
            RuleFor(x => x.FriendId).NotEmpty();
        }
    }
}
