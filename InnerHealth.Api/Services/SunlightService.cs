using Microsoft.EntityFrameworkCore;
using InnerHealth.Api.Data;
using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

/// <inheritdoc />
public class SunlightService : ISunlightService
{
    private readonly ApplicationDbContext _context;
    private const int RecommendedMinutes = 10;

    public SunlightService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SunlightSession>> GetSessionsAsync(DateOnly date)
    {
        return await _context.SunlightSessions
            .Where(s => s.Date == date)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> GetDailyTotalAsync(DateOnly date)
    {
        return await _context.SunlightSessions
            .Where(s => s.Date == date)
            .SumAsync(s => s.Minutes);
    }

    public async Task<IDictionary<DateOnly, int>> GetWeeklyTotalsAsync(DateOnly weekStart)
    {
        var totals = new Dictionary<DateOnly, int>();
        for (int i = 0; i < 7; i++)
        {
            var day = weekStart.AddDays(i);
            totals[day] = 0;
        }
        var weekEnd = weekStart.AddDays(7);
        var grouped = await _context.SunlightSessions
            .Where(s => s.Date >= weekStart && s.Date < weekEnd)
            .GroupBy(s => s.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Minutes) })
            .ToListAsync();
        foreach (var item in grouped)
        {
            totals[item.Date] = item.Total;
        }
        return totals;
    }

    public int GetRecommendedDailyMinutes() => RecommendedMinutes;

    public async Task<SunlightSession> AddSessionAsync(int minutes)
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var user = await _context.UserProfiles.FirstOrDefaultAsync();
        if (user == null)
        {
            user = new UserProfile { Weight = 0, Height = 0, Age = 0 };
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
        }
        var session = new SunlightSession
        {
            Date = date,
            Minutes = minutes,
            UserProfileId = user.Id
        };
        _context.SunlightSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<SunlightSession?> UpdateSessionAsync(int id, int minutes)
    {
        var session = await _context.SunlightSessions.FindAsync(id);
        if (session == null) return null;
        session.Minutes = minutes;
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<bool> DeleteSessionAsync(int id)
    {
        var session = await _context.SunlightSessions.FindAsync(id);
        if (session == null) return false;
        _context.SunlightSessions.Remove(session);
        await _context.SaveChangesAsync();
        return true;
    }
}