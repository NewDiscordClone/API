using Identity.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Identity.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public IActionResult Register(string? returnUrl = null)
        {
            return View(new RegistrationRequest { ReturnUrl = returnUrl });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
        {
            if (!ModelState.IsValid)
                return View(registrationRequest);

            IdentityResult result = await _mediator
                .Send(registrationRequest);

            string returnUrl = registrationRequest.ReturnUrl
                ?? "/";
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty,
                    result.Errors.First().Description);
                return View(registrationRequest);
            }
        }
        [HttpGet("[action]")]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginRequest { ReturnUrl = returnUrl });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return View(loginRequest);

            SignInResult result = await _mediator.Send(loginRequest);

            string returnUrl = loginRequest.ReturnUrl ?? "/";
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty,
                    "Login fail");
                return View(loginRequest);
            }
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> Logout([FromQuery] string logoutId)
        {
            string? returnUrl = await _mediator
                .Send(new LogoutRequest { LogoutId = logoutId });
            returnUrl ??= "http://localhost:3000";

            return Redirect(returnUrl);
        }
    }
}
