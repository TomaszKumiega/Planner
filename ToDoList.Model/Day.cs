using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Model
{
    public class Day
    {
        public DateTime Date { get; }
        public List<Event> Events { get; }

        public Day(DateTime date)
        {
            Date = date;
            Events = new List<Event>();
        }

        public void AddEvent(Event e)
        {
            Events.Add(e);
        }

        public void RemoveEvent(Event e)
        {
            Events.Remove(e);
        }
    }
}
