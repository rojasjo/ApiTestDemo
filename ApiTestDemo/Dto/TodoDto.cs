using System.ComponentModel.DataAnnotations;

namespace ApiTestDemo.Dto;

public class TodoDto
{
    [Required]
    public long Id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public required string Title { get; init; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(500)]
    public required string Description { get; init; }
    
    public bool IsCompleted { get; init; }
}