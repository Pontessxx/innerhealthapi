using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos
{
    /// <summary>
    /// Representa um registro de atividade física armazenado no sistema.
    /// </summary>
    /// <remarks>
    /// Este DTO é retornado pelos endpoints de consulta (GET) e contém
    /// informações sobre a atividade realizada, sua duração e a data.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "id": 1,
    ///   "date": "2025-01-10",
    ///   "modality": "Running",
    ///   "durationMinutes": 30
    /// }
    /// ```
    /// </remarks>
    public class PhysicalActivityDto
    {
        /// <summary>
        /// Identificador único da atividade física.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Data em que a atividade foi realizada.
        /// </summary>
        /// <example>2025-01-10</example>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Modalidade da atividade realizada (ex.: Corrida, Ciclismo, Academia).
        /// </summary>
        /// <example>Running</example>
        public string? Modality { get; set; }

        /// <summary>
        /// Duração da atividade em minutos.
        /// </summary>
        /// <example>30</example>
        public int DurationMinutes { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para criar um novo registro de atividade física.
    /// </summary>
    /// <remarks>
    /// Tanto a modalidade quanto a duração são obrigatórias.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "modality": "Cycling",
    ///   "durationMinutes": 45
    /// }
    /// ```
    /// </remarks>
    public class CreatePhysicalActivityDto
    {
        /// <summary>
        /// Modalidade da atividade (ex.: Running, Cycling, Gym).
        /// </summary>
        /// <example>Cycling</example>
        [Required(ErrorMessage = "A modalidade é obrigatória.")]
        public string? Modality { get; set; }

        /// <summary>
        /// Duração da atividade em minutos.
        /// Deve ser maior que zero.
        /// </summary>
        /// <example>45</example>
        [Range(1, int.MaxValue, ErrorMessage = "A duração deve ser de pelo menos 1 minuto.")]
        public int DurationMinutes { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para atualizar um registro existente de atividade física.
    /// </summary>
    /// <remarks>
    /// O ID da atividade é informado na URL da requisição.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "modality": "Walking",
    ///   "durationMinutes": 20
    /// }
    /// ```
    /// </remarks>
    public class UpdatePhysicalActivityDto
    {
        /// <summary>
        /// Nova modalidade da atividade.
        /// </summary>
        /// <example>Walking</example>
        [Required(ErrorMessage = "A modalidade é obrigatória.")]
        public string? Modality { get; set; }

        /// <summary>
        /// Nova duração da atividade em minutos.
        /// Deve ser no mínimo 1 minuto.
        /// </summary>
        /// <example>20</example>
        [Range(1, int.MaxValue, ErrorMessage = "A duração deve ser de pelo menos 1 minuto.")]
        public int DurationMinutes { get; set; }
    }
}
