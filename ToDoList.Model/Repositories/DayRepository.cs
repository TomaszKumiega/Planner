using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ToDoList.Model.Repositories
{
    public class DayRepository : IRepository<Day>
    {
        private readonly ScheduleDbContext _context;

        public DayRepository(ScheduleDbContext context)
        {
            _context = context;
        }

        public void Add(Day entity)
        {
            if (entity == null) throw new ArgumentNullException();

            _context.Days.Add(entity);
        }

        public void AddRange(IEnumerable<Day> entities)
        {
            if (entities == null) throw new ArgumentNullException();
            if (!entities.Any()) throw new ArgumentException("No entities on the list");

            _context.Days.AddRange(entities);
        }

        public IEnumerable<Day> Find(Expression<Func<Day, bool>> predicate)
        {
            return _context.Days.Where(predicate).ToList();
        }

        public IEnumerable<Day> GetAll()
        {
            return _context.Days;
        }

        public Day GetById(Guid id)
        {
            var days = _context.Days.Where(x => x.Id == id).ToList();
            if (days.Count > 0) return days[0];
            else return null;
        }

        public void Remove(Guid id)
        {
            var days = this.Find(x => x.Id == id).ToList();

            if (days.Count > 0)
            {
                _context.Days.Remove(days[0]);
            }
        }

        public void Remove(Day entity)
        {
            if (entity == null) throw new ArgumentNullException();

            _context.Days.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Day> entities)
        {
            if (entities == null) throw new ArgumentNullException();
            if (!entities.Any()) throw new ArgumentException("No entities on the list");

            _context.Days.RemoveRange(entities);
        }

        public void Update(Day entity)
        {
            var day = _context.Days.Find(entity);

            day.Date = entity.Date;
            day.Events = entity.Events;
        }
    }
}
