namespace ApiTestDemo.Models;

public class Todo
{
    public long Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public bool IsCompleted { get; set; }
}