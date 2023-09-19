using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.UpdateServer
{
    public class UpdateServerCommandValidator : AbstractValidator<UpdateServerCommand>
    {
        public UpdateServerCommandValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
        }
    }
}
