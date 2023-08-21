using Application.Interfaces;
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
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<GetServerLookupDto>> Handle(GetServersRequest request, CancellationToken cancellationToken)
        {
            List<GetServerLookupDto> servers = await _appDbContext.Servers
                .Where(server => server.ServerProfiles
                .Any(profile => profile.User.Id == request.UserId))
                .ProjectTo<GetServerLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return servers;
        }


    }
}