using Application.Common.Interfaces;
using MediatR;
using MongoDB.Driver;

namespace Application.Common.Servers.Queries.GetServers
{
    public class GetServersRequestHandler : RequestHandlerBase,
        IRequestHandler<GetServersRequest, List<GetServerLookupDto>>
    {
        public async Task<List<GetServerLookupDto>> Handle(GetServersRequest request,
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

        public GetServersRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}