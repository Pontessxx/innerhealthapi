using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos;

/// <summary>
/// DTOs para endpoint do perfil de usuario
/// </summary>
public class UserProfileDto
{
    public int Id { get; set; }
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
    public int Age { get; set; }
    public int SleepQuality { get; set; }
    public decimal SleepHours { get; set; }
}

public class UpdateUserProfileDto
{
    [Range(1, 1000)]
    public decimal Weight { get; set; }
    [Range(1, 300)]
    public decimal Height { get; set; }
    [Range(1, 120)]
    public int Age { get; set; }
    [Range(0, 100)]
    public int SleepQuality { get; set; }
    [Range(0, 24)]
    public decimal SleepHours { get; set; }
}