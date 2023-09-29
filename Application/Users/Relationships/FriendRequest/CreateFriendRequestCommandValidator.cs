using FluentValidation;

namespace Sparkle.Application.Users.Relationships.FriendRequest
{
    public class CreateFriendRequestCommandValidator : AbstractValidator<CreateFriendRequestCommand>
    {
        public CreateFriendRequestCommandValidator()
        {
            RuleFor(c => c.FriendId).NotNull();
        }
    }
}
