using Microsoft.EntityFrameworkCore;
using InnerHealth.Api.Models;
using InnerHealth.Api.Domain.Entities;
using InnerHealth.Api.Domain.Enums;

namespace InnerHealth.Api.Data
{
    /// <summary>
    /// Contexto de banco de dados EF Core da API InnerHealth.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<WaterIntake> WaterIntakes => Set<WaterIntake>();
        public DbSet<SunlightSession> SunlightSessions => Set<SunlightSession>();
        public DbSet<MeditationSession> MeditationSessions => Set<MeditationSession>();
        public DbSet<SleepRecord> SleepRecords => Set<SleepRecord>();
        public DbSet<PhysicalActivity> PhysicalActivities => Set<PhysicalActivity>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure DateOnly properties
            builder.Entity<WaterIntake>().Property(w => w.Date).HasColumnType("date");
            builder.Entity<SunlightSession>().Property(s => s.Date).HasColumnType("date");
            builder.Entity<MeditationSession>().Property(m => m.Date).HasColumnType("date");
            builder.Entity<SleepRecord>().Property(s => s.Date).HasColumnType("date");
            builder.Entity<PhysicalActivity>().Property(p => p.Date).HasColumnType("date");
            builder.Entity<TaskItem>().Property(t => t.Date).HasColumnType("date");

            // Decimal precision (SQLite ignora, mas documenta intenção)
            builder.Entity<UserProfile>().Property(u => u.Weight).HasColumnType("decimal(10,2)");
            builder.Entity<UserProfile>().Property(u => u.Height).HasColumnType("decimal(10,2)");
            builder.Entity<UserProfile>().Property(u => u.SleepHours).HasColumnType("decimal(5,2)");
            builder.Entity<SleepRecord>().Property(s => s.Hours).HasColumnType("decimal(5,2)");
        }
    }
}
