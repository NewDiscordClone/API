using FluentValidation;
using Sparkle.Application.Common.RegularExpressions;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Messages.Commands.AddReaction
{
    public class AddReactionCommandValidator : AbstractValidator<AddReactionCommand>
    {
        public AddReactionCommandValidator()
        {
            RuleFor(q => q.Emoji).NotNull().Matches(Regexes.EmojiRegex)
                .WithMessage("Emoji must be a valid emoji (:smile:)");
            RuleFor(q => q.MessageId).NotNull().IsObjectId();
        }
    }
}
