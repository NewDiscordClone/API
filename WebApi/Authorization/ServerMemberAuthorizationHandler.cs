using Application.Providers;
using Application.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Authorization
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
            int userId = _userProvider.GetUserId();
            GetUserDetailsDto user = await _mediator.Send(new GetUserDetailsRequest
            {
                UserId = userId,
                ServerId = requirement.ServerId
            });
            if (user.Profile is not null)
            {
                context.Succeed(requirement);
            }
        }
    }
}
