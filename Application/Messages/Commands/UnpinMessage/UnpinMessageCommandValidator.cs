using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Commands.UnpinMessage
{
    public class UnpinMessageCommandValidator : AbstractValidator<UnpinMessageCommand>
    {
        public UnpinMessageCommandValidator()
        {
            RuleFor(c => c.MessageId).NotNull().IsObjectId();
        }
    }
}
