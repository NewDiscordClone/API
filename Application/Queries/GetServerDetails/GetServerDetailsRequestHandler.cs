using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetServerDetails
{
    public class GetServerDetailsRequestHandler : RequestHandlerBase,
        IRequestHandler<GetServerDetailsRequest, ServerDetailsDto>
    {
        public async Task<ServerDetailsDto> Handle(GetServerDetailsRequest request, CancellationToken cancellationToken)
        {
            Server server = await Context.FindByIdAsync<Server>(request.ServerId, cancellationToken,
                "ServerProfiles",
                "Channels",
                "Roles");
            if (server.ServerProfiles.Find(sp => sp.User.Id == UserId) == null)
                throw new NoPermissionsException("User are not a member of the Server");
            return Mapper.Map<ServerDetailsDto>(server);
        }

        public GetServerDetailsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(context, userProvider, mapper)
        { }
    }
}