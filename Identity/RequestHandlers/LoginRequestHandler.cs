using Identity.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.RequestHandlers
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, SignInResult>
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginRequestHandler(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(request.Email);
            return await _signInManager
                .PasswordSignInAsync(user, request.Password, request.RememberMe, true);
        }
    }
}
