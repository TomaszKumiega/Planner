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
    }
}
