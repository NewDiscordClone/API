using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.GroupChats.Commands.RemoveUserFromGroupChat
{
    public class RemoveUserFromGroupChatCommandValidator : AbstractValidator<RemoveUserFromGroupChatCommand>
    {
        public RemoveUserFromGroupChatCommandValidator()
        {
            RuleFor(c => c.ChatId).NotNull().IsObjectId();
        }
    }
}
