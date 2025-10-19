using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Noted.Shared.Models;
using Noted.Application.Data;

namespace Noted.Application;

public class DbContextFactory : IDesignTimeDbContextFactory<NotedDbContext>
{
    public NotedDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Noted.Api"))
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("CONNECTION_STRING environment variable is not set");
        }

        var optionsBuilder = new DbContextOptionsBuilder<NotedDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new NotedDbContext(optionsBuilder.Options);
    }
}