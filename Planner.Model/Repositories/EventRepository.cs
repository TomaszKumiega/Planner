using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Planner.Model.Repositories
{
    public class EventRepository : IRepository<Event>
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ScheduleDbContext _context;

        public EventRepository(ScheduleDbContext context)
        {
            _context = context;
            _logger.Debug("Event repository created.");
        }

        public void Add(Event entity)
        {
            if (entity == null) throw new ArgumentNullException();
            if (entity.Id == null) throw new ArgumentException("Entity doesnt have Id");

            _context.Events.Add(entity);

            _logger.Info("Entity: " + entity.Id.ToString() + "was added to the database");
        }

        public void AddRange(IEnumerable<Event> entities)
        {
            if (entities == null) throw new ArgumentNullException();
            if (!entities.Any()) throw new ArgumentException("No entities on the list");
            if (entities.Where(x => x.Id == null).ToList().Any()) throw new ArgumentException("Entity doesnt have Id");

            _context.Events.AddRange(entities);

            foreach (var t in entities) _logger.Info("Entity: " + t.Id.ToString() + "was added to the database");
        }

        public IEnumerable<Event> Find(Expression<Func<Event, bool>> predicate)
        {
            var items = _context.Events.ToList();
            return items.AsQueryable<Event>().Where<Event>(predicate).ToList();
        }

        public IEnumerable<Event> GetAll()
        {
            return _context.Events;
        }

        public Event GetById(Guid id)
        {
            var events = _context.Events.Where(x => x.Id == id).ToList();
            if (events.Count > 0) return events[0];
            else return null;
        }

        public void Remove(Guid id)
        {
            var events = this.Find(x => x.Id == id).ToList();

            if (events.Count != 0)
            {
                _context.Events.Remove(events[0]);
                _logger.Info("Event: " + id.ToString() + " was removed from database");
            }
        }

        public void Remove(Event entity)
        {
            if (entity == null) throw new ArgumentNullException();

            _context.Events.Remove(entity);
            _logger.Info("Event: " + entity.Id.ToString() + " was removed from database");
        }

        public void RemoveRange(IEnumerable<Event> entities)
        {
            if (entities == null) throw new ArgumentNullException();
            if (!entities.Any()) throw new ArgumentException("No entities on the list");

            _context.Events.RemoveRange(entities);

            foreach(var t in entities) _logger.Info("Event: " + t.Id.ToString() + " was removed from database");
        }

        public void Update(Event entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

    }
}
