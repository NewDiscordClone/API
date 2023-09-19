using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Commands.EditMessage
{
    public class EditMessageCommandValidator : AbstractValidator<EditMessageCommand>
    {
        public EditMessageCommandValidator()
        {
            RuleFor(q => q.NewText).RequiredMaximumLength(Constants.Message.MaxTextLength);
            RuleFor(q => q.MessageId).NotNull().IsObjectId();
        }
    }
}
