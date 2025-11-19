using BCrypt.Net;
using InnerHealth.Api.Auth;
using InnerHealth.Api.Domain.Entities;
using InnerHealth.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace InnerHealth.Api.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações de autenticação.
    /// </summary>
    /// <remarks>
    /// Este controlador fornece endpoints para realizar login e obter um token JWT.
    /// </remarks>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _repo;
        private readonly JwtService _jwt;

        public AuthController(UserRepository repo, JwtService jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        /// <summary>
        /// Autentica um usuário e retorna um token JWT.
        /// </summary>
        /// <remarks>
        /// <b>Exemplo de requisição:</b>
        ///
        ///     POST /api/auth/login
        ///     {
        ///        "email": "usuario@exemplo.com",
        ///        "password": "123456"
        ///     }
        ///
        /// Este endpoint valida as credenciais enviadas e, se forem válidas,
        /// retorna um token JWT que deve ser utilizado para acessar as rotas protegidas da API.
        /// </remarks>
        /// <param name="dto">Credenciais de login: e-mail e senha.</param>
        /// <returns>Um objeto contendo o token JWT gerado e dados do usuário autenticado.</returns>
        /// <response code="200">Retorna o token JWT junto aos dados do usuário.</response>
        /// <response code="400">E-mail ou senha não foram informados corretamente.</response>
        /// <response code="401">Credenciais inválidas.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "E-mail e senha são obrigatórios." });

            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized(new { message = "Credenciais inválidas." });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Credenciais inválidas." });

            var token = _jwt.GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            });
        }
    }
}
