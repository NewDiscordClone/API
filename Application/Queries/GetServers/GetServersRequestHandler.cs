using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Queries.GetServer
{
    public class GetServersRequestHandler : RequestHandlerBase,
        IRequestHandler<GetServersRequest, List<GetServerLookupDto>>
    {
        public async Task<List<GetServerLookupDto>> Handle(GetServersRequest request,
            CancellationToken cancellationToken)
        {
            List<GetServerLookupDto> servers = new();
            await Context.Servers.Find(
                    Builders<Server>.Filter.ElemMatch(s => s.ServerProfiles,
                        Builders<ServerProfile>.Filter.Eq(sp => sp.User.Id, UserId)))
                .ForEachAsync(s => servers.Add(
                    new GetServerLookupDto
                    {
                        Id = s.Id,
                        Image = s.Image,
                        Title = s.Title
                    }), cancellationToken: cancellationToken);
            return servers;
        }

        public GetServersRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}