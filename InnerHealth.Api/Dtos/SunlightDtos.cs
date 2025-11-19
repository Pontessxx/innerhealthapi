using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos
{
    /// <summary>
    /// Representa uma sessão de exposição ao sol armazenada no sistema.
    /// </summary>
    /// <remarks>
    /// Utilizado nos endpoints GET para retornar os detalhes de cada sessão,
    /// incluindo a duração e a data de ocorrência.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "id": 4,
    ///   "date": "2025-01-10",
    ///   "minutes": 15
    /// }
    /// ```
    /// </remarks>
    public class SunlightSessionDto
    {
        /// <summary>
        /// Identificador único da sessão de exposição ao sol.
        /// </summary>
        /// <example>4</example>
        public int Id { get; set; }

        /// <summary>
        /// Data em que a exposição ao sol ocorreu.
        /// </summary>
        /// <example>2025-01-10</example>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Duração da exposição ao sol em minutos.
        /// </summary>
        /// <example>15</example>
        public int Minutes { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para criar uma nova sessão de exposição ao sol.
    /// </summary>
    /// <remarks>
    /// Apenas os minutos são necessários.
    /// A API automaticamente atribui a data atual.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "minutes": 12
    /// }
    /// ```
    /// </remarks>
    public class CreateSunlightSessionDto
    {
        /// <summary>
        /// Duração da exposição ao sol em minutos.
        /// Deve ser no mínimo 1 minuto.
        /// </summary>
        /// <example>12</example>
        [Range(1, int.MaxValue, ErrorMessage = "A duração deve ser de pelo menos 1 minuto.")]
        public int Minutes { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para atualizar uma sessão existente de exposição ao sol.
    /// </summary>
    /// <remarks>
    /// O ID da sessão é fornecido na URL da requisição.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "minutes": 20
    /// }
    /// ```
    /// </remarks>
    public class UpdateSunlightSessionDto
    {
        /// <summary>
        /// Nova duração da sessão de exposição ao sol em minutos.
        /// Deve ser maior que zero.
        /// </summary>
        /// <example>20</example>
        [Range(1, int.MaxValue, ErrorMessage = "A duração deve ser de pelo menos 1 minuto.")]
        public int Minutes { get; set; }
    }
}
