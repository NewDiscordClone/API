using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application.Servers.Queries.GetServers
{
    public class GetServersQueryHandler : RequestHandlerBase,
        IRequestHandler<GetServersQuery, List<GetServerLookupDto>>
    {
        public async Task<List<GetServerLookupDto>> Handle(GetServersQuery query,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            List<GetServerLookupDto> servers = new();
            (await Context.Servers.FilterAsync(s => s.ServerProfiles.Any(sp => sp.UserId == UserId)))
                .ForEach(s => servers.Add(
                    new GetServerLookupDto
                    {
                        Id = s.Id,
                        Image = s.Image,
                        Title = s.Title
                    }));
            return servers;
        }

        public GetServersQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}