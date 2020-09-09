﻿using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Model;
using ToDoList.Model.Repositories;
using ToDoList.ViewModel.Commands;
using ToDoList.ViewModel.ObserverPattern;
using Event = ToDoList.Model.Event;
using EventType = ToDoList.Model.EventType;
using User = ToDoList.Model.User;

namespace ToDoList.ViewModel
{
    public class EventsCalendarViewModel : IEventsCalendarViewModel, INotifyPropertyChanged
    {
        private DateTime _currentlyDisplayedMonth;
        public User User { get; private set; }
        public Dictionary<DateTime, List<Event>> Schedule { get; private set; }
        public DateTime CurrentlySelectedDay { get; private set; }
        public List<IObserver> Observers { get; }
        public NextMonthCommand NextMonthCommand { get; }
        public PreviousMonthCommand PreviousMonthCommand { get; }

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public DateTime CurrentlyDisplayedMonth 
        { 
            get => _currentlyDisplayedMonth;
            
            private set
            {
                _currentlyDisplayedMonth = value;
                OnPropertyChanged("CurrentlyDisplayedMonth");
            }
        }


        public EventsCalendarViewModel(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            Observers = new List<IObserver>();
            CurrentlyDisplayedMonth = DateTime.Now;
            User = new User("DefaultUser");
            NextMonthCommand = new NextMonthCommand(this);
            PreviousMonthCommand = new PreviousMonthCommand(this);
        }

        /// <summary>
        /// Changes <see cref="CurrentlyDisplayedMonth"/> to the next month
        /// </summary>
        public void NextMonth()
        {
            CurrentlyDisplayedMonth = CurrentlyDisplayedMonth.Month < 12 ? new DateTime(CurrentlyDisplayedMonth.Year, CurrentlyDisplayedMonth.Month + 1, 1) :
                new DateTime(CurrentlyDisplayedMonth.Year + 1, 1, 1);
        }

        /// <summary>
        /// Changes <see cref="CurrentlyDisplayedMonth"/> to the previous month
        /// </summary>
        public void PreviousMonth()
        {
            CurrentlyDisplayedMonth = CurrentlyDisplayedMonth.Month > 1 ? new DateTime(CurrentlyDisplayedMonth.Year, CurrentlyDisplayedMonth.Month - 1, 1) :
                new DateTime(CurrentlyDisplayedMonth.Year - 1, 12, 1);
        }


        //TODO: Add create event methods based on the fields on event creation window

        public async Task LoadScheduleAsync()
        {
            Schedule = new Dictionary<DateTime, List<Event>>();

            var days = await Task.Run(() => GetCurrentlyDisplayedDays());

            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var repetetiveEvents = await Task.Run(() => unitOfWork.EventRepository.Find(x => x.RecurrencePattern != null).ToList());


                foreach (var t in days)
                {
                    var listOfEvents = new List<Event>();

                    //Finds all repetetive events that are going to happen on the day t
                    foreach (var k in repetetiveEvents)
                    {
                        if (k.IsDateTimeMatchingRecurrencePattern(t)) listOfEvents.Add(k);
                    }

                    // Finds all disposable events that are going to happen on the day t
                    listOfEvents.AddRange(await Task.Run(() => unitOfWork.EventRepository.Find(x => x.StartDateTime.Value.Date == t.Date).ToList()));

                    Schedule.Add(t, listOfEvents);
                }
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

            for (int i = 0; i < numberOfDaysFromPreviousMonth; i++)
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

            for (int i = 0; i < numberOfDaysFromNextMonth; i++)
            {
                listOfDays.Add(date);
                date.AddDays(1);
            }

            return listOfDays;
        }

        public async Task RemoveEventAsync(Event @event)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                unitOfWork.EventRepository.Remove(@event);
                unitOfWork.SaveChanges();
            }

            await LoadScheduleAsync();
        }

        public async Task CompleteEventAsync(Event @event)
        {
            if (@event == null) throw new ArgumentNullException();

            if (@event.RecurrencePattern == null)
            {
                @event.CompleteEvent(User);
                await RemoveEventAsync(@event);
            }
            else @event.CompleteEvent(User);
        }

        #region IObservable members
        public void AddObserver(IObserver observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            Observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var t in Observers) t.Update();
        }
        #endregion

        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

    }
}
