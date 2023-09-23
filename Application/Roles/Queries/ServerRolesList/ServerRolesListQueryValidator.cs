using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Roles.Queries.ServerRolesList
{
    public class ServerRolesListQueryValidator : AbstractValidator<ServerRolesListQuery>
    {
        public ServerRolesListQueryValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
        }
    }
}
