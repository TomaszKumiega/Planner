using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Model.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<Event> EventRepository { get; }

        int SaveChanges();

    }
}
