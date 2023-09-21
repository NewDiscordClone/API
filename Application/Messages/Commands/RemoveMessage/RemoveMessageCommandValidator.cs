using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Commands.RemoveMessage
{
    public class RemoveMessageCommandValidator : AbstractValidator<RemoveMessageCommand>
    {
        public RemoveMessageCommandValidator()
        {
            RuleFor(c => c.MessageId).NotNull().IsObjectId();
        }
    }
}
