using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

// Operações relacionadas às sessões de meditação.
public interface IMeditationService
{
    // Retorna as sessões de meditação de um dia específico.
    Task<IEnumerable<MeditationSession>> GetSessionsAsync(DateOnly date);

    // Soma total de minutos meditados no dia.
    Task<int> GetDailyTotalAsync(DateOnly date);

    // Totais da semana inteira, começando pela data informada.
    Task<IDictionary<DateOnly, int>> GetWeeklyTotalsAsync(DateOnly weekStart);

    // Recomendação diária de minutos de meditação.
    int GetRecommendedDailyMinutes();

    // Adiciona uma nova sessão.
    Task<MeditationSession> AddSessionAsync(int minutes);

    // Atualiza uma sessão existente.
    Task<MeditationSession?> UpdateSessionAsync(int id, int minutes);

    // Remove uma sessão pelo ID.
    Task<bool> DeleteSessionAsync(int id);
}