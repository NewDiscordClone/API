using Application.Models;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using AspNetCore.Identity.Mongo;
using MongoDB.Bson;

namespace Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            IServiceCollection services = builder.Services;

            services.AddDatabase(builder.Configuration);
            services.AddControllers();
            services.AddMvc(options => { options.EnableEndpointRouting = false; });

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly
                    .GetExecutingAssembly());
            });

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddIdentityMongoDbProvider<User, Role, ObjectId>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Lockout.MaxFailedAccessAttempts = 5;

                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;

                    options.User.RequireUniqueEmail = true;
                }, mongo =>
                {
                    mongo.ConnectionString = configuration.GetConnectionString("IdentityMongoDb");
                    mongo.UsersCollection = "users";
                    mongo.RolesCollection = "roles";
                })
                //.AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer4WithConfiguration()
                .AddDeveloperSigningCredential();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Spark.Identity.Cookie";
                config.LoginPath = "/Authentication/Login";
                config.LogoutPath = "/Authentication/Logout";
            });

            WebApplication app = builder.Build();

            app.MapControllers();
            app.UseMvc();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, "wwwroot")),
                RequestPath = "/wwwroot"
            });

            app.UseIdentityServer();

            app.Run();
        }
    }
}