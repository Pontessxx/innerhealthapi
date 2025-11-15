namespace InnerHealth.Api.Models;

// Representa uma tarefa que o usuário precisa fazer.
public class TaskItem
{
    public int Id { get; set; }

    // Título da tarefa.
    public string? Title { get; set; }

    // Explicação mais detalhada do que precisa ser feito.
    public string? Description { get; set; }

    // Data da tarefa (só o dia mesmo).
    public DateOnly Date { get; set; }

    // Marca se a tarefa já foi concluída.
    public bool IsComplete { get; set; }

    // Prioridade opcional: 0 = baixa, 1 = média, 2 = alta.
    public int? Priority { get; set; }

    public int UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
}