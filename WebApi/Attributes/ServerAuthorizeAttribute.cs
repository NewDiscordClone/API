using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.WebApi.Attributes
{
    public partial class ServerAuthorizeAttribute : ActionFilterAttribute
    {
        public string? Policy { get; set; }


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
                context.Result = new NotFoundResult();
                return;
            }

            if (Policy is null)
            {
                throw new Exception();
            }

            AuthorizationResult result = await authorizationService
                .AuthorizeAsync(context.HttpContext.User, profile.Id, Policy);

            if (result.Succeeded)
            {
                await next();
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }

        private static async Task<UserProfile?> GetProfileFromServer(string id, ActionExecutingContext context)
        {
            IAppDbContext dbContext = context.HttpContext.RequestServices.GetService<IAppDbContext>()
                ?? throw new InvalidOperationException();

            IAuthorizedUserProvider userProvider = context.HttpContext.RequestServices.GetService<IAuthorizedUserProvider>()
                ?? throw new InvalidOperationException();

            Server? server = await dbContext.Servers.FindOrDefaultAsync(id);
            UserProfile? profile = server?.Profiles.Single(serverProfile => serverProfile.UserId == userProvider.GetUserId());
            return profile;
        }

        private static async Task<UserProfile?> GetProfileFromChat(string id, ActionExecutingContext context)
        {
            IAppDbContext dbContext = context.HttpContext.RequestServices.GetService<IAppDbContext>()
                ?? throw new InvalidOperationException();

            IAuthorizedUserProvider userProvider = context.HttpContext.RequestServices.GetService<IAuthorizedUserProvider>()
                ?? throw new InvalidOperationException();

            Chat? chat = await dbContext.Chats.FindOrDefaultAsync(id);
            UserProfile? profile = chat?.Profiles.SingleOrDefault(chatProfile => chatProfile.UserId == userProvider.GetUserId());
            return profile;
        }
    }
}
