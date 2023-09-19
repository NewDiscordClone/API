using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application.Servers.Queries.ServersList
{
    public class ServersListQueryHandler : RequestHandlerBase,
        IRequestHandler<ServersListQuery, List<ServerLookUpDto>>
    {
        public async Task<List<ServerLookUpDto>> Handle(ServersListQuery query,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            List<ServerLookUpDto> servers = new();
            (await Context.Servers.FilterAsync(s => s.ServerProfiles.Any(sp => sp.UserId == UserId)))
                .ForEach(s => servers.Add(
                    new ServerLookUpDto
                    {
                        Id = s.Id,
                        Image = s.Image,
                        Title = s.Title
                    }));
            return servers;
        }

        public ServersListQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}