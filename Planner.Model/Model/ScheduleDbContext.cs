using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Planner.Model.Model;

namespace Planner.Model
{
    public class ScheduleDbContext : DbContext
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public ScheduleDbContext()
        {

        }

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
            modelBuilder.Entity<User>(x =>
            {
                x.HasKey(u => u.Id);
                x.ToTable("Users");
            });
            modelBuilder.Entity<Event>(x => {
                x.HasKey(e => e.Id);
                x.Property(e => e.UserId).IsRequired();
                x.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId);
                x.ToTable("Events");
            });
        }

    }
}
