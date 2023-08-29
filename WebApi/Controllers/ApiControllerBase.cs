using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly IMediator Mediator;
        private readonly IAuthorizedUserProvider _userProvider;
        protected int UserId => _userProvider.GetUserId();
        //int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        protected ApiControllerBase(IMediator mediator, IAuthorizedUserProvider userProvider)
        {
            Mediator = mediator;
            _userProvider = userProvider;
        }
    }
}