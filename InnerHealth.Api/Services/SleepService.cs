using Microsoft.EntityFrameworkCore;
using InnerHealth.Api.Data;
using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

/// <inheritdoc />
public class SleepService : ISleepService
{
    private readonly ApplicationDbContext _context;
    public SleepService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<SleepRecord?> GetRecordAsync(DateOnly date)
    {
        return await _context.SleepRecords.AsNoTracking().FirstOrDefaultAsync(s => s.Date == date);
    }
    public async Task<IDictionary<DateOnly, SleepRecord?>> GetWeeklyRecordsAsync(DateOnly weekStart)
    {
        var weekEnd = weekStart.AddDays(7);
        var records = await _context.SleepRecords.Where(s => s.Date >= weekStart && s.Date < weekEnd)
            .AsNoTracking()
            .ToListAsync();
        var dict = new Dictionary<DateOnly, SleepRecord?>();
        for (int i = 0; i < 7; i++)
        {
            var day = weekStart.AddDays(i);
            dict[day] = null;
        }
        foreach (var record in records)
        {
            dict[record.Date] = record;
        }
        return dict;
    }
    public async Task<SleepRecord> AddRecordAsync(decimal hours, int quality)
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var user = await _context.UserProfiles.FirstOrDefaultAsync();
        if (user == null)
        {
            user = new UserProfile { Weight = 0, Height = 0, Age = 0 };
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
        }
        var record = new SleepRecord
        {
            Date = date,
            Hours = hours,
            Quality = quality,
            UserProfileId = user.Id
        };
        _context.SleepRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }
    public async Task<SleepRecord?> UpdateRecordAsync(int id, decimal hours, int quality)
    {
        var record = await _context.SleepRecords.FindAsync(id);
        if (record == null) return null;
        record.Hours = hours;
        record.Quality = quality;
        await _context.SaveChangesAsync();
        return record;
    }
    public async Task<bool> DeleteRecordAsync(int id)
    {
        var record = await _context.SleepRecords.FindAsync(id);
        if (record == null) return false;
        _context.SleepRecords.Remove(record);
        await _context.SaveChangesAsync();
        return true;
    }
}