using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Commands.PinMessage
{
    public class PinMessageCommandValidator : AbstractValidator<PinMessageCommand>
    {
        public PinMessageCommandValidator()
        {
            RuleFor(q => q.MessageId).NotNull().IsObjectId();
        }
    }
}
