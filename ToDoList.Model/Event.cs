using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ToDoList.Model
{
    public enum EventType
    {
        Obligatory,
        NotObligatory,
        MustDo
    }

    public enum EventDifficulty
    {
        Easy,
        Medium,
        Hard,
        VeryHard,
        NotPossible
    }

    [Table("Events")]
    public class Event
    {
        [Key()]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EventType EventType { get; set; }
        public EventDifficulty EventDifficulty { get; set; }
        public int Karma { get; set; }
        public DateTime ?StartDateTime { get; set; }
        public DateTime ?EndDateTime { get; set; }
        internal string _RecurrencePattern { get; set; }

        [NotMapped]
        public RecurrencePattern RecurrencePattern
        {
            get => _RecurrencePattern == null ? null : JsonSerializer.Deserialize<RecurrencePattern>(_RecurrencePattern);
            set => _RecurrencePattern = JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Creates disposable version of an event.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="eventType">Event type specified in <see cref="ToDoList.Model.EventType"/></param>
        /// <param name="eventDifficulty"/>Event difficulty specified in <see cref="ToDoList.Model.EventDifficulty"></param>
        /// <param name="dateTime">Date and time of an event</param>
        public Event(string name, EventType eventType, EventDifficulty eventDifficulty, DateTime dateTime)
        {
            Name = name;
            EventType = eventType;
            EventDifficulty = eventDifficulty;
            StartDateTime = dateTime;
        }

        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Creates disposable version of an event with it's ending DateTime.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="eventType">Event type specified in <see cref="ToDoList.Model.EventType"/></param>
        /// <param name="eventDifficulty">Event difficulty specified in <see cref="ToDoList.Model.EventDifficulty"/></param>
        /// <param name="dateTime">Starting date and time of the event</param>
        /// <param name="endDateTime">Ending date and time of the event</param>
        public Event(string name, EventType eventType, EventDifficulty eventDifficulty, DateTime dateTime, DateTime endDateTime)
        {
            Name = name;
            EventType = eventType;
            EventDifficulty = eventDifficulty;
            StartDateTime = dateTime;
            EndDateTime = endDateTime;
        }

        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Creates repetetive version of an event.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="eventType">Event type specified in <see cref="ToDoList.Model.EventType"/></param>
        /// <param name="eventDifficulty">Event difficulty specified in <see cref="ToDoList.Model.EventDifficulty"/></param>
        /// <param name="repetitionPattern">Repetition pattern specified in <see cref="ToDoList.Model.ReocurrencePatternType"/></param>
        public Event(string name, EventType eventType, EventDifficulty eventDifficulty, RecurrencePattern recurrencePattern)
        {
            Name = name;
            EventType = eventType;
            EventDifficulty = eventDifficulty;
            RecurrencePattern = recurrencePattern;
            StartDateTime = null;
            EndDateTime = null;
        }

        //TODO: Remove CompleteEvent()
        /// <summary>
        /// Completes the event.
        /// If event type is repetetive, returns the <see cref="Event"/> object with additional karma for event completion.
        /// If event type is not repetetive, returns null.
        /// </summary>
        /// <returns></returns>
        public Event CompleteEvent()
        {
            if (RecurrencePattern != null)
            {
                this.Karma += 5;
                return this;
            }
            else
            {
                return null;
            }
        }


        public bool IsDateTimeMatchingRecurrencePattern(DateTime dateTime)
        {
            if (System.DateTime.Compare(dateTime, StartDateTime.Value) < 0) return false;

            switch (RecurrencePattern.Type)
            {
                case RecurrencePatternType.Daily:
                    {
                        if (Math.Abs((StartDateTime - dateTime).Value.Days) % RecurrencePattern.Interval == 0) return true;
                    }
                    break;
                case RecurrencePatternType.Weekly:
                    {
                        var diff = Math.Abs((StartDateTime - dateTime).Value.Days) % (7 * RecurrencePattern.Interval);

                        if (diff == 0) return true;
                        
                        int dayOfWeek;
                        if ((int)dateTime.DayOfWeek == 0) dayOfWeek = 7;
                        else dayOfWeek = (int)dateTime.DayOfWeek;

                        if ((dayOfWeek - (7 * RecurrencePattern.Interval - diff) > 0) || (7 - dayOfWeek >= diff))
                        {
                            foreach (var t in RecurrencePattern.DaysOfWeek)
                            {
                                if ((int)t == (int)dateTime.DayOfWeek) return true;
                            }
                        }
                    }
                    break;
                case RecurrencePatternType.AbsoluteMonthly:
                    {
                        var diff = ((dateTime.Year - StartDateTime.Value.Year) * 12) + dateTime.Month - StartDateTime.Value.Month;

                        if (diff % RecurrencePattern.Interval == 0 && dateTime.Day == RecurrencePattern.DayOfMonth) return true;
                    }
                    break;
                case RecurrencePatternType.AbsoluteYearly:
                    {
                        var diff = dateTime.Year - StartDateTime.Value.Year;

                        if (diff % RecurrencePattern.Interval == 0 && (dateTime.Day == StartDateTime.Value.Day && dateTime.Month == StartDateTime.Value.Month)) return true;
                    }
                    break;
                case RecurrencePatternType.RelativeMonthly:
                    {
                        var diff = ((dateTime.Year - StartDateTime.Value.Year) * 12) + dateTime.Month - StartDateTime.Value.Month;

                        if (diff % RecurrencePattern.Interval == 0)
                        {
                            foreach(var t in RecurrencePattern.DaysOfWeek)
                            {
                                if ((int)t == (int)dateTime.DayOfWeek && (dateTime.Day - 1) / 7 == (int)RecurrencePattern.Index.Value) return true;
                            }
                        }
                    }
                    break;
                case RecurrencePatternType.RelativeYearly:
                    {
                        var diff = dateTime.Year - StartDateTime.Value.Year;

                        if(diff % RecurrencePattern.Interval == 0 && dateTime.Month == RecurrencePattern.Month)
                        {
                            foreach(var t in RecurrencePattern.DaysOfWeek)
                            {
                                if ((int)t == (int)dateTime.DayOfWeek) return true;
                            }
                        }
                    }
                    break;
            }

            return false;
        }
    }
}
