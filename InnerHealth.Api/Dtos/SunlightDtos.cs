using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos;

/// <summary>
/// DTOs para endpoint de exposição ao sol.
/// </summary>
public class SunlightSessionDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int Minutes { get; set; }
}

public class CreateSunlightSessionDto
{
    [Range(1, int.MaxValue)]
    public int Minutes { get; set; }
}

public class UpdateSunlightSessionDto
{
    [Range(1, int.MaxValue)]
    public int Minutes { get; set; }
}