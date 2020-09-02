using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoList.Model;
using ToDoList.Model.Repositories;

namespace ToDoList.ViewModel
{
    public class EventsCalendarViewModel
    {
        public User User { get; private set; }
        public DateTime CurrentlyDisplayedMonth { get; private set; }
        public List<Day> DisplayedDays { get; private set; }
        public DateTime CurrentlySelectedDay { get; private set; }

        private readonly IUnitOfWork _unitOfWork;

        public EventsCalendarViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }

        /// <summary>
        /// Changes <see cref="CurrentlyDisplayedMonth"/> to the next month
        /// </summary>
        public void NextMonth()
        {
            CurrentlyDisplayedMonth = CurrentlyDisplayedMonth.Month < 12 ? new DateTime(CurrentlyDisplayedMonth.Year, CurrentlyDisplayedMonth.Month + 1, 0) :
                new DateTime(CurrentlyDisplayedMonth.Year + 1, 1, 0);
        }

        /// <summary>
        /// Changes <see cref="CurrentlyDisplayedMonth"/> to the previous month
        /// </summary>
        public void PreviousMonth()
        {
            CurrentlyDisplayedMonth = CurrentlyDisplayedMonth.Month > 1 ? new DateTime(CurrentlyDisplayedMonth.Year, CurrentlyDisplayedMonth.Month - 1, 0) :
                new DateTime(CurrentlyDisplayedMonth.Year - 1, 12, 0);
        }

        
        public void CreateEvent(string name, EventType eventType, EventDifficulty eventDifficulty, DateTime dateTime)
        {
            var days = _unitOfWork.DayRepository.Find(x => x.Date.Date == dateTime.Date).ToList();
            var e = new Event(name, eventType,eventDifficulty, days[0]);

            if (days.Any())
            {

                days[0].Events.Add(e);
                _unitOfWork.SaveChanges();
            }
            else
            {
                var day = new Day(dateTime);
                day.Events.Add(e);
                _unitOfWork.DayRepository.Add(day);
                _unitOfWork.SaveChanges();
            }

            LoadDisplayedDays();
        }


        public void CreateEvent(string name, EventType eventType, EventDifficulty eventDifficulty, RepetitionPattern repetitionPattern)
        {
            var e = new Event(name, eventType, eventDifficulty, repetitionPattern);

            _unitOfWork.EventRepository.Add(e);
            _unitOfWork.SaveChanges();

            LoadDisplayedDays();
        }

        private void LoadDisplayedDays()
        {

        }

        //TODO:
        /*
        public void CompleteEvent()
        {

        }

        public void RemoveEvent()
        {

        }
        */
    }
}
