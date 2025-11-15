using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos;

/// <summary>
/// DTOs para endpoint de tarefas
/// </summary>
public class TaskItemDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateOnly Date { get; set; }
    public bool IsComplete { get; set; }
    public int? Priority { get; set; }
}

public class CreateTaskItemDto
{
    [Required]
    public string? Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public DateOnly Date { get; set; }
    public int? Priority { get; set; }
}

public class UpdateTaskItemDto
{
    [Required]
    public string? Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public DateOnly Date { get; set; }
    public bool IsComplete { get; set; }
    public int? Priority { get; set; }
}