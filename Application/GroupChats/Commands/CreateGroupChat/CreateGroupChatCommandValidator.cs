using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.GroupChats.Commands.CreateGroupChat
{
    public class CreateGroupChatCommandValidator : AbstractValidator<CreateGroupChatCommand>
    {
        public CreateGroupChatCommandValidator()
        {
            RuleFor(x => x.Title)!.RequiredMaximumLength(Constants.Channel.TitleMaxLength)
                .When(x => x.Title is not null);
            RuleFor(x => x.Image)!.IsMedia();
            RuleFor(x => x.UsersId).Must(u => u.Count >= 3)
                .WithMessage(users => $"You cant create group chat with less than 3 people. You added {users.UsersId.Count} users");
        }
    }
}
