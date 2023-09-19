using FluentValidation;

namespace Sparkle.Application.Users.Commands.FriendRequest
{
    public class CreateFriendRequestCommandValidator : AbstractValidator<CreateFriendRequestCommand>
    {
        public CreateFriendRequestCommandValidator()
        {
            RuleFor(c => c.UserId).NotNull();
        }
    }
}
