namespace InnerHealth.Api.Models;

// Perfil do usuário, com dados básicos usados pra gerar recomendações.
// Mesmo tendo só um usuário no app, o modelo já suporta vários.
public class UserProfile
{
    public int Id { get; set; }

    // Peso em kg — usado pra calcular quanto de água a pessoa deve beber no dia.
    public decimal Weight { get; set; }

    // Altura em cm. Guardado pra usos futuros.
    public decimal Height { get; set; }

    // Idade do usuário.
    public int Age { get; set; }

    // Nota de qualidade do sono (0 a 100) do dia atual. Zera todo dia.
    public int SleepQuality { get; set; }

    // Horas dormidas no dia. Também zera diariamente.
    public decimal SleepHours { get; set; }

    // Relacionamentos com os outros registros do sistema
    public ICollection<WaterIntake>? WaterIntakes { get; set; }
    public ICollection<SunlightSession>? SunlightSessions { get; set; }
    public ICollection<MeditationSession>? MeditationSessions { get; set; }
    public ICollection<SleepRecord>? SleepRecords { get; set; }
    public ICollection<PhysicalActivity>? PhysicalActivities { get; set; }
    public ICollection<TaskItem>? TaskItems { get; set; }
}