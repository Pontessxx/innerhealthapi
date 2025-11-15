namespace InnerHealth.Api.Models;

// Registro de quanto tempo a pessoa pegou sol em um dia.
public class SunlightSession
{
    public int Id { get; set; }

    // Data da exposição ao sol.
    public DateOnly Date { get; set; }

    // Quantos minutos a pessoa ficou no sol.
    public int Minutes { get; set; }

    public int UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
}