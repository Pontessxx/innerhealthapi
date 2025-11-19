using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos
{
    /// <summary>
    /// Representa uma tarefa armazenada no sistema,
    /// incluindo título, descrição, data, status e prioridade.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "id": 12,
    ///   "title": "Morning workout",
    ///   "description": "30 min cardio session",
    ///   "date": "2025-01-10",
    ///   "isComplete": false,
    ///   "priority": 2
    /// }
    /// ```
    /// </remarks>
    public class TaskItemDto
    {
        /// <summary>
        /// Identificador único da tarefa.
        /// </summary>
        /// <example>12</example>
        public int Id { get; set; }

        /// <summary>
        /// Título curto que descreve a tarefa.
        /// </summary>
        /// <example>Beber água</example>
        public string? Title { get; set; }

        /// <summary>
        /// Descrição opcional com mais detalhes sobre a tarefa.
        /// </summary>
        /// <example>Beber pelo menos 500ml de água pela manhã.</example>
        public string? Description { get; set; }

        /// <summary>
        /// Data associada à tarefa.
        /// </summary>
        /// <example>2025-01-10</example>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Indica se a tarefa foi concluída.
        /// </summary>
        /// <example>false</example>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Nível de prioridade da tarefa (0–5).
        /// Valores maiores representam maior prioridade.
        /// </summary>
        /// <example>2</example>
        public int? Priority { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para criar uma nova tarefa.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "title": "Estudar React Native",
    ///   "description": "Finalizar tela de login",
    ///   "date": "2025-01-10",
    ///   "priority": 3
    /// }
    /// ```
    /// </remarks>
    public class CreateTaskItemDto
    {
        /// <summary>
        /// Título da tarefa.
        /// Campo obrigatório.
        /// </summary>
        /// <example>Estudar React Native</example>
        [Required(ErrorMessage = "O título é obrigatório.")]
        public string? Title { get; set; }

        /// <summary>
        /// Descrição detalhada da tarefa (opcional).
        /// </summary>
        /// <example>Finalizar componentes de login e troca de tema.</example>
        public string? Description { get; set; }

        /// <summary>
        /// Data associada à tarefa.
        /// Campo obrigatório.
        /// </summary>
        /// <example>2025-01-10</example>
        [Required(ErrorMessage = "A data é obrigatória.")]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Prioridade opcional da tarefa (0–5).
        /// </summary>
        /// <example>3</example>
        public int? Priority { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para atualizar uma tarefa existente.
    /// </summary>
    /// <remarks>
    /// O ID da tarefa é passado na URL do endpoint PUT.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "title": "Estudar React Native",
    ///   "description": "Adicionar animações com Reanimated",
    ///   "date": "2025-01-10",
    ///   "isComplete": true,
    ///   "priority": 4
    /// }
    /// ```
    /// </remarks>
    public class UpdateTaskItemDto
    {
        /// <summary>
        /// Título atualizado da tarefa.
        /// Campo obrigatório.
        /// </summary>
        /// <example>Estudar React Native</example>
        [Required(ErrorMessage = "O título é obrigatório.")]
        public string? Title { get; set; }

        /// <summary>
        /// Descrição atualizada da tarefa (opcional).
        /// </summary>
        /// <example>Adicionar animações usando Reanimated 3.</example>
        public string? Description { get; set; }

        /// <summary>
        /// Data atualizada da tarefa.
        /// Campo obrigatório.
        /// </summary>
        /// <example>2025-01-10</example>
        [Required(ErrorMessage = "A data é obrigatória.")]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Indica se a tarefa está concluída.
        /// </summary>
        /// <example>true</example>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Prioridade atualizada da tarefa (0–5).
        /// </summary>
        /// <example>4</example>
        public int? Priority { get; set; }
    }
}
