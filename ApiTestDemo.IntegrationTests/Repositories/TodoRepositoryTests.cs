using ApiTestDemo.IntegrationTests.TestSuite;
using ApiTestDemo.Models;
using ApiTestDemo.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiTestDemo.IntegrationTests.Repositories;

[TestFixture]
public class TodoRepositoryTests : IntegrationTestFixture
{
    private TodoRepository _todoRepository = null!;

    public override void Setup()
    {
        base.Setup();
        _todoRepository = new TodoRepository(TodoDbContext);
    }

    [Test]
    public async Task AddASync_DbIsEmpty_TodoSavedCorrectly()
    {
        await _todoRepository.CreateAsync("Title", "Description");

        var todoFromDb = await TodoDbContext.Todos.FirstOrDefaultAsync();

        Assert.Multiple(() =>
        {
            Assert.That(todoFromDb, Is.Not.Null);
            Assert.That(todoFromDb!.Id, Is.EqualTo(1));
            Assert.That(todoFromDb.Title, Is.EqualTo("Title"));
            Assert.That(todoFromDb.Description, Is.EqualTo("Description"));
            Assert.That(todoFromDb.IsCompleted, Is.EqualTo(false));
        });
    }

    [Test]
    public async Task GetByIdAsync_EmptyDatabase_ReturnsNull()
    {
        var todoDto = await _todoRepository.GetByIdAsync(1);

        Assert.That(todoDto, Is.Null);
    }
    
    [Test]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        await CreateTodo();
        
        var todoFromRepo = await _todoRepository.GetByIdAsync(100);

        Assert.That(todoFromRepo, Is.Null);
    }
    
    [Test]
    public async Task GetByIdAsync_ValidId_ReturnsNull()
    {
        var todo = await CreateTodo();

        var todoFromRepo = await _todoRepository.GetByIdAsync(1);

        Assert.That(todoFromRepo, Is.EqualTo(todo));
    }

    private async Task<Todo> CreateTodo()
    {
        var todo = new Todo { Title = "Title", Description = "Description", IsCompleted = true };
        await TodoDbContext.Todos.AddAsync(todo);
        await TodoDbContext.SaveChangesAsync();
        return todo;
    }
}