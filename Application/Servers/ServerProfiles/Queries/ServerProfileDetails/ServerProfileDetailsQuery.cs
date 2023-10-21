using MediatR;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.ServerProfileDetails
{
    public record ServerProfileDetailsQuery(Guid ProfileId) : IRequest<ServerProfileViewModel>;
}
