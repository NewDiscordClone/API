using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.Servers.CreateServer
{
    public class CreateServerRequestHandler : RequestHandlerBase, IRequestHandler<CreateServerRequest, string>
    {
        public async Task<string> Handle(CreateServerRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            UserLookUp ownerLookUp = Mapper.Map<UserLookUp>(await Context.SqlUsers.FindAsync(UserId));

            Server server = new()
            {
                Title = request.Title,
                Image = request.Image,
                Owner = ownerLookUp
            };

            server.ServerProfiles.Add(new() { User = ownerLookUp });

            await Context.Servers.AddAsync(server);
            return server.Id;
        }

        public CreateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
            : base(context, userProvider, mapper)
        {
        }
    }
}
