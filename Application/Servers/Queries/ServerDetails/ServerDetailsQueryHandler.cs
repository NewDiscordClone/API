using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Queries.ServerDetails
{
    public class ServerDetailsQueryHandler : RequestHandlerBase,
        IRequestHandler<ServerDetailsQuery, ServerDetailsDto>
    {
        public async Task<ServerDetailsDto> Handle(ServerDetailsQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(query.ServerId);

            ServerDetailsDto dto = Mapper.Map<ServerDetailsDto>(server);

            dto.Channels = await Context.Channels.FilterAsync(c => c.ServerId == server.Id);
            return dto;
        }

        public ServerDetailsQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}