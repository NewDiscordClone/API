using FluentValidation;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application.Chats.PersonalChats.Commands.CreateChat
{
    public class CreatePersonalChatCommandValidator : AbstractValidator<CreatePersonalChatCommand>
    {
        public CreatePersonalChatCommandValidator(IAuthorizedUserProvider userProvider)
        {
            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("User id is required.");

            RuleFor(x => x.UserId).NotEqual(userProvider.GetUserId())
                .WithMessage("You can't create chat with yourself.");
        }
    }
}
