using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planner.Model.Services
{
    public interface IScheduleService
    {
        Task AddEventAsync(Event @event);
        Event BuildEvent(string name, int eventType, int eventDifficulty, DateTime startDateTime, DateTime? endDateTime, bool allDay, int recurrenceType, int interval, List<Microsoft.Graph.DayOfWeek> daysOfWeek, int index, int month, int? occurrences);
        Task CompleteEventAsync(Event @event, int displayedDay, User user, Dictionary<DateTime, List<Event>> schedule);
        Task<Dictionary<DateTime, List<Event>>> GetScheduleAsync(DateTime CurrentlyDisplayedDate);
        Task RemoveEventAsync(Event @event);
    }
}