using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos
{
    /// <summary>
    /// Representa uma sessão de meditação armazenada no sistema.
    /// </summary>
    /// <remarks>
    /// Este DTO é retornado pelos endpoints de consulta (GET) e contém
    /// informações como data e duração da sessão.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "id": 1,
    ///   "date": "2025-01-10",
    ///   "minutes": 15
    /// }
    /// ```
    /// </remarks>
    public class MeditationSessionDto
    {
        /// <summary>
        /// Identificador único da sessão de meditação.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Data em que a sessão de meditação ocorreu.
        /// </summary>
        /// <example>2025-01-10</example>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Duração da sessão de meditação em minutos.
        /// </summary>
        /// <example>15</example>
        public int Minutes { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para criar uma nova sessão de meditação.
    /// </summary>
    /// <remarks>
    /// Apenas a quantidade de minutos precisa ser informada.
    /// A API automaticamente associará a sessão à data atual.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "minutes": 12
    /// }
    /// ```
    /// </remarks>
    public class CreateMeditationSessionDto
    {
        /// <summary>
        /// Duração da sessão de meditação em minutos.
        /// Deve ser maior que zero.
        /// </summary>
        /// <example>12</example>
        [Range(1, int.MaxValue, ErrorMessage = "A duração deve ser de pelo menos 1 minuto.")]
        public int Minutes { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para atualizar uma sessão de meditação existente.
    /// </summary>
    /// <remarks>
    /// O ID da sessão é informado na URL do endpoint.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "minutes": 20
    /// }
    /// ```
    /// </remarks>
    public class UpdateMeditationSessionDto
    {
        /// <summary>
        /// Nova duração da sessão de meditação em minutos.
        /// Deve ser no mínimo 1 minuto.
        /// </summary>
        /// <example>20</example>
        [Range(1, int.MaxValue, ErrorMessage = "A duração deve ser de pelo menos 1 minuto.")]
        public int Minutes { get; set; }
    }
}
