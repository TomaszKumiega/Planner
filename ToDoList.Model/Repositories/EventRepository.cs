using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ToDoList.Model.Repositories
{
    public class EventRepository : IRepository<Event>
    {
        private readonly ScheduleDbContext _context;

        public EventRepository(ScheduleDbContext context)
        {
            _context = context;
        }

        public void Add(Event entity)
        {
            if (entity == null) throw new ArgumentNullException();

            _context.Events.Add(entity);
            _context.SaveChangesAsync();
        }

        public void AddRange(IEnumerable<Event> entities)
        {
            if (entities == null) throw new ArgumentNullException();
            if (!entities.Any()) throw new ArgumentException("No entities on the list");

            _context.Events.AddRange(entities);
            _context.SaveChangesAsync();
        }

        public IEnumerable<Event> Find(Expression<Func<Event, bool>> predicate)
        {
            return _context.Events.Where(predicate).ToList();
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
                _context.SaveChangesAsync();
            }
        }

        public void Remove(Event entity)
        {
            if (entity == null) throw new ArgumentNullException();

            _context.Events.Remove(entity);
            _context.SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<Event> entities)
        {
            if (entities == null) throw new ArgumentNullException();
            if (!entities.Any()) throw new ArgumentException("No entities on the list");

            _context.Events.RemoveRange(entities);
            _context.SaveChangesAsync();
        }

        public void Update(Event entity)
        {
            var e = _context.Events.Find(entity);

            e.Karma = entity.Karma;
            e.Name = entity.Name;
            e.RepetitionPattern = entity.RepetitionPattern;
            e.EventDifficulty = entity.EventDifficulty;
            e.EventType = entity.EventType;
            e.DateTime = entity.DateTime;

            _context.SaveChangesAsync();
        }
    }
}
