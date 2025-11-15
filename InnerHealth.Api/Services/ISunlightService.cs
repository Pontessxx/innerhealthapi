using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

// Operações relacionadas às sessões de exposição ao sol.
public interface ISunlightService
{
    // Lista as sessões de sol de um dia específico.
    Task<IEnumerable<SunlightSession>> GetSessionsAsync(DateOnly date);

    // Soma total de minutos de sol no dia.
    Task<int> GetDailyTotalAsync(DateOnly date);

    // Totais de sol da semana inteira, começando pela data informada.
    Task<IDictionary<DateOnly, int>> GetWeeklyTotalsAsync(DateOnly weekStart);

    // Tempo de sol recomendado por dia (em minutos).
    int GetRecommendedDailyMinutes();

    // Adiciona uma nova sessão de sol.
    Task<SunlightSession> AddSessionAsync(int minutes);

    // Atualiza uma sessão existente.
    Task<SunlightSession?> UpdateSessionAsync(int id, int minutes);

    // Remove uma sessão pelo ID.
    Task<bool> DeleteSessionAsync(int id);
}