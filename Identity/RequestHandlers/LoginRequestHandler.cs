using Application.Models;
using Identity.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.RequestHandlers
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, SignInResult>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginRequestHandler(SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByEmailAsync(request.Email);
            string userName = user?.UserName ?? string.Empty;

            return await _signInManager
                .PasswordSignInAsync(userName, request.Password, request.RememberMe, true);
        }
    }
}
