using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Model.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ScheduleDbContext _dbContext;

        public IRepository<User> UserRepository { get; }
        public IRepository<Event> EventRepository { get;  }

        public UnitOfWork(ScheduleDbContext dbContext)
        {
            _dbContext = dbContext;
            UserRepository = new UserRepository(dbContext);
            EventRepository = new EventRepository(dbContext);
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
