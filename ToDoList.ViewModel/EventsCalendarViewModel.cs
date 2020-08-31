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

        /// <summary>
        /// Changes CurrentlyDisplayedMonth to the next month
        /// </summary>
        public void NextMonth()
        {
            CurrentlyDisplayedMonth = CurrentlyDisplayedMonth.Month < 12 ? new DateTime(CurrentlyDisplayedMonth.Year, CurrentlyDisplayedMonth.Month + 1, 0) :
                new DateTime(CurrentlyDisplayedMonth.Year + 1, 1, 0);
        }

        public void PreviousMonth()
        {
            CurrentlyDisplayedMonth = CurrentlyDisplayedMonth.Month > 1 ? new DateTime(CurrentlyDisplayedMonth.Year, CurrentlyDisplayedMonth.Month - 1, 0) :
                new DateTime(CurrentlyDisplayedMonth.Year - 1, 12, 0);
        }
    }
}
