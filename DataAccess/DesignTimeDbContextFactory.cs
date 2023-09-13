using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class DesignTimeDbContextFactory : /*IDesignTimeDbContextFactory<AppDbContext>*/ IDesignTimeDbContextFactory<AppDbContext>
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
    public AppDbContext CreateDbContext(string[] args)
    {
        string configurationPath = Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\WebApi"
                )
            );

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(configurationPath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
        string? connectionString = configuration.GetConnectionString("SqlServer");
        if (connectionString == null)
            throw new Exception("Connection string is null");
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options, configuration);
    }
}