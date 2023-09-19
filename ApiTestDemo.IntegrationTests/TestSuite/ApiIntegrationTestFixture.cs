using ApiTestDemo.Data;

namespace ApiTestDemo.IntegrationTests.TestSuite;

public abstract class ApiIntegrationTestFixture
{
    private WebAppFactory _factory = null!;
    private TodoDbContext _todoDbContext = null!;
    
    protected HttpClient HttpClient = null!;

    [SetUp]
    public virtual void Setup()
    {
        _factory = new WebAppFactory();
        _todoDbContext = _factory.CreateDbContext();
        HttpClient = HttpClientFactory.Create(_factory);
    }

    [TearDown]
    public virtual void TearDown()
    {
        _todoDbContext.Database.EnsureDeleted();
        _todoDbContext.Dispose();
        _factory.Dispose();
        HttpClient.Dispose();
    }
}