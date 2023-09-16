using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.Commands.ChangeServerProfileRoles
{
    public class UpdateServerProfileRolesCommandValidator : AbstractValidator<UpdateServerProfileRolesCommand>
    {
        public UpdateServerProfileRolesCommandValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
            RuleFor(x => x.Roles).NotNull();
            RuleFor(x => x.UserId).NotNull();
        }
    }
}
