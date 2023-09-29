using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.GetServerProfiles
{
    public class ServerProfilesQueryValidator : AbstractValidator<ServerProfilesQuery>
    {
        public ServerProfilesQueryValidator()
        {
            RuleFor(x => x.ServerId).NotNull().IsObjectId();
        }
    }
}
