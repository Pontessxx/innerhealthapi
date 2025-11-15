using Microsoft.EntityFrameworkCore;
using InnerHealth.Api.Data;
using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

/// <inheritdoc />
public class PhysicalActivityService : IPhysicalActivityService
{
    private readonly ApplicationDbContext _context;
    public PhysicalActivityService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<PhysicalActivity>> GetActivitiesAsync(DateOnly date)
    {
        return await _context.PhysicalActivities
            .Where(p => p.Date == date)
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<IDictionary<DateOnly, IEnumerable<PhysicalActivity>>> GetWeeklyActivitiesAsync(DateOnly weekStart)
    {
        var dict = new Dictionary<DateOnly, IEnumerable<PhysicalActivity>>();
        for (int i = 0; i < 7; i++)
        {
            dict[weekStart.AddDays(i)] = Enumerable.Empty<PhysicalActivity>();
        }
        var weekEnd = weekStart.AddDays(7);
        var activities = await _context.PhysicalActivities
            .Where(p => p.Date >= weekStart && p.Date < weekEnd)
            .AsNoTracking()
            .ToListAsync();
        foreach (var activity in activities)
        {
            if (!dict.ContainsKey(activity.Date))
            {
                dict[activity.Date] = new List<PhysicalActivity>();
            }
            var list = dict[activity.Date].ToList();
            list.Add(activity);
            dict[activity.Date] = list;
        }
        return dict;
    }
    public async Task<PhysicalActivity> AddActivityAsync(string? modality, int durationMinutes)
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var user = await _context.UserProfiles.FirstOrDefaultAsync();
        if (user == null)
        {
            user = new UserProfile { Weight = 0, Height = 0, Age = 0 };
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
        }
        var activity = new PhysicalActivity
        {
            Date = date,
            Modality = modality,
            DurationMinutes = durationMinutes,
            UserProfileId = user.Id
        };
        _context.PhysicalActivities.Add(activity);
        await _context.SaveChangesAsync();
        return activity;
    }
    public async Task<PhysicalActivity?> UpdateActivityAsync(int id, string? modality, int durationMinutes)
    {
        var activity = await _context.PhysicalActivities.FindAsync(id);
        if (activity == null) return null;
        activity.Modality = modality;
        activity.DurationMinutes = durationMinutes;
        await _context.SaveChangesAsync();
        return activity;
    }
    public async Task<bool> DeleteActivityAsync(int id)
    {
        var activity = await _context.PhysicalActivities.FindAsync(id);
        if (activity == null) return false;
        _context.PhysicalActivities.Remove(activity);
        await _context.SaveChangesAsync();
        return true;
    }
}