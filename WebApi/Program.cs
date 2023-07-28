using Application;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

services.AddControllers();

services.AddDatabases(builder.Configuration);
services.AddApplication();

services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;

})
    .AddEntityFrameworkStores<AuthorizationDbContext>()
    .AddDefaultTokenProviders();

services.AddSwaggerGen(options =>
{
    string xmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlName);
    options.IncludeXmlComments(xmlPath);
});
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
        option.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();


app.Run();
