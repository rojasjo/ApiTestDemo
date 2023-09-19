using ApiTestDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTestDemo.Data;

public class TodoDbContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }
    
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);
    }
}