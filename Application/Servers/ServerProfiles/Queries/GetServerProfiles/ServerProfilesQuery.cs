using MediatR;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.GetServerProfiles
{
    public record ServerProfilesQuery : IRequest<List<ServerProfileLookup>>
    {
        public string ServerId { get; init; }
    }
}
