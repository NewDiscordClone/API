using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.JoinServer
{
    public class JoinServerCommandValidator : AbstractValidator<JoinServerCommand>
    {
        public JoinServerCommandValidator()
        {
            RuleFor(x => x.InvitationId).NotNull().IsObjectId();
        }
    }
}
