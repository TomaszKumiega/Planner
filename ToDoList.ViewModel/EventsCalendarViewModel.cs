using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Model;

namespace ToDoList.ViewModel
{
    public class EventsCalendarViewModel
    {
        public User User { get; private set; }
        public DateTime CurrentlyDisplayedMonth { get; private set; }
        public List<Day> DisplayedDays { get; private set; }
        public List<Event> RepetetiveEvents { get; }
        public DateTime CurrentlySelectedDay { get; private set; }

        public EventsCalendarViewModel()
        {
            
        }
    }
}
