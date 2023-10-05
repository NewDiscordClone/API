using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Chats.GroupChats.Commands.ChangeGroupChatImage
{
    public class ChangeGroupChatImageCommandValidator : AbstractValidator<ChangeGroupChatImageCommand>
    {
        public ChangeGroupChatImageCommandValidator()
        {
            RuleFor(c => c.ChatId).NotNull().IsObjectId();
            RuleFor(c => c.NewImage).NotNull().IsMedia();
        }
    }
}
