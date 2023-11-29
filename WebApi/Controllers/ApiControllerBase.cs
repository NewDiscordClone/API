using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly IMediator Mediator;
        private readonly IAuthorizedUserProvider _userProvider;
        protected Guid UserId => _userProvider.GetUserId();
        protected readonly IMapper Mapper;

        protected ApiControllerBase(IMediator mediator, IAuthorizedUserProvider userProvider)
        {
            Mediator = mediator;
            _userProvider = userProvider;
        }
        protected ApiControllerBase(IMediator mediator, IAuthorizedUserProvider userProvider, IMapper mapper)
        {
            Mediator = mediator;
            _userProvider = userProvider;
            Mapper = mapper;
        }

        protected ApiControllerBase(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}