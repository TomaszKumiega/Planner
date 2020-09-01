using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ToDoList.Model.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void Update(T entity);

        void Remove(Guid id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}