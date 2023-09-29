using AutoMapper;
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
                .Where(user => user.Id == UserId)
                .SelectMany(user => user.UserProfiles)
                .Where(profile => profile.ChatId == null)
                .Select(profile => (profile as ServerProfile)!.ServerId)
                .ToListAsync(cancellationToken);

            List<Server> servers = await Context.Servers
                .FilterAsync(server => serverIds.Contains(server.Id), cancellationToken);

            List<ServerLookUpDto> dtos = servers.ConvertAll(Mapper.Map<ServerLookUpDto>);

            return dtos;
        }

        public ServersListQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}