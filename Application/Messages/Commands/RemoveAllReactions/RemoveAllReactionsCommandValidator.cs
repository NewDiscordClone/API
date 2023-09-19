using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Commands.RemoveAllReactions
{
    public class RemoveAllReactionsCommandValidator : AbstractValidator<RemoveAllReactionsCommand>
    {
        public RemoveAllReactionsCommandValidator()
        {
            RuleFor(c => c.MessageId).NotNull().IsObjectId();
        }
    }
}
