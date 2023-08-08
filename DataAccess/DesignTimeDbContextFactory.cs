using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
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
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServer"));

        return new AppDbContext(optionsBuilder.Options);
    }
}