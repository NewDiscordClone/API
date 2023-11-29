using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Commands.RemoveReaction
{
    public class RemoveReactionCommandValidator : AbstractValidator<RemoveReactionCommand>
    {
        public RemoveReactionCommandValidator()
        {
            RuleFor(c => c.MessageId).NotNull().IsObjectId();
            // RuleFor(c => c.ReactionIndex).GreaterThanOrEqualTo(0);
        }
    }
}
