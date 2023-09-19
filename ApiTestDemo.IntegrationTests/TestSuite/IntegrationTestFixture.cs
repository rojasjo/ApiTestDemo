using ApiTestDemo.Data;

namespace ApiTestDemo.IntegrationTests.TestSuite;

public abstract class IntegrationTestFixture
{
    protected TodoDbContext TodoDbContext { get; set; } = null!;

    [SetUp]
    public virtual void Setup()
    {
        TodoDbContext = TodoDbContextFactory.CreateWithSqlLite();
    }

    [TearDown]
    public virtual void TearDown()
    {
        TodoDbContext.Database.EnsureDeleted();
        TodoDbContext.Dispose();
    }
}