﻿using Identity.Requests;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.RequestHandlers
{
    public class LogoutRequestHandler : IRequestHandler<LogoutRequest, string?>
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public LogoutRequestHandler(IIdentityServerInteractionService interactionService,
            SignInManager<IdentityUser> signInManager)
        {
            _interactionService = interactionService;
            _signInManager = signInManager;
        }

        public async Task<string?> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            IdentityServer4.Models.LogoutRequest logoutRequest =
                await _interactionService.GetLogoutContextAsync(request.LogoutId);
            return logoutRequest.PostLogoutRedirectUri;
        }
    }
}