using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Model;
using ToDoList.ViewModel.Commands;
using ToDoList.ViewModel.ObserverPattern;

namespace ToDoList.ViewModel
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
    }
}