using Application.Authentication.Requests.RegisterRequest;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IMediator _mediator;
        public AuthorizationController(IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
        {
            IdentityResult result = await _mediator.Send(registrationRequest);

            string returnUser = registrationRequest.ReturnUrl ?? "/";
            if (result.Succeeded)
            {
                return Created(returnUser, null);
            }
            else
            {
                return BadRequest(result.Errors.First().Description);
            }


        }
    }
}
