using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.WebApi.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly IMediator Mediator;
        private readonly IAuthorizedUserProvider _userProvider;
        protected Guid UserId => _userProvider.GetUserId();
        //int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        protected ApiControllerBase(IMediator mediator, IAuthorizedUserProvider userProvider)
        {
            Mediator = mediator;
            _userProvider = userProvider;
        }
    }
}