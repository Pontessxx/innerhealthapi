using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

// Operações relacionadas às tarefas do usuário.
public interface ITaskService
{
    // Retorna as tarefas de um dia específico.
    Task<IEnumerable<TaskItem>> GetTasksAsync(DateOnly date);

    // Retorna todas as tarefas cadastradas.
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();

    // Cria uma nova tarefa.
    Task<TaskItem> AddTaskAsync(string title, string? description, DateOnly date, int? priority);

    // Atualiza uma tarefa existente.
    Task<TaskItem?> UpdateTaskAsync(int id, string title, string? description, DateOnly date, bool isComplete, int? priority);

    // Remove uma tarefa pelo ID.
    Task<bool> DeleteTaskAsync(int id);
}