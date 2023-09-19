using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Channels.Commands.RenameChannel
{
    public class RenameChannelCommandValidator : AbstractValidator<RenameChannelCommand>
    {
        public RenameChannelCommandValidator()
        {
            RuleFor(x => x.ChatId).NotNull().IsObjectId();
            RuleFor(x => x.NewTitle).RequiredMaximumLength(Constants.Channel.TitleMaxLength);
        }
    }
}
