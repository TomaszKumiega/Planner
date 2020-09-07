using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public DateTime ?DateTime { get; set; }
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
            DateTime = dateTime;
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
            DateTime = dateTime;
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
            DateTime = null;
            EndDateTime = null;
        }

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

        public bool IsDateTimeMatchingRepetitionPattern(DateTime dateTime)
        {
            return false;
        }
    }
}
