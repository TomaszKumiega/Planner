using Microsoft.Graph;
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
using DayOfWeek = Microsoft.Graph.DayOfWeek;
using Event = ToDoList.Model.Event;
using EventType = ToDoList.Model.EventType;
using User = ToDoList.Model.User;

namespace ToDoList.ViewModel.ViewModels
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
            Task.Run(() => LoadScheduleAsync()).Wait();
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


        public async Task AddEventAsync(string name, int eventType, int eventDifficulty, DateTime startDateTime, DateTime? endDateTime, bool allDay, int recurrenceType, int interval, List<DayOfWeek> daysOfWeek, int index, int month, int? occurrences)
        {
            RecurrencePattern recurrencePattern = null;
            Event @event = null;

            switch(recurrenceType)
            {
                case -1:
                    break;
                case 0:
                    {
                        recurrencePattern = new RecurrencePattern();
                        recurrencePattern.Interval = interval;
                        recurrencePattern.Type = RecurrencePatternType.Daily;
                    }
                    break;
                case 1:
                    {
                        recurrencePattern = new RecurrencePattern();
                        recurrencePattern.Type = RecurrencePatternType.Weekly;
                        recurrencePattern.Interval = interval;
                        recurrencePattern.DaysOfWeek = daysOfWeek;
                    }
                    break;
                case 2:
                    {
                        recurrencePattern = new RecurrencePattern();
                        recurrencePattern.Interval = interval;
                       
                        if (index == -1)
                        {
                            recurrencePattern.Type = RecurrencePatternType.AbsoluteMonthly;
                        }
                        else
                        {
                            recurrencePattern.Type = RecurrencePatternType.RelativeMonthly;
                            recurrencePattern.Index = (WeekIndex)index;
                            recurrencePattern.DaysOfWeek = daysOfWeek;
                        }
                    }
                    break;
                case 3:
                    {
                        recurrencePattern = new RecurrencePattern();
                        recurrencePattern.Interval = interval;
                        
                        if(index ==-1)
                        {
                            recurrencePattern.Type = RecurrencePatternType.AbsoluteYearly;
                        }
                        else
                        {
                            recurrencePattern.Type = RecurrencePatternType.RelativeYearly;
                            recurrencePattern.Index = (WeekIndex)index;
                            recurrencePattern.DaysOfWeek = daysOfWeek;
                            recurrencePattern.Month = month+1;
                        }
                    }
                    break;
            }

            if(endDateTime == null && recurrencePattern == null)
            {
                @event = new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, allDay);
            }
            else if(endDateTime.HasValue  && recurrencePattern == null)
            {
                @event = new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, endDateTime.Value);
            }
            else if(endDateTime == null && recurrencePattern != null && occurrences == null)
            {
                @event = new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, allDay, recurrencePattern);
            }
            else if(endDateTime.HasValue && recurrencePattern != null && occurrences == null)
            {
                @event = new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, endDateTime.Value, recurrencePattern);
            }
            else if(endDateTime == null && recurrencePattern != null && occurrences.HasValue)
            {
                @event = new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, allDay, recurrencePattern, occurrences.Value);
            }
            else if(endDateTime.HasValue && recurrencePattern != null && occurrences.HasValue)
            {
                @event = new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, endDateTime.Value, recurrencePattern, occurrences.Value);
            }

            using(var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                unitOfWork.EventRepository.Add(@event);
                unitOfWork.SaveChanges();
            }

            await LoadScheduleAsync();

        }

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

            NotifyObservers();
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

            date = date.AddDays(-numberOfDaysFromPreviousMonth);

            for (int i = 0; i < numberOfDaysFromPreviousMonth; i++)
            {
                listOfDays.Add(date);
                date = date.AddDays(1);
            }

            // Generate list of days from currently displayed month
            listOfDays.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month))
                .Select(day => new DateTime(date.Year, date.Month, day))
                .ToList());

            // Generate days from next month to fill a gap at the end of the calendar
            date = date.AddMonths(1);

            var numberOfDaysFromNextMonth = 35 - listOfDays.Count;

            for (int i = 0; i < numberOfDaysFromNextMonth; i++)
            {
                listOfDays.Add(date);
                date = date.AddDays(1);
            }

            return listOfDays;
        }

        public async Task RemoveEventAsync(Event @event)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var events = unitOfWork.EventRepository.Find(x => x.Id.Equals(@event.Id)).ToList();
                if (events.Any())
                {
                    var actualEvent = events[0];
                    unitOfWork.EventRepository.Remove(actualEvent);
                    unitOfWork.SaveChanges();
                }
                else return;
            }

            await LoadScheduleAsync();
        }

        public async Task CompleteEventAsync(Event @event)
        {
            if (@event == null) throw new ArgumentNullException();

            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var events = unitOfWork.EventRepository.Find(x => x.Id == @event.Id).ToList();
                
                if (events.Any())
                {
                    var actualEvent = events[0];

                    if (actualEvent.RecurrencePattern == null)
                    {
                        actualEvent.CompleteEvent(User);
                        await RemoveEventAsync(actualEvent);
                    }
                    else
                    {
                        actualEvent.CompleteEvent(User);
                        await LoadScheduleAsync();
                    }
                }
            }
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
