using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Planner.Model.Model;

namespace Planner.Model.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ScheduleDbContext _context;
        public UserRepository(ScheduleDbContext context)
        {
            _context = context;
            _logger.Debug("User repository created.");
        }

        public void Add(User entity)
        {
            if (entity == null) throw new ArgumentNullException();

            _context.Users.Add(entity);
            _logger.Info("User: " + entity.Id + " added to the database");
        }

        public void AddRange(IEnumerable<User> entities)
        {
            if (entities == null) throw new ArgumentNullException();
            if (!entities.Any()) throw new ArgumentException("No entities on the list");

            _context.Users.AddRange(entities);

            foreach(var t in entities) _logger.Info("User: " + t.Id + " added to the database");
        }

        public void Remove(Guid id)
        {
            var users = this.Find(x => x.Id == id);

            _context.Users.Remove(users);
            _logger.Info("User: " + id.ToString() + " removed from the database");
        }

        public void Remove(User entity)
        {
            if (entity == null) throw new ArgumentNullException();

            _context.Users.Remove(entity);

            _logger.Info("User: " + entity.Id.ToString() + " removed from the database");
        }

        public User Find(System.Predicate<User> predicate)
        {
            var users = _context.Users.ToList();
            return users.Find(predicate);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(Guid id)
        {
            var users = _context.Users.Where(x => x.Id == id).ToList();
            if (users.Count > 0) return users[0];
            else return null;
        }

        public void RemoveRange(IEnumerable<User> entities)
        {
            _context.Users.RemoveRange(entities);
            foreach(var t in entities) _logger.Info("User: " + t.Id.ToString() + " removed from the database");
        }

        public void Update(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
