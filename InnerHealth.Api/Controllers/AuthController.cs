using BCrypt.Net;
using InnerHealth.Api.Auth;
using InnerHealth.Api.Domain.Entities;
using InnerHealth.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace InnerHealth.Api.Controllers
{
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Email and password are required." });

            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials." });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid credentials." });

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
