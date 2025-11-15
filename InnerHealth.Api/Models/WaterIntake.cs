namespace InnerHealth.Api.Models;

// Registro de água bebida em um dia (em ml).
public class WaterIntake
{
    public int Id { get; set; }

    // Data em que a pessoa bebeu água. Só a data importa.
    public DateOnly Date { get; set; }

    // Quantidade de água ingerida, em mililitros.
    public int AmountMl { get; set; }

    // Liga esse registro ao perfil do usuário.
    public int UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
}