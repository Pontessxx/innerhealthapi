using System.ComponentModel.DataAnnotations;
namespace InnerHealth.Api.Auth
{
    /// <summary>
    /// Representa o payload utilizado para autenticação do usuário.
    /// </summary>
    /// <remarks>
    /// Este DTO é enviado para o endpoint de login contendo as credenciais
    /// (e-mail e senha) que serão validadas pela API. Caso as informações estejam
    /// corretas, um token JWT será retornado para acesso às rotas protegidas.
    ///
    /// <b>Exemplo:</b>
    /// ```json
    /// {
    ///   "email": "usuario@exemplo.com",
    ///   "password": "minhasenha123"
    /// }
    /// ```
    /// </remarks>
    public class AuthRequestDto
    {
        /// <summary>
        /// Endereço de e-mail associado à conta do usuário.
        /// </summary>
        /// <example>usuario@exemplo.com</example>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        public string Email { get; set; } = "";

        /// <summary>
        /// Senha em texto puro fornecida pelo usuário.
        /// </summary>
        /// <remarks>
        /// A senha será comparada com o hash armazenado (BCrypt) para validação.
        /// </remarks>
        /// <example>minhasenha123</example>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; } = "";
    }
}
