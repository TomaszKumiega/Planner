﻿using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Model;
using ToDoList.Model.Repositories;
using Event = ToDoList.Model.Event;
using EventType = ToDoList.Model.EventType;
using User = ToDoList.Model.User;

namespace ToDoList.ViewModel
{
    public class EventsCalendarViewModel
    {
        public User User { get; private set; }
        public DateTime CurrentlyDisplayedMonth { get; private set; }
        public Dictionary<DateTime, List<Event>> Schedule { get; private set; }
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
            var e = new Event(name, eventType,eventDifficulty, dateTime);

            _unitOfWork.EventRepository.Add(e);
            _unitOfWork.SaveChanges();

            LoadScheduleAsync();
        }


        public void CreateEvent(string name, EventType eventType, EventDifficulty eventDifficulty, RecurrencePattern repetitionPattern)
        {
            var e = new Event(name, eventType, eventDifficulty, repetitionPattern);

            _unitOfWork.EventRepository.Add(e);
            _unitOfWork.SaveChanges();

            LoadScheduleAsync();
        }

        private async Task LoadScheduleAsync()
        {
            Schedule = new Dictionary<DateTime, List<Event>>();
            
            var days = GetCurrentlyDisplayedDays();
            var repetetiveEvents = await Task.Run(() => _unitOfWork.EventRepository.Find(x => x.RecurrencePattern != null).ToList());

            foreach (var t in days)
            {
                var listOfEvents = new List<Event>();

                //Finds all repetetive events that are going to happen on the day t
                foreach (var k in repetetiveEvents)
                {
                    if (k.IsDateTimeMatchingRecurrencePattern(t)) listOfEvents.Add(k);
                }
            
                // Finds all disposable events that are going to happen on the day t
                listOfEvents.AddRange( await Task.Run(() => _unitOfWork.EventRepository.Find(x => x.StartDateTime.Value.Date == t.Date).ToList()));

                Schedule.Add(t, listOfEvents);
            }
        }

        private List<DateTime> GetCurrentlyDisplayedDays()
        {
            var listOfDays = new List<DateTime>();
            var date = new DateTime(CurrentlyDisplayedMonth.Year, CurrentlyDisplayedMonth.Month, 1);

            // Generate days from previous months to fill a gap at the begining of the calendar
            int numberOfDaysFromPreviousMonth;

            // Sunday is 0
            if ((int)date.DayOfWeek == 0) numberOfDaysFromPreviousMonth = 6;
            else numberOfDaysFromPreviousMonth = (int)date.DayOfWeek - 1;

            date.AddDays(-numberOfDaysFromPreviousMonth);

            for (int i=0;i<numberOfDaysFromPreviousMonth;i++)
            {
                listOfDays.Add(date);
                date.AddDays(1);
            }

            // Generate list of days from currently displayed month
            listOfDays.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month))
                .Select(day => new DateTime(date.Year, date.Month, day))
                .ToList());

            // Generate days from next month to fill a gap at the end of the calendar
            date.AddMonths(1);

            var numberOfDaysFromNextMonth = 35 - listOfDays.Count;

            for (int i=0; i<numberOfDaysFromNextMonth;i++)
            {
                listOfDays.Add(date);
                date.AddDays(1);
            }
            
            return listOfDays;
        }

        public void RemoveEvent(Event @event)
        {
            var item = Schedule.First(x => x.Value.Contains(@event));
            Schedule.Remove(item.Key);

            _unitOfWork.EventRepository.Remove(@event);
            _unitOfWork.SaveChanges();
        }

        //TODO:
        /*
        public void CompleteEvent()
        {

        }

        */
    }
}
