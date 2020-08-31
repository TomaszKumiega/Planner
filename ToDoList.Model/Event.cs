using System;
using System.Collections.Generic;
using System.Text;

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

    public enum RepetitionType
    {
        Daily,
        Weekly,
        AbsoluteMonthly,
        RelativeMonthly,
        AbsoluteYearly,
        RelativeYearly
    }

    /// <summary>
    /// Repetition pattern of an event
    /// </summary>
    public struct RepetitionPattern
    {
        public RepetitionType RepetitionType { get; set; }
        public int Interval { get; set; }
        public List<int> DaysOfWeek { get; set; }
        public List<int> DayOfMonth { get; set; }
        public List<int> WeekOfMonth { get; set; }
        public int Month { get; set; }
    }

    public class Event
    {
        public string Name { get; set; }
        public EventType EventType { get; set; }
        public EventDifficulty EventDifficulty { get; set; }
        public int Karma { get; private set; }
        public bool IsRepetetive { get; private set; }
        public DateTime DateTime { get; private set; }
        public RepetitionPattern RepetitionPattern { get; private set; }

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
            DateTime = dateTime;
        }

        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Creates repetetive version of an event.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="eventType">Event type specified in <see cref="ToDoList.Model.EventType"/></param>
        /// <param name="eventDifficulty">Event difficulty specified in <see cref="ToDoList.Model.EventDifficulty"/></param>
        /// <param name="repetitionPattern">Repetition pattern specified in <see cref="ToDoList.Model.RepetitionPattern"/></param>
        public Event(string name, EventType eventType, EventDifficulty eventDifficulty, RepetitionPattern repetitionPattern)
        {
            Name = name;
            EventType = eventType;
            EventDifficulty = eventDifficulty;
            RepetitionPattern = repetitionPattern;
        }
    }
}
