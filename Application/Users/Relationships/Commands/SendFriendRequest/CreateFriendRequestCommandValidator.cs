using FluentValidation;

namespace Sparkle.Application.Users.Relationships.Commands.SendFriendRequest
{
    public class CreateFriendRequestCommandValidator : AbstractValidator<CreateFriendRequestCommand>
    {
        public CreateFriendRequestCommandValidator()
        {
            RuleFor(c => c.FriendId).NotNull();
        }
    }
}
