using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Chats.GroupChats.Commands.ChangeGroupChatOwner
{
    public class ChangeGroupChatOwnerCommandValidator : AbstractValidator<ChangeGroupChatOwnerCommand>
    {
        public ChangeGroupChatOwnerCommandValidator()
        {
            RuleFor(c => c.ChatId).NotNull().IsObjectId();
            RuleFor(c => c.ProfileId).NotNull();
        }
    }
}
