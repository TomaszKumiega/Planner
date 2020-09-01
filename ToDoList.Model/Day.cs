using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ToDoList.Model
{
    [Table("Days")]
    public class Day
    {
        [Key()]
        public Guid Id { get; set; }
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
