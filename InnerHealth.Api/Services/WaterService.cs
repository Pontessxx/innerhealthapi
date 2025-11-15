using Microsoft.EntityFrameworkCore;
using InnerHealth.Api.Models;
using InnerHealth.Api.Data;

namespace InnerHealth.Api.Services;

/// <inheritdoc />
public class WaterService : IWaterService
{
    private readonly ApplicationDbContext _context;
    public WaterService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WaterIntake>> GetIntakesAsync(DateOnly date)
    {
        return await _context.WaterIntakes
            .Where(w => w.Date == date)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> GetDailyTotalAsync(DateOnly date)
    {
        return await _context.WaterIntakes
            .Where(w => w.Date == date)
            .SumAsync(w => w.AmountMl);
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
        var grouped = await _context.WaterIntakes
            .Where(w => w.Date >= weekStart && w.Date < weekEnd)
            .GroupBy(w => w.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(x => x.AmountMl) })
            .ToListAsync();
        foreach (var item in grouped)
        {
            totals[item.Date] = item.Total;
        }
        return totals;
    }

    public async Task<int> GetRecommendedDailyAmountAsync()
    {
        var user = await _context.UserProfiles.FirstOrDefaultAsync();
        if (user == null || user.Weight <= 0)
        {
            return 0;
        }
        var amount = user.Weight * 35m;
        return (int)Math.Round(amount);
    }

    public async Task<WaterIntake> AddIntakeAsync(int amountMl)
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var user = await _context.UserProfiles.FirstOrDefaultAsync();
        if (user == null)
        {
            // Create a default user if none exists
            user = new UserProfile { Weight = 0, Height = 0, Age = 0 };
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
        }
        var intake = new WaterIntake
        {
            Date = date,
            AmountMl = amountMl,
            UserProfileId = user.Id
        };
        _context.WaterIntakes.Add(intake);
        await _context.SaveChangesAsync();
        return intake;
    }

    public async Task<WaterIntake?> UpdateIntakeAsync(int id, int amountMl)
    {
        var intake = await _context.WaterIntakes.FindAsync(id);
        if (intake == null)
        {
            return null;
        }
        intake.AmountMl = amountMl;
        await _context.SaveChangesAsync();
        return intake;
    }

    public async Task<bool> DeleteIntakeAsync(int id)
    {
        var intake = await _context.WaterIntakes.FindAsync(id);
        if (intake == null)
        {
            return false;
        }
        _context.WaterIntakes.Remove(intake);
        await _context.SaveChangesAsync();
        return true;
    }
}