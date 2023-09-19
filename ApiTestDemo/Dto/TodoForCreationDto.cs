using System.ComponentModel.DataAnnotations;

namespace ApiTestDemo.Dto;

public class TodoForCreationDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public required string Title { get; init; }

    [Required]
    [MinLength(3)]
    [MaxLength(500)]
    public required string Description { get; init; }
}