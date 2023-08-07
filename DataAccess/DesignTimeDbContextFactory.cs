using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using DataAccess;

public class DesignTimeDbContextFactory : /*IDesignTimeDbContextFactory<AppDbContext>*/ IDesignTimeDbContextFactory<AuthorizationDbContext>
{
    // public AppDbContext CreateDbContext(string[] args)
    // {
    //     var configurationPath = Path.GetFullPath(
    //         Path.Combine(
    //             Directory.GetCurrentDirectory(),
    //             @"..\Identity"
    //             )
    //         );  
    //     
    //     IConfiguration configuration = new ConfigurationBuilder()
    //         .SetBasePath(configurationPath)
    //         .AddJsonFile("appsettings.json", optional: false)
    //         .Build();
    //
    //     var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    //     optionsBuilder.UseSqlServer(configuration.GetConnectionString("Auth"));
    //
    //     return new AppDbContext(optionsBuilder.Options, configuration);
    // }
    public AuthorizationDbContext CreateDbContext(string[] args)
    {
        var configurationPath = Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\Identity"
            )
        );  
        
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(configurationPath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AuthorizationDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AuthorizationDbContextConnection"));

        return new AuthorizationDbContext(optionsBuilder.Options);
    }
}