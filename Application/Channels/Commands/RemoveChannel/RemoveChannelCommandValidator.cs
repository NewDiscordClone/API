using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Channels.Commands.RemoveChannel
{
    public class RemoveChannelCommandValidator : AbstractValidator<RemoveChannelCommand>
    {
        public RemoveChannelCommandValidator()
        {
            RuleFor(x => x.ChatId).NotNull().IsObjectId();
        }
    }
}
