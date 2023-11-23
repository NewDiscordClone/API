using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Serilog;
using Sparkle.Application;
using Sparkle.Application.Common.Convertors;
using Sparkle.Application.Common.Interfaces;
using Sparkle.DataAccess;
using Sparkle.WebApi;
using Sparkle.WebApi.Attributes;
using Sparkle.WebApi.Authorization.Handlers;
using Sparkle.WebApi.Common.Mapping;
using Sparkle.WebApi.Common.Options;
using Sparkle.WebApi.Common.Parsers;
using Sparkle.WebApi.Hubs;
using Sparkle.WebApi.Providers;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi.Providers;
using ExceptionFilterAttribute = Sparkle.WebApi.Attributes.ExceptionFilterAttribute;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

builder.Logging.AddSerilog();

services.AddApplication();

services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilterAttribute>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new PrivateChatLookUpConverter());
});

services.AddDatabase(builder.Configuration);

services.AddMapping();

JwtOptions jwtOptions = new();
builder.Configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);
services.AddSingleton(jwtOptions);

services.AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = jwtOptions.Authority;
        options.Audience = jwtOptions.Audience;
        options.MetadataAddress = jwtOptions.MetadataAddress;
    });

services.AddAuthorization();

services.AddScoped<IAuthorizedUserProvider, AuthorizedUserProvider>();

services.AddSingleton<IAuthorizationParser, StringPolicyParser>();
services.AddScoped<IHubContextProvider, HubContextProvider>();

services.AddSingleton<IAuthorizationPolicyProvider, ServerAuthorizationPolicyProvider>();
services.AddScoped<IAuthorizationHandler, ServerMemberAuthorizationHandler>();
services.AddScoped<IAuthorizationHandler, ServerClaimsAuthorizationRequirementHandler>();
services.AddScoped<IAuthorizationHandler, ProfileRoleRequirementHandler>();
services.AddScoped<IActionFilter, ServerAuthorizeAttribute>();

services.AddHttpContextAccessor();
services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://sparkle.net.ua", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
     SwaggerConfigurationOptions>();
services.AddSwaggerGen();

builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
});

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("chat");

if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/sparkle/swagger.json", "WebApi");
        option.DisplayRequestDuration();
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.Run();

