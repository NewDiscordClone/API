using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.GroupChats.Commands.RemoveGroupChatMember
{
    public class RemoveGroupChatMemberCommandValidator : AbstractValidator<RemoveGroupChatMemberCommand>
    {
        public RemoveGroupChatMemberCommandValidator()
        {
            RuleFor(c => c.ChatId).NotNull().IsObjectId();
            RuleFor(c => c.MemberId).NotNull();
        }
    }
}
