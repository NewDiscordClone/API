using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Chats.Queries.PrivateChatDetails
{
    public class PrivateChatDetailsQueryValidator : AbstractValidator<PrivateChatDetailsQuery>
    {
        public PrivateChatDetailsQueryValidator()
        {
            RuleFor(q => q.ChatId).NotNull().IsObjectId();
        }
    }
}
