using FluentValidation;
using Sparkle.Application.Common.RegularExpressions;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeAvatarCommandValidator : AbstractValidator<ChangeAvatarCommand>
    {
        public ChangeAvatarCommandValidator()
        {
            RuleFor(c => c.AvatarUrl).Matches(Regexes.UrlRegex)
                .WithMessage("{PropertyValue} is not url");
        }
    }
}
