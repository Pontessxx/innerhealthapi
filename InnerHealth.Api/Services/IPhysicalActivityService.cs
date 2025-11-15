using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

// Operações relacionadas às atividades físicas.
public interface IPhysicalActivityService
{
    // Lista as atividades de um dia específico.
    Task<IEnumerable<PhysicalActivity>> GetActivitiesAsync(DateOnly date);

    // Retorna as atividades da semana inteira, começando pela data informada.
    Task<IDictionary<DateOnly, IEnumerable<PhysicalActivity>>> GetWeeklyActivitiesAsync(DateOnly weekStart);

    // Cadastra uma nova atividade (corrida, treino, etc.).
    Task<PhysicalActivity> AddActivityAsync(string? modality, int durationMinutes);

    // Atualiza uma atividade já registrada.
    Task<PhysicalActivity?> UpdateActivityAsync(int id, string? modality, int durationMinutes);

    // Remove uma atividade pelo ID.
    Task<bool> DeleteActivityAsync(int id);
}