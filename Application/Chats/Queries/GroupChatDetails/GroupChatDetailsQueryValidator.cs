using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Chats.Queries.GroupChatDetails
{
    public class GroupChatDetailsQueryValidator : AbstractValidator<PrivateChatDetailsQuery>
    {
        public GroupChatDetailsQueryValidator()
        {
            RuleFor(q => q.ChatId).NotNull().IsObjectId();
        }
    }
}
