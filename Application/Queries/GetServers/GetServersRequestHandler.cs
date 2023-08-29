using Application.Interfaces;
using Application.Models;
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
            Context.SetToken(cancellationToken);
            
            List<GetServerLookupDto> servers = new();
            (await Context.Servers.FilterAsync(s => s.ServerProfiles.Any(sp => sp.User.Id == UserId)))
                .ForEach(s => servers.Add(
                    new GetServerLookupDto
                    {
                        Id = s.Id,
                        Image = s.Image,
                        Title = s.Title
                    }));
            return servers;
        }

        public GetServersRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}