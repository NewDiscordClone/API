using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeAvatarCommandValidator : AbstractValidator<ChangeAvatarCommand>
    {
        public ChangeAvatarCommandValidator()
        {
            RuleFor(c => c.AvatarUrl)!.IsMedia()
                .WithMessage("{PropertyValue} is not url");
        }
    }
}
