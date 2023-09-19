using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Channels.Commands.CreateChannel
{
    public class CreateChannelCommandValidator : AbstractValidator<CreateChannelCommand>
    {
        public CreateChannelCommandValidator()
        {
            RuleFor(x => x.Title).RequiredMaximumLength(Constants.Channel.TitleMaxLength);
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
        }
    }
}
