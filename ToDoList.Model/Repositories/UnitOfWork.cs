using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Model.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ScheduleDbContext _dbContext;

        public IRepository<User> UserRepository { get; }
        public IRepository<Day> DayRepository { get;  }
        public IRepository<Event> EventRepository { get;  }

        public UnitOfWork(ScheduleDbContext dbContext)
        {
            _dbContext = dbContext;
            UserRepository = new UserRepository(dbContext);
            DayRepository = new DayRepository(dbContext);
            EventRepository = new EventRepository(dbContext);
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
