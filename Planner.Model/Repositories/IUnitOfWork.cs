using Planner.Model.Model;
using System;

namespace Planner.Model.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Event> EventRepository { get; }

        int SaveChanges();

    }
}
