using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.GetServerProfiles
{
    public record ServerProfilesQuery : IRequest<List<ServerProfile>>
    {
        public string ServerId { get; init; }
    }
}
