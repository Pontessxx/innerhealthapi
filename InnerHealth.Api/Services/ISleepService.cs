using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

// Operações relacionadas aos registros de sono.
public interface ISleepService
{
    Task<SleepRecord?> GetRecordAsync(DateOnly date);
    Task<IDictionary<DateOnly, SleepRecord?>> GetWeeklyRecordsAsync(DateOnly weekStart);
    Task<SleepRecord> AddRecordAsync(decimal hours, int quality);
    Task<SleepRecord?> UpdateRecordAsync(int id, decimal hours, int quality);
    Task<bool> DeleteRecordAsync(int id);
}