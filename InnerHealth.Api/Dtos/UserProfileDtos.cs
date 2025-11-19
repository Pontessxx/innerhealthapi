using System.ComponentModel.DataAnnotations;

namespace InnerHealth.Api.Dtos
{
    /// <summary>
    /// Representa o perfil de saúde do usuário,
    /// incluindo características físicas e indicadores de sono.
    /// </summary>
    /// <remarks>
    /// Este DTO é retornado pelos endpoints GET e contém
    /// dados utilizados pela aplicação para gerar recomendações
    /// e insights personalizados.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "id": 1,
    ///   "weight": 72.5,
    ///   "height": 178,
    ///   "age": 25,
    ///   "sleepQuality": 80,
    ///   "sleepHours": 7.2
    /// }
    /// ```
    /// </remarks>
    public class UserProfileDto
    {
        /// <summary>
        /// Identificador único do perfil do usuário.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Peso corporal do usuário em quilogramas.
        /// </summary>
        /// <example>72.5</example>
        public decimal Weight { get; set; }

        /// <summary>
        /// Altura do usuário em centímetros.
        /// </summary>
        /// <example>178</example>
        public decimal Height { get; set; }

        /// <summary>
        /// Idade do usuário em anos.
        /// </summary>
        /// <example>25</example>
        public int Age { get; set; }

        /// <summary>
        /// Média da qualidade do sono (0–100).
        /// </summary>
        /// <example>80</example>
        public int SleepQuality { get; set; }

        /// <summary>
        /// Média de horas dormidas por noite.
        /// Aceita valores decimais para maior precisão.
        /// </summary>
        /// <example>7.2</example>
        public decimal SleepHours { get; set; }
    }

    /// <summary>
    /// Representa o payload utilizado para atualizar
    /// o perfil de saúde do usuário.
    /// </summary>
    /// <remarks>
    /// Todos os campos são obrigatórios e devem seguir
    /// os intervalos de validação definidos.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "weight": 70.3,
    ///   "height": 175,
    ///   "age": 24,
    ///   "sleepQuality": 85,
    ///   "sleepHours": 7.0
    /// }
    /// ```
    /// </remarks>
    public class UpdateUserProfileDto
    {
        /// <summary>
        /// Peso corporal atualizado em quilogramas (1–1000).
        /// </summary>
        /// <example>70.3</example>
        [Range(1, 1000, ErrorMessage = "O peso deve estar entre 1 e 1000 kg.")]
        public decimal Weight { get; set; }

        /// <summary>
        /// Altura atualizada em centímetros (1–300).
        /// </summary>
        /// <example>175</example>
        [Range(1, 300, ErrorMessage = "A altura deve estar entre 1 e 300 cm.")]
        public decimal Height { get; set; }

        /// <summary>
        /// Idade atualizada do usuário (1–120).
        /// </summary>
        /// <example>24</example>
        [Range(1, 120, ErrorMessage = "A idade deve estar entre 1 e 120 anos.")]
        public int Age { get; set; }

        /// <summary>
        /// Nota atualizada de qualidade do sono (0–100).
        /// </summary>
        /// <example>85</example>
        [Range(0, 100, ErrorMessage = "A qualidade do sono deve estar entre 0 e 100.")]
        public int SleepQuality { get; set; }

        /// <summary>
        /// Média atualizada de horas dormidas por noite (0–24).
        /// </summary>
        /// <example>7</example>
        [Range(0, 24, ErrorMessage = "As horas de sono devem estar entre 0 e 24.")]
        public decimal SleepHours { get; set; }
    }
}
