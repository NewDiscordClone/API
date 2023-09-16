using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.CreateServer
{
    public class CreateServerCommandValidator : AbstractValidator<CreateServerCommand>
    {
        public CreateServerCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Image)!.IsMedia().When(x => x.Image is not null);
        }
    }
}
