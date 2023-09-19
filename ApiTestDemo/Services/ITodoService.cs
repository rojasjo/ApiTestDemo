using ApiTestDemo.Dto;

namespace ApiTestDemo.Services;

public interface ITodoService
{
    Task<TodoDto> AddAsync(TodoForCreationDto todoForCreationDto);
    
    Task<TodoDto?> GetByIdAsync(long id);
}