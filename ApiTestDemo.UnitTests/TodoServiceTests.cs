using System.ComponentModel.DataAnnotations;
using ApiTestDemo.Dto;
using ApiTestDemo.Models;
using ApiTestDemo.Repositories;
using ApiTestDemo.Services;
using Moq;

namespace ApiTestDemo.UnitTests;

[TestFixture]
public class TodoServiceTests
{
    private Mock<ITodoRepository> _todoRepository = null!;
    private TodoService _todoService = null!;

    [SetUp]
    public void Setup()
    {
        _todoRepository = new Mock<ITodoRepository>();
        _todoService = new TodoService(_todoRepository.Object);
    }
    
    [Test]
    public async Task AddAsync_WithValidTodo_ReturnsTodo()
    {
        var todo = new Todo { Id = 1, Title = "Title", Description = "Description", IsCompleted = false };
        _todoRepository.Setup(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(todo);
        
        var todoDto = await _todoService.AddAsync(new TodoForCreationDto { Title = "Title", Description = "Description" });
        
        Assert.Multiple(() =>
        {
            Assert.That(todoDto, Is.Not.Null);
            Assert.That(todoDto.Id, Is.EqualTo(1));
            Assert.That(todoDto.Title, Is.EqualTo("Title"));
            Assert.That(todoDto.Description, Is.EqualTo("Description"));
            Assert.That(todoDto.IsCompleted, Is.EqualTo(false));
        });
    }
    
    [Test]
    [TestCase("Title", null)]
    [TestCase(null, "Description")]
    [TestCase(null, null)]
    public void AddAsync_WithInvalidTodo_ThrowsException(string title, string description)
    {
        Assert.ThrowsAsync<ValidationException>(async () => await _todoService.AddAsync(new TodoForCreationDto { Title = title, Description = description}));
    }
    
    [Test]
    public async Task GetByIdAsync_WithValidId_ReturnsTodo()
    {
        var todo = new Todo { Id = 1, Title = "Title", Description = "Description", IsCompleted = false };
        _todoRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(todo);
        
        var todoDto = await _todoService.GetByIdAsync(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(todoDto, Is.Not.Null);
            Assert.That(todoDto!.Id, Is.EqualTo(1));
            Assert.That(todoDto.Title, Is.EqualTo("Title"));
            Assert.That(todoDto.Description, Is.EqualTo("Description"));
            Assert.That(todoDto.IsCompleted, Is.EqualTo(false));
        });
    }
    
    [Test]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        _todoRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((Todo?)null);
        
        var todoDto = await _todoService.GetByIdAsync(1);
        
        Assert.That(todoDto, Is.Null);
    }
}