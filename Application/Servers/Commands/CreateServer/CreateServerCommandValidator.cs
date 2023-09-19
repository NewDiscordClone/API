using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.CreateServer
{
    public class CreateServerCommandValidator : AbstractValidator<CreateServerCommand>
    {
        public CreateServerCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(Constants.ServerProfile.DisplayNameMaxLength);
            RuleFor(x => x.Image)!.IsMedia();
        }
    }
}
