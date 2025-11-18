using InnerHealth.Api.Domain.Enums;

namespace InnerHealth.Api.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = "";
        public string Email { get; set; } = "";
        public UserRole Role { get; set; }
    }
}
