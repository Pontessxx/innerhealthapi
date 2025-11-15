using Microsoft.EntityFrameworkCore;
using InnerHealth.Api.Data;
using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

// Implementação do serviço de meditação.
public class MeditationService : IMeditationService
{
    private readonly ApplicationDbContext _context;
    private const int RecommendedMinutes = 5;

    public MeditationService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Retorna todas as sessões de meditação de um dia.
    public async Task<IEnumerable<MeditationSession>> GetSessionsAsync(DateOnly date)
    {
        return await _context.MeditationSessions
            .Where(m => m.Date == date)
            .AsNoTracking()
            .ToListAsync();
    }

    // Soma total de minutos meditados no dia.
    public async Task<int> GetDailyTotalAsync(DateOnly date)
    {
        return await _context.MeditationSessions
            .Where(m => m.Date == date)
            .SumAsync(m => m.Minutes);
    }

    // Retorna os totais de meditação dos 7 dias a partir da data enviada.
    public async Task<IDictionary<DateOnly, int>> GetWeeklyTotalsAsync(DateOnly weekStart)
    {
        var totals = new Dictionary<DateOnly, int>();

        // Inicializa o dicionário com os 7 dias da semana.
        for (int i = 0; i < 7; i++)
        {
            var day = weekStart.AddDays(i);
            totals[day] = 0;
        }

        var weekEnd = weekStart.AddDays(7);

        // Agrupa por data e soma os minutos.
        var grouped = await _context.MeditationSessions
            .Where(m => m.Date >= weekStart && m.Date < weekEnd)
            .GroupBy(m => m.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Minutes) })
            .ToListAsync();

        // Preenche o dicionário com os valores reais.
        foreach (var item in grouped)
        {
            totals[item.Date] = item.Total;
        }

        return totals;
    }

    // Minutos recomendados por dia.
    public int GetRecommendedDailyMinutes() => RecommendedMinutes;

    // Cadastra uma nova sessão (referente ao dia de hoje).
    public async Task<MeditationSession> AddSessionAsync(int minutes)
    {
        var date = DateOnly.FromDateTime(DateTime.Now);

        // Garante que o usuário exista.
        var user = await _context.UserProfiles.FirstOrDefaultAsync();
        if (user == null)
        {
            user = new UserProfile { Weight = 0, Height = 0, Age = 0 };
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
        }

        var session = new MeditationSession
        {
            Date = date,
            Minutes = minutes,
            UserProfileId = user.Id
        };

        _context.MeditationSessions.Add(session);
        await _context.SaveChangesAsync();

        return session;
    }

    // Atualiza minutos de uma sessão existente.
    public async Task<MeditationSession?> UpdateSessionAsync(int id, int minutes)
    {
        var session = await _context.MeditationSessions.FindAsync(id);
        if (session == null)
            return null;

        session.Minutes = minutes;
        await _context.SaveChangesAsync();

        return session;
    }

    // Remove uma sessão pelo ID.
    public async Task<bool> DeleteSessionAsync(int id)
    {
        var session = await _context.MeditationSessions.FindAsync(id);
        if (session == null)
            return false;

        _context.MeditationSessions.Remove(session);
        await _context.SaveChangesAsync();

        return true;
    }
}