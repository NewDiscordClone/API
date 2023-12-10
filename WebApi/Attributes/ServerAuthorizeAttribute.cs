using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Domain;

namespace Sparkle.WebApi.Attributes
{
    public partial class ServerAuthorizeAttribute : ActionFilterAttribute
    {
        public string? Policy { get; set; }
        public string? Roles { get; set; }
        public string? Claims { get; set; }

        private static string GetServerId(ActionExecutingContext context)
        {
            InvalidOperationException exception = new("Server id not found in route");

            if (!context.RouteData.Values.TryGetValue("serverId", out object? serverId))
                throw exception;

            return serverId is string id ? id : throw exception;
        }

        private static string? GetChatId(ActionExecutingContext context)
            => context.RouteData.Values.GetValueOrDefault("chatId")?.ToString();

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IAuthorizationService authorizationService = context.HttpContext.RequestServices.GetService<IAuthorizationService>()
                ?? throw new InvalidOperationException();

            UserProfile? profile;

            string? id = GetChatId(context);
            if (id is not null)
            {
                profile = await GetProfileFromChat(id, context);
            }
            else
            {
                id = GetServerId(context);
                profile = await GetProfileFromServer(id, context);
            }

            if (profile is null)
            {
                context.Result = new ForbidResult();
                return;
            }

            string authParams = "";

            if (!string.IsNullOrEmpty(Claims))
            {
                authParams += $"Claims:{Claims}|";
            }
            if (!string.IsNullOrEmpty(Roles))
            {
                authParams += $"Roles:{Roles}|";
            }
            if (!string.IsNullOrEmpty(Policy))
            {
                authParams += $"Policy:{Policy}|";
            }

            if (string.IsNullOrEmpty(authParams) && (context.HttpContext.User.Identity?.IsAuthenticated ?? false))
            {
                await next();
                return;
            }

            authParams += $"[profileId:{profile.Id}]";
            AuthorizationResult result = await authorizationService
                .AuthorizeAsync(context.HttpContext.User, profile.Id, authParams);

            if (result.Succeeded)
            {
                await next();
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }

        private static async Task<UserProfile?> GetProfileFromServer(string serverId, ActionExecutingContext context)
        {
            IAuthorizedUserProvider userProvider = context.HttpContext.RequestServices.GetService<IAuthorizedUserProvider>()
                ?? throw new InvalidOperationException();

            IServerProfileRepository repository = context.HttpContext.RequestServices.GetService<IServerProfileRepository>()
                ?? throw new InvalidOperationException();

            UserProfile? profile = await repository.FindUserProfileOnServerAsync(serverId, userProvider.GetUserId());
            return profile;
        }

        private static async Task<UserProfile?> GetProfileFromChat(string chatId, ActionExecutingContext context)
        {
            IAuthorizedUserProvider userProvider = context.HttpContext
                .RequestServices.GetService<IAuthorizedUserProvider>()
                ?? throw new InvalidOperationException();

            IUserProfileRepository userRepository = context.HttpContext
                .RequestServices.GetService<IUserProfileRepository>()
                ?? throw new InvalidOperationException();

            IServerProfileRepository serverRepository = context.HttpContext
                .RequestServices.GetService<IServerProfileRepository>()
                ?? throw new InvalidOperationException();

            UserProfile? profile = await userRepository
                .FindOrDefaultByChatIdAndUserIdAsync(chatId, userProvider.GetUserId(), false);

            profile ??= await serverRepository.FindProfileByChannelIdAsync(chatId, userProvider.GetUserId());

            return profile;
        }
    }
}
