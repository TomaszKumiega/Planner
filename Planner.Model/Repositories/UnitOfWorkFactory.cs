using System;
using System.Collections.Generic;
using System.Text;

namespace Planner.Model.Repositories
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork(new ScheduleDbContext());
        }
    }
}
