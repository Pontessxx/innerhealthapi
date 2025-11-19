using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos
{
    /// <summary>
    /// Representa um registro de sono armazenado no sistema,
    /// incluindo a duração (em horas) e o índice de qualidade do sono.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "id": 3,
    ///   "date": "2025-01-10",
    ///   "hours": 7.5,
    ///   "quality": 85
    /// }
    /// ```
    /// </remarks>
    public class SleepRecordDto
    {
        /// <summary>
        /// Identificador único do registro de sono.
        /// </summary>
        /// <example>3</example>
        public int Id { get; set; }

        /// <summary>
        /// Data à qual o registro de sono se refere.
        /// </summary>
        /// <example>2025-01-10</example>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Número de horas dormidas.
        /// Aceita valores decimais (ex.: 7.5 horas).
        /// </summary>
        /// <example>7.5</example>
        public decimal Hours { get; set; }

        /// <summary>
        /// Índice de qualidade do sono, variando de 0 a 100.
        /// </summary>
        /// <example>85</example>
        public int Quality { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para criar um novo registro de sono.
    /// </summary>
    /// <remarks>
    /// A API atribui automaticamente o registro à data atual.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "hours": 6.8,
    ///   "quality": 70
    /// }
    /// ```
    /// </remarks>
    public class CreateSleepRecordDto
    {
        /// <summary>
        /// Horas dormidas (0 a 24).
        /// Aceita decimais para maior precisão.
        /// </summary>
        /// <example>6.8</example>
        [Range(0, 24, ErrorMessage = "As horas devem estar entre 0 e 24.")]
        public decimal Hours { get; set; }

        /// <summary>
        /// Índice de qualidade do sono (0 a 100).
        /// </summary>
        /// <example>70</example>
        [Range(0, 100, ErrorMessage = "A qualidade deve estar entre 0 e 100.")]
        public int Quality { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para atualizar um registro de sono existente.
    /// </summary>
    /// <remarks>
    /// Usado em endpoints PUT para modificar a duração ou qualidade do sono.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "hours": 8,
    ///   "quality": 90
    /// }
    /// ```
    /// </remarks>
    public class UpdateSleepRecordDto
    {
        /// <summary>
        /// Número atualizado de horas dormidas (0 a 24).
        /// </summary>
        /// <example>8</example>
        [Range(0, 24, ErrorMessage = "As horas devem estar entre 0 e 24.")]
        public decimal Hours { get; set; }

        /// <summary>
        /// Índice atualizado de qualidade do sono (0 a 100).
        /// </summary>
        /// <example>90</example>
        [Range(0, 100, ErrorMessage = "A qualidade deve estar entre 0 e 100.")]
        public int Quality { get; set; }
    }
}
