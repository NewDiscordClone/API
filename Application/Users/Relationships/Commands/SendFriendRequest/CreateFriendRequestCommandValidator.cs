using FluentValidation;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public class CreateFriendRequestCommandValidator : AbstractValidator<CreateFriendRequestCommand>
    {
        public CreateFriendRequestCommandValidator(IAuthorizedUserProvider userProvider)
        {
            RuleFor(c => c.FriendId).NotEmpty().NotCurrentUser(userProvider)
                .WithMessage("You can't add yourself");
        }
    }
}
