using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos;

/// <summary>
/// DTOs para o endpoint de meditação
/// </summary>
public class MeditationSessionDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int Minutes { get; set; }
}

public class CreateMeditationSessionDto
{
    [Range(1, int.MaxValue)]
    public int Minutes { get; set; }
}

public class UpdateMeditationSessionDto
{
    [Range(1, int.MaxValue)]
    public int Minutes { get; set; }
}