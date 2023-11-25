using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Queries.ServersList
{
    public class ServersListQueryHandler : RequestHandler,
        IRequestHandler<ServersListQuery, List<ServerLookUpDto>>
    {
        private readonly IServerRepository _serverRepository;
        private readonly IUserRepository _userRepository;

        public async Task<List<ServerLookUpDto>> Handle(ServersListQuery query, CancellationToken cancellationToken)
        {
            List<string> serverIds = await _userRepository
                .ExecuteCustomQuery(users => users
                .Where(user => user.Id == UserId)
                .SelectMany(user => user.UserProfiles)
                .Where(profile => profile.ChatId == null)
                .Select(profile => (profile as ServerProfile)!.ServerId))
                .ToListAsync(cancellationToken);

            List<Server> servers = await _serverRepository.ExecuteCustomQuery(servers => servers
                .Where(server => serverIds.Contains(server.Id)))
                .ToListAsync(cancellationToken);

            List<ServerLookUpDto> dtos = servers.ConvertAll(Mapper.Map<ServerLookUpDto>);

            return dtos;
        }

        public ServersListQueryHandler(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IServerRepository serverRepository,
            IUserRepository userRepository) :
            base(userProvider, mapper)
        {
            _serverRepository = serverRepository;
            _userRepository = userRepository;
        }
    }
}