using Microsoft.Graph;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using Xunit;

namespace Planner.Model
{
    public class EventTests
    {
        [Fact]
        public void DaysCompleted_ShouldBeAbleToRetrieveAddedValue()
        {
            var @event = new Event();
            var daysList = new List<DateTime>();
            var dateTime = DateTime.Now;
            daysList.Add(dateTime);

            @event.DaysCompleted = daysList;

            Assert.True(@event.DaysCompleted[0].Equals(dateTime));
        }

        [Fact]
        public void RecurrencePattern_ShouldBeAbleToRetrieveAddedValue()
        {
            var @event = new Event();
            var recurrencePattern = new RecurrencePattern();
            var interval = 1553;
            recurrencePattern.Interval = interval;

            @event.RecurrencePattern = recurrencePattern;

            Assert.True(@event.RecurrencePattern.Interval == interval);
        }

        [Fact]
        public void CompleteEvent_ShouldAddKarmaToUser()
        {
            var @event = new Event("name", EventType.Voluntary, EventDifficulty.Hard, DateTime.Now, true);
            var user = new User();
            user.Karma = 0;

            @event.CompleteEvent(user);

            Assert.True(user.Karma > 0);
        }

        [Fact]
        public void CompleteEvent_ShouldDecreseNumberOfOcurrences_WhenEventIsRecurringAndNumberOfOccurrencesIsntNull()
        {
            var numberOfOcurrences = 5;
            var @event = new Event("name", EventType.Voluntary, EventDifficulty.Hard, DateTime.Now, DateTime.Now, new RecurrencePattern(), numberOfOcurrences);
            var user = new User();

            @event.CompleteEvent(user);

            Assert.True(@event.NumberOfOccurrences < numberOfOcurrences);
        }

        [Fact]
        public void CompleteEvent_ShouldAddBonusToCompletionKarma()
        {
            var @event = new Event("name", EventType.Voluntary, EventDifficulty.Hard, DateTime.Now, DateTime.Now, new RecurrencePattern());
            var completionKarma = @event.CompletionKarma;
            var user = new User();

            @event.CompleteEvent(user);

            Assert.True(@event.CompletionKarma > completionKarma);
        }

        [Fact]
        public void CompleteEvent_ShouldAddDateToDaysCompletedList()
        {
            var @event = new Event();
            var dateTime = DateTime.Now;
            var user = new User();

            @event.CompleteEvent(user, dateTime);

            Assert.Contains(dateTime, @event.DaysCompleted);
        }
    }
}
