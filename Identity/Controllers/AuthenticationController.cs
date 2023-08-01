using Application.Authentication.Requests.LoginRequest;
using Application.Authentication.Requests.RegisterRequest;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;
        private SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthenticationController(IMediator mediator, SignInManager<IdentityUser> signInManager, IIdentityServerInteractionService interactionService)
        {
            _mediator = mediator;
            _signInManager = signInManager;
            _interactionService = interactionService;
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
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            IdentityServer4.Models.LogoutRequest logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            return Redirect("/");
        }
    }
}
