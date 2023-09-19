using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Queries.GetMessages
{
    public class GetMessagesQueryValidator : AbstractValidator<GetMessagesQuery>
    {
        public GetMessagesQueryValidator()
        {
            RuleFor(q => q.ChatId).NotNull().IsObjectId();
            RuleFor(q => q.MessagesCount).GreaterThanOrEqualTo(0);
        }
    }
}
