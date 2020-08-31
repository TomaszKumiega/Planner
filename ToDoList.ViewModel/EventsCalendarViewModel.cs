using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Model;

namespace ToDoList.ViewModel
{
    public class EventsCalendarViewModel
    {
        public User User { get; private set; }
        public List<Event> DisposableEvents { get; }
        public List<Event> RepetetiveEvents { get; }
        public DateTime CurrentlyDisplayedMonth { get; private set; }
        public int CurrentlySelectedDay { get; private set; }

        public EventsCalendarViewModel()
        {
            
        }
    }
}
