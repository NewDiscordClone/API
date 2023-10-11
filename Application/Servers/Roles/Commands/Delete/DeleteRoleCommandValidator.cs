using FluentValidation;
using Sparkle.Application.Servers.Roles.Common.Validation;

namespace Sparkle.Application.Servers.Roles.Commands.Delete
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(x => x.RoleId).NotDefaultRoleId();
        }
    }
}
