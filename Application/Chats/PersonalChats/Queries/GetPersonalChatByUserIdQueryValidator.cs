using FluentValidation;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Chats.PersonalChats.Queries
{
    public class GetPersonalChatByUserIdQueryValidator : AbstractValidator<GetPersonalChatByUserIdQuery>
    {
        public GetPersonalChatByUserIdQueryValidator(IAuthorizedUserProvider userProvider)
        {
            RuleFor(query => query.UserId)
                .NotEmpty()
                .NotCurrentUser(userProvider)
                .WithMessage("You cant find chat with yourself");
        }
    }
}
