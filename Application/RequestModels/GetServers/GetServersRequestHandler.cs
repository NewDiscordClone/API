using Application.Interfaces;
using Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RequestModels.GetServer
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
            User? user = await _appDbContext.Users
                .FindAsync(new object[] { request.UserId }, cancellationToken)
                ?? throw new NoSuchUserException();

            List<GetServerLookupDto> servers = await _appDbContext.Servers
                .Where(server => server.ServerProfiles
                .Find(profile => profile.User.Id == user.Id) != null)
                .ProjectTo<GetServerLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return servers;
        }


    }
}