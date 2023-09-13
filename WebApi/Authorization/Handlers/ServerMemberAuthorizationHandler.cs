using Application.Common.Interfaces;
using Application.Queries.GetUser;
using Application.Users.Queries.GetUserDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using WebApi.Authorization.Requirements;

namespace WebApi.Authorization.Handlers
{
    public class ServerMemberAuthorizationHandler : AuthorizationHandler<ServerMemberRequirement>
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizedUserProvider _userProvider;

        public ServerMemberAuthorizationHandler(IMediator mediator, IAuthorizedUserProvider userProvider)
        {
            _mediator = mediator;
            _userProvider = userProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ServerMemberRequirement requirement)
        {
            Guid userId = _userProvider.GetUserId();
            GetUserDetailsDto user = await _mediator.Send(new GetUserDetailsRequest
            {
                UserId = userId,
                ServerId = requirement.ServerId
            });
            if (user.ServerProfile is not null)
            {
                context.Succeed(requirement);
            }
        }
    }
}
