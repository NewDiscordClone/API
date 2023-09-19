using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.GroupChats.Commands.LeaveFromGroupChat
{
    public class LeaveFromGroupChatCommandValidator : AbstractValidator<LeaveFromGroupChatCommand>
    {
        public LeaveFromGroupChatCommandValidator()
        {
            RuleFor(c => c.ChatId).NotNull().IsObjectId();
        }
    }
}
