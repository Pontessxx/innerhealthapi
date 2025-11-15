using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos;

/// <summary>
/// DTOs para endpoint do sono
/// </summary>
public class SleepRecordDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public decimal Hours { get; set; }
    public int Quality { get; set; }
}

public class CreateSleepRecordDto
{
    [Range(0, 24)]
    public decimal Hours { get; set; }
    [Range(0, 100)]
    public int Quality { get; set; }
}

public class UpdateSleepRecordDto
{
    [Range(0, 24)]
    public decimal Hours { get; set; }
    [Range(0, 100)]
    public int Quality { get; set; }
}