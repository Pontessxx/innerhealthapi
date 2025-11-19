using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos
{
    /// <summary>
    /// Representa um registro de ingestão de água armazenado no sistema.
    /// </summary>
    /// <remarks>
    /// Utilizado por endpoints GET para retornar registros individuais de hidratação.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "id": 15,
    ///   "date": "2025-01-10",
    ///   "amountMl": 250
    /// }
    /// ```
    /// </remarks>
    public class WaterIntakeDto
    {
        /// <summary>
        /// Identificador único da entrada de ingestão de água.
        /// </summary>
        /// <example>15</example>
        public int Id { get; set; }

        /// <summary>
        /// Data da ingestão de água.
        /// </summary>
        /// <example>2025-01-10</example>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Quantidade de água consumida, em mililitros.
        /// </summary>
        /// <example>250</example>
        public int AmountMl { get; set; }
    }

    /// <summary>
    /// Representa o payload usado para registrar uma nova ingestão de água.
    /// </summary>
    /// <remarks>
    /// A API associa automaticamente a ingestão com a data atual.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "amountMl": 300
    /// }
    /// ```
    /// </remarks>
    public class CreateWaterIntakeDto
    {
        /// <summary>
        /// Quantidade de água consumida (em mililitros).
        /// Deve ser maior que zero.
        /// </summary>
        /// <example>300</example>
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser de pelo menos 1 mL.")]
        public int AmountMl { get; set; }
    }

    /// <summary>
    /// Representa o payload usado para atualizar um registro existente de ingestão de água.
    /// </summary>
    /// <remarks>
    /// O ID do registro é informado na URL.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "amountMl": 500
    /// }
    /// ```
    /// </remarks>
    public class UpdateWaterIntakeDto
    {
        /// <summary>
        /// Quantidade atualizada de água consumida (em mililitros).
        /// Deve ser maior que zero.
        /// </summary>
        /// <example>500</example>
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser de pelo menos 1 mL.")]
        public int AmountMl { get; set; }
    }
}
