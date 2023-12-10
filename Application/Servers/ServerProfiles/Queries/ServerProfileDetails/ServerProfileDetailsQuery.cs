using MediatR;
using Sparkle.Domain.LookUps;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.ServerProfileDetails
{
    public record ServerProfileDetailsQuery(Guid ProfileId) : IRequest<ServerProfileViewModel>;
}
