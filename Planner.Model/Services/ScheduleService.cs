using Microsoft.Graph;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Services
{
    public class ScheduleService : IScheduleService
    {
        private string BaseURL = "https://localhost:5001/api/";
        private IUserService _userService;

        public ScheduleService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task AddEventAsync(Event @event)
        {
            var jsonEvent = new JavaScriptSerializer().Serialize(@event);
            var data = new StringContent(jsonEvent, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                var response = await client.PostAsync(BaseURL + "Events", data);

                string result = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(result);
            }
        }

        public async Task RemoveEventAsync(Event @event)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                var response = await client.DeleteAsync(BaseURL + "Events/" + @event.Id.ToString());

                string result = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(result);
            }
        }

        public async Task CompleteEventAsync(Event @event, int displayedDay, User user, Dictionary<DateTime, List<Event>> schedule)
        {
            if (@event.RecurrencePattern == null)
            {
                await RemoveEventAsync(@event);
            }
            else
            {
                var day = schedule.ElementAt(displayedDay).Key;
                @event.CompleteEvent(user, day);

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };


                using (var client = new HttpClient(clientHandler))
                {
                    var jsonEvent = new JavaScriptSerializer().Serialize(@event);

                    var content = new StringContent(jsonEvent, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(BaseURL + "Events/" + @event.Id.ToString(), content);

                    string result = response.Content.ReadAsStringAsync().Result;

                    Console.WriteLine(result);
                }
            }
        }

        public Event BuildEvent(string name, int eventType, int eventDifficulty, DateTime startDateTime, DateTime? endDateTime, bool allDay,
                int recurrenceType, int interval, List<Microsoft.Graph.DayOfWeek> daysOfWeek, int index, int month, int? occurrences)
        {
            RecurrencePattern recurrencePattern = null;

            switch (recurrenceType)
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

                        if (index == -1)
                        {
                            recurrencePattern.Type = RecurrencePatternType.AbsoluteYearly;
                        }
                        else
                        {
                            recurrencePattern.Type = RecurrencePatternType.RelativeYearly;
                            recurrencePattern.Index = (WeekIndex)index;
                            recurrencePattern.DaysOfWeek = daysOfWeek;
                            recurrencePattern.Month = month + 1;
                        }
                    }
                    break;
            }

            if (endDateTime == null && recurrencePattern == null)
            {
                return new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, allDay);
            }
            else if (endDateTime.HasValue && recurrencePattern == null)
            {
                return new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, endDateTime.Value);
            }
            else if (endDateTime == null && recurrencePattern != null && occurrences == null)
            {
                return new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, allDay, recurrencePattern);
            }
            else if (endDateTime.HasValue && recurrencePattern != null && occurrences == null)
            {
                return new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, endDateTime.Value, recurrencePattern);
            }
            else if (endDateTime == null && recurrencePattern != null && occurrences.HasValue)
            {
                return new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, allDay, recurrencePattern, occurrences.Value);
            }
            else if (endDateTime.HasValue && recurrencePattern != null && occurrences.HasValue)
            {
                return new Event(name, (EventType)eventType, (EventDifficulty)eventDifficulty, startDateTime, endDateTime.Value, recurrencePattern, occurrences.Value);
            }
            else return null;
        }

        public async Task<Dictionary<DateTime, List<Event>>> GetScheduleAsync(DateTime CurrentlyDisplayedDate)
        {
            var Schedule = new Dictionary<DateTime, List<Event>>();
            var days = await Task.Run(() => GetCurrentlyDisplayedDays(CurrentlyDisplayedDate));
            var events = new List<Event>();

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };


            using (var client = new HttpClient(clientHandler))
            {
                var response = await client.GetAsync(BaseURL + "Events");

                events = new JavaScriptSerializer().Deserialize<List<Event>>(response.Content.ReadAsStringAsync().Result);
            }

            var repetetiveEvents = await Task.Run(() => events.Where(x => x.RecurrencePattern != null).ToList());

            foreach (var t in days)
            {
                var listOfEvents = new List<Event>();

                //Finds all repetetive events that are going to happen on the day t
                foreach (var k in repetetiveEvents)
                {
                    if (k.IsDateTimeMatchingRecurrencePattern(t) && !k.DaysCompleted.Where(x => x.Date == t.Date).ToList().Any()) listOfEvents.Add(k);
                }

                // Finds all disposable events that are going to happen on the day t
                listOfEvents.AddRange(await Task.Run(() => events.Where(x => x.StartDateTime.Value.Date == t.Date && x.RecurrencePattern == null).ToList()));

                Schedule.Add(t, listOfEvents);
            }

            return Schedule;
        }

        private List<DateTime> GetCurrentlyDisplayedDays(DateTime CurrentlyDisplayedDate)
        {
            var listOfDays = new List<DateTime>();
            var date = new DateTime(CurrentlyDisplayedDate.Year, CurrentlyDisplayedDate.Month, 1);

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
    }
}
