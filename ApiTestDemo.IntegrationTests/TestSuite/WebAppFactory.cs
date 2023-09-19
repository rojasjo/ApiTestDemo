using ApiTestDemo.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiTestDemo.IntegrationTests.TestSuite;

public class WebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(ReplaceDbContextWithInMemoryDb);
    }

    /// <summary>
    /// It is necessary to replace the default DbContext with an in-memory database
    /// </summary>
    /// <param name="services"></param>
    private static void ReplaceDbContextWithInMemoryDb(IServiceCollection services)
    {
        var existingDbContextRegistration = services.SingleOrDefault(
            d => d.ServiceType == typeof(DbContextOptions<TodoDbContext>)
        );

        if (existingDbContextRegistration != null)
        {
            services.Remove(existingDbContextRegistration);
        }

        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "sql_lite_tests" };
        var connectionString = connectionStringBuilder.ToString();
        services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlite(connectionString));
    }

    public TodoDbContext CreateDbContext()
    {
        var dbContext = Services.CreateScope().ServiceProvider.GetService<TodoDbContext>()!;
        dbContext.Database.EnsureCreated();
        
        return dbContext;
    }
}