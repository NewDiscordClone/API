using FluentValidation;

namespace Sparkle.Application.Servers.Roles.Commands.Delete
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(x => x.RoleId).NotNull();
        }
    }
}
