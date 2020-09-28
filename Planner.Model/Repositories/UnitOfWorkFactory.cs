using Microsoft.EntityFrameworkCore;

namespace Planner.Model.Repositories
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public IUnitOfWork GetUnitOfWork()
        {
            _logger.Debug("Unit of work object is created");
            return new UnitOfWork(new ScheduleDbContext(new DbContextOptionsBuilder().UseSqlServer("Server=localhost;Database=ToDoList;Trusted_Connection=true;").Options));
        }
    }
}
