using ApiTestDemo.Dto;
using ApiTestDemo.Models;

namespace ApiTestDemo.Repositories;

public interface ITodoRepository
{
    Task<Todo> CreateAsync(string title, string description);
    
    Task<Todo?> GetByIdAsync(long id);
}