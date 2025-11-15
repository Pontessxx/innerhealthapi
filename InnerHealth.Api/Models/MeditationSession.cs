namespace InnerHealth.Api.Models;

/// <summary>
///Representa uma entrada de sessão de meditação 
/// </summary>
public class MeditationSession
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    /// <summary>
    /// Duração da meditação em minutos
    /// </summary>
    public int Minutes { get; set; }

    public int UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
}