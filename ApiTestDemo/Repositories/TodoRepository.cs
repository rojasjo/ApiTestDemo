using ApiTestDemo.Data;
using ApiTestDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTestDemo.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoDbContext _todoDbContext;

    public TodoRepository(TodoDbContext todoDbContext)
    {
        _todoDbContext = todoDbContext;
    }

    public async Task<Todo> CreateAsync(string title, string description)
    {
        var todo = new Todo { Title = title, Description = description, IsCompleted = false };

        _todoDbContext.Todos.Add(todo);
        await _todoDbContext.SaveChangesAsync();

        return todo;
    }

    public Task<Todo?> GetByIdAsync(long id)
    {
        return _todoDbContext.Todos.FirstOrDefaultAsync(t => t.Id == id);
    }
}