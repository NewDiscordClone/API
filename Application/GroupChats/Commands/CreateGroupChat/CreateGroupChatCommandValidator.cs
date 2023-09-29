using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.GroupChats.Commands.CreateGroupChat
{
    public class CreateGroupChatCommandValidator : AbstractValidator<CreateGroupChatCommand>
    {
        public CreateGroupChatCommandValidator(IAuthorizedUserProvider userProvider)
        {
            RuleFor(x => x.Title)!.RequiredMaximumLength(Constants.Channel.TitleMaxLength)
                .When(x => x.Title is not null);
            RuleFor(x => x.Image)!.IsMedia();
            RuleFor(x => x.UsersId)
                    .Must(u => u.Count >= 2)
                .WithMessage(users => $"You cant create group chat with less than 2 people. You added {users.UsersId.Count} user")
                    .Must(u => !u.Contains(userProvider.GetUserId()))
                .WithMessage("Users list should not contains current user");
        }
    }
}
