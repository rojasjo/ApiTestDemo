using System.Net;
using System.Net.Http.Json;
using ApiTestDemo.Dto;
using ApiTestDemo.IntegrationTests.TestSuite;

namespace ApiTestDemo.IntegrationTests.Api;

[TestFixture]
public class TodoControllerTests : ApiIntegrationTestFixture
{
    [Test]
    public async Task Post_ValidTodo_Returns201Created()
    {
        var todo = new TodoForCreationDto { Title = "Title", Description = "Description" };

        var httpResponseMessage = await HttpClient.PostAsJsonAsync("api/todos", todo);

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task Post_ValidTodo_ReturnsExpectedDto()
    {
        var todo = new TodoForCreationDto { Title = "Title", Description = "Description" };

        var httpResponseMessage = await HttpClient.PostAsJsonAsync("api/todos", todo);
        var todoFromResponse = await httpResponseMessage.DeserializeAsync<TodoDto>();

        Assert.Multiple(() =>
        {
            Assert.That(todoFromResponse, Is.Not.Null);
            Assert.That(todoFromResponse!.Id, Is.EqualTo(1));
            Assert.That(todoFromResponse.Title, Is.EqualTo("Title"));
            Assert.That(todoFromResponse.Description, Is.EqualTo("Description"));
            Assert.That(todoFromResponse.IsCompleted, Is.EqualTo(false));
        });
    }

    [Test]
    public async Task Post_ValidTodo_ReturnsExpectedLocationHeader()
    {
        var todo = new TodoForCreationDto { Title = "Title", Description = "Description" };

        var httpResponseMessage = await HttpClient.PostAsJsonAsync("api/todos", todo);

        Assert.That(httpResponseMessage.Headers.Location, Is.EqualTo(new Uri("https://localhost/api/todos/1")));
    }

    [Test]
    public async Task Get_ExistingTodo_Returns200Ok()
    {
        var todo = new TodoForCreationDto { Title = "Title", Description = "Description" };
        await HttpClient.PostAsJsonAsync("api/todos", todo);

        var httpResponseMessage = await HttpClient.GetAsync("api/todos/1");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Get_NonExistingTodo_Returns404NotFound()
    {
        var httpResponseMessage = await HttpClient.GetAsync("api/todos/1");

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}