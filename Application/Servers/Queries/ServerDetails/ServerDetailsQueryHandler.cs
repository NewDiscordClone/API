using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Queries.ServerDetails
{
    public class ServerDetailsQueryHandler : RequestHandlerBase,
        IRequestHandler<ServerDetailsQuery, ServerDetailsDto>
    {
        private readonly IServerRepository _serverRepository;
        private readonly IChatRepository _chatRepository;
        public async Task<ServerDetailsDto> Handle(ServerDetailsQuery query, CancellationToken cancellationToken)
        {
            Server server = await _serverRepository.FindAsync(query.ServerId, cancellationToken);

            ServerDetailsDto dto = Mapper.Map<ServerDetailsDto>(server);

            dto.Channels = await _chatRepository.Channels
                .Where(c => c.ServerId == server.Id)
                .ToListAsync(cancellationToken);
            return dto;
        }

        public ServerDetailsQueryHandler(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IChatRepository chatRepository,
            IServerRepository serverRepository) : base(userProvider, mapper)
        {
            _chatRepository = chatRepository;
            _serverRepository = serverRepository;
        }
    }
}