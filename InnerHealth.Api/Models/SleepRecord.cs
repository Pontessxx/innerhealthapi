namespace InnerHealth.Api.Models;

// Registro de como a pessoa dormiu em uma certa data.
public class SleepRecord
{
    public int Id { get; set; }

    // Dia do registro. Representa a noite que veio antes.
    public DateOnly Date { get; set; }

    // Total de horas dormidas.
    public decimal Hours { get; set; }

    // Nota de qualidade do sono (0 a 100).
    public int Quality { get; set; }

    public int UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
}