using Microsoft.EntityFrameworkCore;

namespace Planner.Model
{
    public class ScheduleDbContext : DbContext
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public ScheduleDbContext(DbContextOptions options) : base(options)
        {
            _logger.Debug("ScheduleDbContext created");
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(x => x._RecurrencePattern).HasColumnName("RecurrencePattern");
            modelBuilder.Entity<Event>()
                .Property(x => x._DaysCompleted).HasColumnName("DaysCompleted");
        }

    }
}
