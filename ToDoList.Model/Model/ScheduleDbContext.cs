using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace ToDoList.Model
{
    public class ScheduleDbContext : DbContext
    {
        public ScheduleDbContext() : base("Server=.;Database=ToDoList;Integrated Security=true;")
        {

        }

        public System.Data.Entity.DbSet<Event> Events { get; set; }
        public System.Data.Entity.DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(x => x._RecurrencePattern).HasColumnName("RecurrencePattern");
            modelBuilder.Entity<Event>()
                .Property(x => x._DaysCompleted).HasColumnName("DaysCompleted");
        }

    }
}
