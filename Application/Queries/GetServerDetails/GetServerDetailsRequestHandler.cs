using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;
using MongoDB.Driver;

namespace Application.Queries.GetServerDetails
{
    public class GetServerDetailsRequestHandler : RequestHandlerBase,
        IRequestHandler<GetServerDetailsRequest, ServerDetailsDto>
    {
        public async Task<ServerDetailsDto> Handle(GetServerDetailsRequest request, CancellationToken cancellationToken)
        {
            Server server = await Context.FindByIdAsync<Server>(request.ServerId, cancellationToken);
            
            if (server.ServerProfiles.Find(sp => sp.User.Id == UserId) == null)
                throw new NoPermissionsException("User are not a member of the Server");
            var res = Mapper.Map<ServerDetailsDto>(server);
            res.Channels = await Context.Channels.Find(
                Builders<Channel>.Filter.Eq(c => c.ServerId, server.Id)
            ).ToListAsync(cancellationToken);
            return res;
        }

        public GetServerDetailsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(context, userProvider, mapper)
        { }
    }
}