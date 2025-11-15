using Microsoft.EntityFrameworkCore;
using InnerHealth.Api.Data;
using InnerHealth.Api.Models;

namespace InnerHealth.Api.Services;

/// <inheritdoc />
public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<TaskItem>> GetTasksAsync(DateOnly date)
    {
        return await _context.TaskItems.Where(t => t.Date == date).AsNoTracking().ToListAsync();
    }
    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        return await _context.TaskItems.AsNoTracking().ToListAsync();
    }
    public async Task<TaskItem> AddTaskAsync(string title, string? description, DateOnly date, int? priority)
    {
        var user = await _context.UserProfiles.FirstOrDefaultAsync();
        if (user == null)
        {
            user = new UserProfile { Weight = 0, Height = 0, Age = 0 };
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
        }
        var task = new TaskItem
        {
            Title = title,
            Description = description,
            Date = date,
            IsComplete = false,
            Priority = priority,
            UserProfileId = user.Id
        };
        _context.TaskItems.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }
    public async Task<TaskItem?> UpdateTaskAsync(int id, string title, string? description, DateOnly date, bool isComplete, int? priority)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task == null) return null;
        task.Title = title;
        task.Description = description;
        task.Date = date;
        task.IsComplete = isComplete;
        task.Priority = priority;
        await _context.SaveChangesAsync();
        return task;
    }
    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task == null) return false;
        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}