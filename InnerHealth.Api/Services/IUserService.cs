using InnerHealth.Api.Dtos;
using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

// Operações relacionadas ao perfil do usuário.
public interface IUserService
{
    // Retorna o perfil do usuário (só existe um no sistema).
    Task<UserProfile?> GetUserAsync();

    // Atualiza os dados do perfil.
    Task<UserProfile> UpdateUserAsync(UpdateUserProfileDto dto);
}