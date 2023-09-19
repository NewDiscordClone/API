using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.DeleteServer
{
    public class DeleteServerCommandValidator : AbstractValidator<DeleteServerCommand>
    {
        public DeleteServerCommandValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
        }
    }
}
