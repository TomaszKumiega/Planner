using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planner.Model.Services
{
    public class ScheduleService
    {
        public List<Event> Events { get; private set; }

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
    }
}
