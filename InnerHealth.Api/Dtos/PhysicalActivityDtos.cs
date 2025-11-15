using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos;

/// <summary>
/// DTOs para endpoint de atividade fisica
/// </summary>
public class PhysicalActivityDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string? Modality { get; set; }
    public int DurationMinutes { get; set; }
}

public class CreatePhysicalActivityDto
{
    [Required]
    public string? Modality { get; set; }
    [Range(1, int.MaxValue)]
    public int DurationMinutes { get; set; }
}

public class UpdatePhysicalActivityDto
{
    [Required]
    public string? Modality { get; set; }
    [Range(1, int.MaxValue)]
    public int DurationMinutes { get; set; }
}