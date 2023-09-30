using FluentValidation;

namespace Sparkle.Application.Users.Relationships.SendFriendRequest
{
    public class CreateFriendRequestCommandValidator : AbstractValidator<CreateFriendRequestCommand>
    {
        public CreateFriendRequestCommandValidator()
        {
            RuleFor(c => c.FriendId).NotNull();
        }
    }
}
