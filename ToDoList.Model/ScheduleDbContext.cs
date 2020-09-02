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

    }
}
