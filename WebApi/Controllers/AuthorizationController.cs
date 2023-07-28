using Application.Authentication.Requests.LoginRequest;
using Application.Authentication.Requests.RegisterRequest;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
        {
            IdentityResult result = await _mediator.Send(registrationRequest);

            string returnUrl = registrationRequest.ReturnUrl ?? "/";
            if (result.Succeeded)
            {
                return Created(returnUrl, null);
            }
            else
            {
                return BadRequest(result.Errors.First().Description);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            SignInResult result = await _mediator.Send(loginRequest);

            string returnUrl = loginRequest.ReturnUrl ?? "/";
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return BadRequest("Login fail");
            }
        }
    }
}
