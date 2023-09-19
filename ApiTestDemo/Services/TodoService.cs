using System.ComponentModel.DataAnnotations;
using ApiTestDemo.Dto;
using ApiTestDemo.Repositories;

namespace ApiTestDemo.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<TodoDto> AddAsync(TodoForCreationDto todoForCreationDto)
    {
        if (string.IsNullOrWhiteSpace(todoForCreationDto.Title) ||
            string.IsNullOrWhiteSpace(todoForCreationDto.Description))
        {
            throw new ValidationException("Title and Description cannot be empty.");
        }

        var todo = await _todoRepository.CreateAsync(todoForCreationDto.Title, todoForCreationDto.Description);
        return new TodoDto
            { Id = todo.Id, Title = todo.Title, Description = todo.Description, IsCompleted = todo.IsCompleted };
    }

    public async Task<TodoDto?> GetByIdAsync(long id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);

        if (todo is null)
        {
            return null;
        }

        return new TodoDto
            { Id = todo.Id, Title = todo.Title, Description = todo.Description, IsCompleted = todo.IsCompleted };
    }
}