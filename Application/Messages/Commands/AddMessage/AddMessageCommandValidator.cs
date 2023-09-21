using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Commands.AddMessage
{
    public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand>
    {
        public AddMessageCommandValidator()
        {
            RuleFor(c => c.Text).MaximumLength(Constants.Message.MaxTextLength);
            RuleFor(c => c.ChatId).NotNull().IsObjectId();
            RuleFor(c => c.Attachments).NotNull().NotEmpty().When(c => string.IsNullOrEmpty(c.Text.Trim()));
        }
    }
}
