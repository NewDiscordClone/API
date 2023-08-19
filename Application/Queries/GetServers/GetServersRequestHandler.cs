using Application.Interfaces;
using Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GetServer
{
    public class GetServersRequestHandler : IRequestHandler<GetServersRequest, List<GetServerLookupDto>>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetServersRequestHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            this._appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<GetServerLookupDto>> Handle(GetServersRequest request, CancellationToken cancellationToken)
        {
            User user = await _appDbContext.FindByIdAsync<User>(request.UserId, cancellationToken, "ServerProfiles");

            List<GetServerLookupDto> servers = await _appDbContext.Servers
                .Where(server => server.ServerProfiles
                .Any(profile => profile.User.Id == request.UserId))
                .ProjectTo<GetServerLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return servers;
        }


    }
}