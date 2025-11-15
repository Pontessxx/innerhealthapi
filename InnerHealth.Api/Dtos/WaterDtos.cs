using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos;

/// <summary>
/// DTOs para o consumo de agua
/// </summary>
public class WaterIntakeDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int AmountMl { get; set; }
}

public class CreateWaterIntakeDto
{
    /// <summary>
    /// Quantidade de agua consumida em Ml
    /// </summary>
    [Range(1, int.MaxValue)]
    public int AmountMl { get; set; }
}

public class UpdateWaterIntakeDto
{
    /// <summary>
    /// Atualizar quantidade de água consumida (mL)
    /// </summary>
    [Range(1, int.MaxValue)]
    public int AmountMl { get; set; }
}