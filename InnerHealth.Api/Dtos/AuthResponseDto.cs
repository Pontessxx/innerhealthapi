using InnerHealth.Api.Domain.Enums;

namespace InnerHealth.Api.Auth
{
    /// <summary>
    /// Representa a resposta retornada quando um usuário é autenticado com sucesso.
    /// Inclui o token JWT gerado e informações básicas do usuário autenticado.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado pelo endpoint de login para retornar os dados
    /// de autenticação ao cliente.
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///   "email": "usuario@exemplo.com",
    ///   "role": "User"
    /// }
    /// ```
    /// </remarks>
    public class AuthResponseDto
    {
        /// <summary>
        /// Token JWT gerado após a autenticação bem-sucedida.
        /// Este token contém os claims do usuário e deve ser enviado
        /// no cabeçalho <c>Authorization</c> nas requisições subsequentes.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string Token { get; set; } = "";

        /// <summary>
        /// Endereço de e-mail do usuário autenticado.
        /// </summary>
        /// <example>usuario@exemplo.com</example>
        public string Email { get; set; } = "";

        /// <summary>
        /// Papel (role) associado ao usuário autenticado,
        /// como <c>User</c>, <c>Admin</c> ou outros papéis personalizados.
        /// </summary>
        /// <example>User</example>
        public UserRole Role { get; set; }
    }
}
