using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Queries.GetPinnedMessages
{
    public class GetPinnedMessagesQueryValidator : AbstractValidator<GetPinnedMessagesQuery>
    {
        public GetPinnedMessagesQueryValidator()
        {
            RuleFor(q => q.ChatId).NotNull().IsObjectId();
        }
    }
}
