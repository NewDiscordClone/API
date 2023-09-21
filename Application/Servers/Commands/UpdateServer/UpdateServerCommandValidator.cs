using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.UpdateServer
{
    public class UpdateServerCommandValidator : AbstractValidator<UpdateServerCommand>
    {
        public UpdateServerCommandValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
            RuleFor(x => x.Title).NotEmpty().MaximumLength(Constants.Server.TitleMaxLength);
            RuleFor(x => x.Image)!.IsMedia();
        }
    }
}
