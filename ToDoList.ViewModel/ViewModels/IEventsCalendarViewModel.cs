using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Model;
using ToDoList.ViewModel.Commands;
using ToDoList.ViewModel.ObserverPattern;
using Microsoft.Graph;
using DayOfWeek = Microsoft.Graph.DayOfWeek;
using Event = ToDoList.Model.Event;
using User = ToDoList.Model.User;

namespace ToDoList.ViewModel.ViewModels
{
    public interface IEventsCalendarViewModel : IObservable
    {
        DateTime CurrentlyDisplayedMonth { get; }
        DateTime CurrentlySelectedDay { get; }
        Dictionary<DateTime, List<Event>> Schedule { get; }
        User User { get; }
        NextMonthCommand NextMonthCommand { get; }
        PreviousMonthCommand PreviousMonthCommand { get; }

        Task CompleteEventAsync(Event @event);
        Task LoadScheduleAsync();
        void NextMonth();
        void PreviousMonth();
        Task RemoveEventAsync(Event @event);
        Task AddEventAsync(string name, int eventType, int eventDifficulty, DateTime startDateTime, DateTime ?endDateTime, bool allDay, int recurrenceType, int interval, List<DayOfWeek> daysOfWeek, int index, int month, int? occurrences);
    }
}