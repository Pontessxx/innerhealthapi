namespace InnerHealth.Api.Models;

/// <summary>
/// Representa uma ssessão de atividade fisica
/// </summary>
public class PhysicalActivity
{
    public int Id { get; set; }
    /// <summary>
    /// data da sessão
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Modalidade da atividade fisica (musculação, corrida, yoga)
    /// </summary>
    public string? Modality { get; set; }

    /// <summary>
    ///Duração em minutos
    /// </summary>
    public int DurationMinutes { get; set; }

    public int UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
}