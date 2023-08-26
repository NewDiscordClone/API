using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Servers.CreateServer
{
    public class CreateServerRequestHandler : RequestHandlerBase, IRequestHandler<CreateServerRequest, ObjectId>
    {
        public async Task<ObjectId> Handle(CreateServerRequest request, CancellationToken cancellationToken)
        {
            var ownerLookUp = Mapper.Map<UserLookUp>(await Context.FindByIdAsync<User>(UserId, cancellationToken));
            
            Server server = new()
            {
                Title = request.Title,
                Image = request.Image,
                Owner = ownerLookUp
            };
            server.ServerProfiles.Add(new() { User = ownerLookUp});

            await Context.Servers.InsertOneAsync(server, null, cancellationToken);
            return server.Id;
        }

        public CreateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) 
            : base(context, userProvider, mapper)
        {
        }
    }
}
