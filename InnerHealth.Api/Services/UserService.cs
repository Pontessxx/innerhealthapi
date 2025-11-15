using Microsoft.EntityFrameworkCore;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Models;
using InnerHealth.Api.Data;

namespace InnerHealth.Api.Services;

// Serviço responsável por lidar com o perfil do usuário.
public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Retorna o perfil do usuário (só existe um). Se não existir, só devolve null.
    public async Task<UserProfile?> GetUserAsync()
    {
        var user = await _context.UserProfiles.FirstOrDefaultAsync();
        return user;
    }

    // Atualiza o perfil do usuário. Cria um novo se ainda não existir.
    public async Task<UserProfile> UpdateUserAsync(UpdateUserProfileDto dto)
    {
        var user = await _context.UserProfiles.FirstOrDefaultAsync();

        // Se não existir, cria um perfil vazio.
        if (user == null)
        {
            user = new UserProfile();
            _context.UserProfiles.Add(user);
        }

        // Atualiza os campos.
        user.Weight = dto.Weight;
        user.Height = dto.Height;
        user.Age = dto.Age;
        user.SleepQuality = dto.SleepQuality;
        user.SleepHours = dto.SleepHours;

        await _context.SaveChangesAsync();
        return user;
    }
}