using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.GroupChats.Commands.ChangeGroupChatOwner
{
    public class ChangeGroupChatOwnerCommandValidator : AbstractValidator<ChangeGroupChatOwnerCommand>
    {
        public ChangeGroupChatOwnerCommandValidator()
        {
            RuleFor(c => c.ChatId).NotNull().IsObjectId();
            RuleFor(c => c.MemberId).NotNull();
        }
    }
}
