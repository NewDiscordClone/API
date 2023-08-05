using Identity.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.RequestHandlers
{
    public class RegistrationRequestHandler : IRequestHandler<RegistrationRequest, IdentityResult>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegistrationRequestHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(RegistrationRequest request, CancellationToken cancellationToken)
        {
            IdentityUser user = new()
            {
                UserName = request.Username,
                Email = request.Email
            };
            return await _userManager.CreateAsync(user, request.Password);
        }
    }
}
