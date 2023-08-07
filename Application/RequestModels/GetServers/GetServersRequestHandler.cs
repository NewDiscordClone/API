using Application.Interfaces;
using Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RequestModels.GetServer
{
    public class GetServersRequestHandler : IRequestHandler<GetServersRequest, List<GetServerDto>>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetServersRequestHandler(IAppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<List<GetServerDto>> Handle(GetServersRequest request, CancellationToken cancellationToken)
        {
            var user = await _appDbContext.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
            if (user == null) throw new NoSuchUserException();
            List<GetServerDto> servers = new();
            await _appDbContext.Servers
                .Where(server => server.ServerProfiles
                    .Find(profile => profile.User.Id == user.Id) != null)
                .ForEachAsync(server => servers.Add(Convertors.Convert(server)), cancellationToken: cancellationToken);
            return servers;
        }

        
    }
}