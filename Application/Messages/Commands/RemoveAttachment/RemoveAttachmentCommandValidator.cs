using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Commands.RemoveAttachment
{
    public class RemoveAttachmentCommandValidator : AbstractValidator<RemoveAttachmentCommand>
    {
        public RemoveAttachmentCommandValidator()
        {
            RuleFor(c => c.MessageId).NotNull().IsObjectId();
            RuleFor(c => c.AttachmentIndex).GreaterThanOrEqualTo(0);
        }
    }
}
