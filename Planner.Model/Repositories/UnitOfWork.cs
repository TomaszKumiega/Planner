using Planner.Model.Model;

namespace Planner.Model.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

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
            _logger.Info("Saving changes to database");
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _logger.Debug("Unit of work was disposed");
        }
    }
}
