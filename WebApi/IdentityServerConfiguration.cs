using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebApi
{
    public static class IdentityServerConfiguration
    {
        /// <summary>
        /// Adds and configures IdentityServer
        /// </summary>
        public static IServiceCollection AddIdentityServer4(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityResources)
                .AddInMemoryApiResources(ApiResources)
                .AddInMemoryClients(Clients)
                .AddTestUsers(TestUsers)
                .AddAspNetIdentity<IdentityUser>();

            return services;
        }

        private static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles","User roles", new[]{"role"})
            };

        private static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("messageApi", "Your API")
                {
                    Scopes = { "messageApi.read", "messageApi.write" }
                }
            };

        private static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "react-client",
                    ClientSecrets = { new Secret("react-client-super-secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "http://localhost:3000/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:3000/signout-callback-oidc" },
                    AllowedCorsOrigins  = { "http://localhost:3000" },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "messageApi.read",
                        "messageApi.write"
                    },
                    RequireConsent = false
                }
            };

        private static List<TestUser> TestUsers =>
            new()
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "testuser",
                    Password = "testuser",
                    Claims = new List<Claim>
                    {
                        new Claim("role", "admin"), // Роль пользователя
                        new Claim("given_name", "Test"),
                        new Claim("family_name", "User")
                    }
                }
            };
    }
}
