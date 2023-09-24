using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Queries.ServersList
{
    public class ServersListQueryHandler : RequestHandlerBase,
        IRequestHandler<ServersListQuery, List<ServerLookUpDto>>
    {
        public async Task<List<ServerLookUpDto>> Handle(ServersListQuery query,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            List<string> serverIds = await Context.Users
                .Select(user => user.UserProfiles)
                .OfType<ServerProfile>()
                .Select(profile => profile.ServerId)
                .ToListAsync(cancellationToken);

            List<Server> servers = await Context.Servers
                .FilterAsync(server => serverIds.Contains(server.Id));

            List<ServerLookUpDto> dtos = servers.ConvertAll(Mapper.Map<ServerLookUpDto>);

            return dtos;
        }

        public ServersListQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}