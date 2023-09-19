using ApiTestDemo.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ApiTestDemo.IntegrationTests.TestSuite;

public static class TodoDbContextFactory
{
    public static TodoDbContext CreateWithSqlLite()
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "sql_lite_tests" };
        var connectionString = connectionStringBuilder.ToString();

        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseSqlite(connectionString)
            .Options;
        
        var applicationDbContext = new TodoDbContext(options);
        applicationDbContext.Database.EnsureCreated();

        return applicationDbContext;
    }
}