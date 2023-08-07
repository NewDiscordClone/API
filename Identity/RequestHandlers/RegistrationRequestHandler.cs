using Application.Models;
using Identity.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.RequestHandlers
{
    public class RegistrationRequestHandler : IRequestHandler<RegistrationRequest, IdentityResult>
    {
        private readonly UserManager<User> _userManager;

        public RegistrationRequestHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(RegistrationRequest request, CancellationToken cancellationToken)
        {
            User user = new()
            {
                UserName = request.Username,
                Email = request.Email
            };
            return await _userManager.CreateAsync(user, request.Password);
        }
    }
}
