using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Planner.Model;
using Planner.Model.Repositories;
using Planner.ViewModel.Commands;
using Planner.ViewModel.ObserverPattern;
using DayOfWeek = Microsoft.Graph.DayOfWeek;
using Event = Planner.Model.Event;
using EventType = Planner.Model.EventType;
using User = Planner.Model.User;
using Planner.Model.Services;

namespace Planner.ViewModel.ViewModels
{
    public enum DisplayMode
    {
        Day,
        Week,
        Month,
        Year
    }

    public class ScheduleViewModel : IScheduleViewModel, INotifyPropertyChanged
    {
        private DateTime _currentlyDisplayedDate;
        public DisplayMode DisplayMode { get; set; }
        public User User { get; private set; }
        public Dictionary<DateTime, List<Event>> Schedule { get; private set; }
        public DateTime CurrentlySelectedDay { get; private set; }
        public List<IObserver> Observers { get; }
        public NextCommand NextCommand { get; }
        public PreviousCommand PreviousCommand { get; }

        private readonly IScheduleService _scheduleService;

        public DateTime CurrentlyDisplayedDate 
        { 
            get => _currentlyDisplayedDate;
            
            private set
            {
                _currentlyDisplayedDate = value;
                OnPropertyChanged("CurrentlyDisplayedMonth");
            }
        }


        public ScheduleViewModel(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
            DisplayMode = DisplayMode.Month;
            Observers = new List<IObserver>();
            CurrentlySelectedDay = DateTime.Now;
            CurrentlyDisplayedDate = DateTime.Now;            
            NextCommand = new NextCommand(this);
            PreviousCommand = new PreviousCommand(this);
            LoginUser();

            Task.Run(() => LoadScheduleAsync()).Wait();
        }

        private void LoginUser()
        {
            User = new User("Grewor", "Tomek", "Kumiega");
        }

        /// <summary>
        /// Changes <see cref="CurrentlyDisplayedDate"/> to the next month
        /// </summary>
        public void Next()
        {
            switch(DisplayMode)
            {
                case DisplayMode.Day: CurrentlyDisplayedDate.AddDays(1);
                    break;

                case DisplayMode.Week: CurrentlyDisplayedDate.AddDays(7);
                    break;

                case DisplayMode.Month:
                    CurrentlyDisplayedDate = CurrentlyDisplayedDate.AddMonths(1);
                    break;

                case DisplayMode.Year:
                    CurrentlyDisplayedDate.AddYears(1);
                    break;
            } 
        }

        /// <summary>
        /// Changes <see cref="CurrentlyDisplayedDate"/> to the previous month
        /// </summary>
        public void Previous()
        {
            switch (DisplayMode)
            {
                case DisplayMode.Day:
                    CurrentlyDisplayedDate.AddDays(-1);
                    break;

                case DisplayMode.Week:
                    CurrentlyDisplayedDate.AddDays(-7);
                    break;

                case DisplayMode.Month:
                    CurrentlyDisplayedDate = CurrentlyDisplayedDate.AddMonths(-1);
                    break;

                case DisplayMode.Year:
                    CurrentlyDisplayedDate.AddYears(-1);
                    break;
            }
        }


        public async Task AddEventAsync(string name, int eventType, int eventDifficulty, DateTime startDateTime, DateTime? endDateTime, bool allDay, int recurrenceType, int interval, List<DayOfWeek> daysOfWeek, int index, int month, int? occurrences)
        {
            var @event = await Task.Run(() => _scheduleService.BuildEvent(name, eventType, eventDifficulty, startDateTime, endDateTime, allDay, recurrenceType, interval,
                daysOfWeek, index, month, occurrences));
            await _scheduleService.AddEventAsync(@event);
            await LoadScheduleAsync();
        }

        public async Task LoadScheduleAsync()
        {
            Schedule = await _scheduleService.GetScheduleAsync(CurrentlyDisplayedDate);

            NotifyObservers();
        }

        public async Task RemoveEventAsync(Event @event)
        {
            await _scheduleService.RemoveEventAsync(@event);
            await LoadScheduleAsync();
        }

        public async Task CompleteEventAsync(Event @event, int displayedDay)
        {
            await _scheduleService.CompleteEventAsync(@event, displayedDay, User, Schedule);
            await LoadScheduleAsync();
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
