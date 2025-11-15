using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services; 

// Operações relacionadas ao consumo de água.
public interface IWaterService
{
    // Lista todos os registros de água de um dia específico.
    Task<IEnumerable<WaterIntake>> GetIntakesAsync(DateOnly date);

    // Total de água bebida no dia (em ml).
    Task<int> GetDailyTotalAsync(DateOnly date);

    // Totais de água da semana inteira, começando pela data informada.
    Task<IDictionary<DateOnly, int>> GetWeeklyTotalsAsync(DateOnly weekStart);

    // Cálculo da quantidade diária recomendada (baseado no peso do usuário).
    Task<int> GetRecommendedDailyAmountAsync();

    // Adiciona um novo registro de água para hoje.
    Task<WaterIntake> AddIntakeAsync(int amountMl);

    // Atualiza um registro existente.
    Task<WaterIntake?> UpdateIntakeAsync(int id, int amountMl);

    // Remove um registro pelo ID.
    Task<bool> DeleteIntakeAsync(int id);
}