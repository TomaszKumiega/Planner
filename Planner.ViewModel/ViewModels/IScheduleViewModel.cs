using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Planner.ViewModel.Commands;
using Planner.ViewModel.ObserverPattern;
using DayOfWeek = Microsoft.Graph.DayOfWeek;
using Event = Planner.Model.Event;
using User = Planner.Model.User;

namespace Planner.ViewModel.ViewModels
{
    public interface IScheduleViewModel : IObservable
    {
        DateTime CurrentlyDisplayedMonth { get; }
        DateTime CurrentlySelectedDay { get; }
        Dictionary<DateTime, List<Event>> Schedule { get; }
        User User { get; }
        NextCommand NextCommand { get; }
        PreviousCommand PreviousCommand { get; }

        Task CompleteEventAsync(Event @event, int displayedDay);
        Task LoadScheduleAsync();
        void Next();
        void Previous();
        Task RemoveEventAsync(Event @event);
        Task AddEventAsync(string name, int eventType, int eventDifficulty, DateTime startDateTime, DateTime ?endDateTime, bool allDay, int recurrenceType, int interval, List<DayOfWeek> daysOfWeek, int index, int month, int? occurrences);
    }
}