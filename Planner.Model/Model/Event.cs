using Microsoft.Graph;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Model
{
    public enum EventType
    {
        Obligatory,
        Voluntary,
        MustDo
    }

    public enum EventDifficulty
    {
        Easy,
        Medium,
        Hard,
        VeryHard,
        Impossible
    }

    /// <summary>
    /// Describes User's event displayed on the planner
    /// </summary>
    [Table("Events")]
    public class Event
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        [Key()]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public EventType EventType { get; set; }
        public EventDifficulty EventDifficulty { get; set; }
        public int CompletionKarma { get; set; }
        public int FailureKarma { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool AllDay { get; set; }
        public int? NumberOfOccurrences { get; set; }
        
        internal string _RecurrencePattern { get; set; }
        internal string _DaysCompleted { get; set; }

        #region Serialize Complex Properties
        [NotMapped]
        public List<DateTime> DaysCompleted
        {
            get
            {
                var utcDates = _DaysCompleted == null ? null : new JavaScriptSerializer().Deserialize<List<DateTime>>(_DaysCompleted);
                var actualDates = new List<DateTime>();

                foreach(var t in utcDates)
                {
                    actualDates.Add(t.ToLocalTime());
                }

                return actualDates;
            }
            set
            {
                var localDates = value;
                var utcDates = new List<DateTime>();

                foreach(var t in localDates)
                {
                    utcDates.Add(t.ToUniversalTime());
                }

                _DaysCompleted = new JavaScriptSerializer().Serialize(utcDates);
            }
        }

        [NotMapped]
        public RecurrencePattern RecurrencePattern
        {
            get => _RecurrencePattern == null ? null : new JavaScriptSerializer().Deserialize<RecurrencePattern>(_RecurrencePattern);
            set => _RecurrencePattern = new JavaScriptSerializer().Serialize(value);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Constructor for entity framework usage
        /// </summary>
        public Event()
        {
            DaysCompleted = new List<DateTime>();
        }

        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Creates disposable version of an event.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="eventType">Event type specified in <see cref="Planner.Model.EventType"/></param>
        /// <param name="eventDifficulty"/>Event difficulty specified in <see cref="Planner.Model.EventDifficulty"></param>
        /// <param name="startDateTime">Date and time of an event</param>
        public Event(string name, EventType eventType, EventDifficulty eventDifficulty, DateTime startDateTime, bool allDay, Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            EventType = eventType;
            EventDifficulty = eventDifficulty;
            StartDateTime = startDateTime;
            EndDateTime = null;
            AllDay = allDay;
            _RecurrencePattern = null;
            DaysCompleted = new List<DateTime>();
            InitializeKarma();
            _logger.Debug("Event: " + this.ToString() + " created");
        }

        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Creates disposable version of an event with it's ending DateTime.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="eventType">Event type specified in <see cref="Planner.Model.EventType"/></param>
        /// <param name="eventDifficulty">Event difficulty specified in <see cref="Planner.Model.EventDifficulty"/></param>
        /// <param name="startDateTime">Starting date and time of the event</param>
        /// <param name="endDateTime">Ending date and time of the event</param>
        public Event(string name, EventType eventType, EventDifficulty eventDifficulty, DateTime startDateTime, DateTime endDateTime, Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            EventType = eventType;
            EventDifficulty = eventDifficulty;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            AllDay = false;
            DaysCompleted = new List<DateTime>();
            InitializeKarma();
            _logger.Debug("Event: " + this.ToString() + " created");
        }

        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Creates recurring event.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="eventType">Event type specified in <see cref="Planner.Model.EventType"/></param>
        /// <param name="eventDifficulty">Event difficulty specified in <see cref="Planner.Model.EventDifficulty"/></param>
        /// <param name="startDateTime">Starting date and time of the event</param>
        /// <param name="allDay">Specifies if event should ignore startDateTime's time component</param>
        /// <param name="recurrencePattern">Specifies when event should recur</param>
        public Event(string name, EventType eventType, EventDifficulty eventDifficulty, DateTime startDateTime, bool allDay, RecurrencePattern recurrencePattern, Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            EventType = eventType;
            EventDifficulty = eventDifficulty;
            RecurrencePattern = recurrencePattern;
            StartDateTime = startDateTime;
            AllDay = allDay;
            EndDateTime = null;
            NumberOfOccurrences = null;
            DaysCompleted = new List<DateTime>();
            InitializeKarma();
            _logger.Debug("Event: " + this.ToString() + " created");
        }

        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Creates recurring event.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="eventType">Event type specified in <see cref="Planner.Model.EventType"/></param>
        /// <param name="eventDifficulty">Event difficulty specified in <see cref="Planner.Model.EventDifficulty"/></param>
        /// <param name="startDateTime">Starting date and time of the event</param>
        /// <param name="endDateTime">Ending date and time of the event</param>
        /// <param name="recurrencePattern">/// <param name="recurrencePattern">Specifies when event should recur</param></param>
        public Event(string name, EventType eventType, EventDifficulty eventDifficulty, DateTime startDateTime, DateTime endDateTime, RecurrencePattern recurrencePattern, Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            EventType = eventType;
            EventDifficulty = eventDifficulty;
            RecurrencePattern = recurrencePattern;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            AllDay = false;
            NumberOfOccurrences = null;
            DaysCompleted = new List<DateTime>();
            InitializeKarma();
            _logger.Debug("Event: " + this.ToString() + " created");
        }

        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Creates repetetive version of an event.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="eventType">Event type specified in <see cref="Planner.Model.EventType"/></param>
        /// <param name="eventDifficulty">Event difficulty specified in <see cref="Planner.Model.EventDifficulty"/></param>
        /// <param name="startDateTime">Starting date and time of the event</param>
        /// <param name="allDay">Specifies if event should ignore startDateTime's time component</param>
        /// <param name="recurrencePattern">Specifies when event should recur</param>
        /// <param name="numberOfOccurrences">Number of ocurrences of the event</param>
        public Event(string name, EventType eventType, EventDifficulty eventDifficulty, DateTime startDateTime, bool allDay, 
            RecurrencePattern recurrencePattern, int numberOfOccurrences, Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            EventType = eventType;
            EventDifficulty = eventDifficulty;
            RecurrencePattern = recurrencePattern;
            StartDateTime = startDateTime;
            AllDay = allDay;
            EndDateTime = null;
            NumberOfOccurrences = numberOfOccurrences;
            DaysCompleted = new List<DateTime>();
            InitializeKarma();
            _logger.Debug("Event: " + this.ToString() + " created");
        }

        /// <summary>
        /// Initializes new instance of <see cref="Event"/> class.
        /// Creates repetetive version of an event.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="eventType">Event type specified in <see cref="Planner.Model.EventType"/></param>
        /// <param name="eventDifficulty">Event difficulty specified in <see cref="Planner.Model.EventDifficulty"/></param>
        /// <param name="startDateTime">Starting date and time of the event</param>
        /// <param name="endDateTime">Ending date and time of the event</param>
        /// <param name="recurrencePattern">Specifies when event should recur</param>
        /// <param name="numberOfOccurrences">Number of ocurrences of the event</param>
        public Event(string name, EventType eventType, EventDifficulty eventDifficulty, DateTime startDateTime, DateTime endDateTime,
            RecurrencePattern recurrencePattern, int numberOfOccurrences, Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            EventType = eventType;
            EventDifficulty = eventDifficulty;
            RecurrencePattern = recurrencePattern;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            AllDay = false;
            NumberOfOccurrences = numberOfOccurrences;
            DaysCompleted = new List<DateTime>();
            InitializeKarma();
            _logger.Debug("Event: " + this.ToString() + " created");
        }
        #endregion

        private void AddCompletedDay(DateTime day)
        {
            var list = DaysCompleted;
            list.Add(day);
            DaysCompleted = list;
        }

        private void InitializeKarma()
        {
            switch (EventType)
            {
                case EventType.Obligatory:
                    {
                        FailureKarma = Int32.Parse(KarmaEconomyResources.FailureObligatory);

                        switch (EventDifficulty)
                        {
                            case EventDifficulty.Easy:
                                CompletionKarma = Int32.Parse(KarmaEconomyResources.CompletionEasy);
                                break;
                            case EventDifficulty.Medium:
                                CompletionKarma = Int32.Parse(KarmaEconomyResources.CompletionMedium);
                                break;
                            case EventDifficulty.Hard:
                                CompletionKarma = Int32.Parse(KarmaEconomyResources.CompletionHard);
                                break;
                            case EventDifficulty.VeryHard:
                                CompletionKarma = Int32.Parse(KarmaEconomyResources.CompletionVeryHard);
                                break;
                            case EventDifficulty.Impossible:
                                CompletionKarma = Int32.Parse(KarmaEconomyResources.CompletionImpossible);
                                break;
                        }

                    }
                    break;
                case EventType.Voluntary:
                    {
                        FailureKarma = 0;

                        switch (EventDifficulty)
                        {
                            case EventDifficulty.Easy:
                                CompletionKarma = (int)(Int32.Parse(KarmaEconomyResources.CompletionEasy) / 1.5) * 2;
                                break;
                            case EventDifficulty.Medium:
                                CompletionKarma = (int)(Int32.Parse(KarmaEconomyResources.CompletionMedium) / 1.5) * 2;
                                break;
                            case EventDifficulty.Hard:
                                CompletionKarma = (int)(Int32.Parse(KarmaEconomyResources.CompletionHard) / 1.5) * 2;
                                break;
                            case EventDifficulty.VeryHard:
                                CompletionKarma = (int)(Int32.Parse(KarmaEconomyResources.CompletionVeryHard) / 1.5) * 2;
                                break;
                            case EventDifficulty.Impossible:
                                CompletionKarma = (int)(Int32.Parse(KarmaEconomyResources.CompletionImpossible) / 1.5) * 2;
                                break;
                        }
                    }
                    break;
                case EventType.MustDo:
                    {
                        FailureKarma = Int32.Parse(KarmaEconomyResources.FailureMustDo);

                        switch (EventDifficulty)
                        {
                            case EventDifficulty.Easy:
                                CompletionKarma = 0;
                                break;
                            case EventDifficulty.Medium:
                                CompletionKarma = Int32.Parse(KarmaEconomyResources.CompletionMustDo);
                                break;
                            case EventDifficulty.Hard:
                                CompletionKarma = Int32.Parse(KarmaEconomyResources.CompletionMustDo) * 2;
                                break;
                            case EventDifficulty.VeryHard:
                                CompletionKarma = Int32.Parse(KarmaEconomyResources.CompletionMustDo) * 4;
                                break;
                            case EventDifficulty.Impossible:
                                CompletionKarma = Int32.Parse(KarmaEconomyResources.CompletionMustDo) * 8;
                                break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Adds completion karma to <see cref="UserModel.Karma"/> and adds karma bonus to <see cref="CompletionKarma"/> if event is reccuring.
        /// </summary>
        /// <param name="user"></param>
        public void CompleteEvent(UserModel user)
        {
            user.Karma += CompletionKarma;

            if (NumberOfOccurrences.HasValue) NumberOfOccurrences--;

            if (RecurrencePattern != null)
            {
                switch (EventType)
                {
                    case EventType.Obligatory:
                        CompletionKarma += Int32.Parse(KarmaEconomyResources.CompletionObligatoryRecurringBonus);
                        break;
                    case EventType.Voluntary:
                        CompletionKarma += Int32.Parse(KarmaEconomyResources.CompletionVoluntaryRecurringBonus);
                        break;
                    case EventType.MustDo:
                        CompletionKarma += Int32.Parse(KarmaEconomyResources.CompletionMustDoRecurringBonus);
                        break;
                }

            }

            _logger.Info("Event: " + Id.ToString() + " completed");
        }

        /// <summary>
        /// Adds dateTime to <see cref="DaysCompleted"/> collection. 
        /// Adds completion karma to <see cref="UserModel.Karma"/> and adds karma bonus to <see cref="CompletionKarma"/> if event is recurring.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dateTime"></param>
        public void CompleteEvent(UserModel user, DateTime dateTime)
        {
            AddCompletedDay(dateTime);
            CompleteEvent(user);
        }

        /// <summary>
        /// Checks if <see cref="DateTime"/> is matching <see cref="RecurrencePattern"/> of the event.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
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
                            foreach (var t in RecurrencePattern.DaysOfWeek)
                            {
                                if ((int)t == (int)dateTime.DayOfWeek && (dateTime.Day - 1) / 7 == (int)RecurrencePattern.Index.Value) return true;
                            }
                        }
                    }
                    break;
                case RecurrencePatternType.RelativeYearly:
                    {
                        var diff = dateTime.Year - StartDateTime.Value.Year;

                        if (diff % RecurrencePattern.Interval == 0 && dateTime.Month == RecurrencePattern.Month)
                        {
                            foreach (var t in RecurrencePattern.DaysOfWeek)
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
