using Microsoft.EntityFrameworkCore;

namespace Planner.Model.Repositories
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork(new ScheduleDbContext(new DbContextOptionsBuilder().UseSqlServer("Server=.;Database=ToDoList;Trusted_Connection=true;").Options));
        }
    }
}
