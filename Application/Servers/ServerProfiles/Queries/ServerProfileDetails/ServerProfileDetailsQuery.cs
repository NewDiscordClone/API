using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.ServerProfileDetails
{
    public class ServerProfileDetailsQuery : IRequest<ServerProfile>
    {
        public Guid ProfileId { get; init; }
    }
}
