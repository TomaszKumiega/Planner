using System;
using System.Collections.Generic;
using System.Text;
using Planner.Model;
using Planner.ViewModel.ViewModels;

namespace Planner.WPF.UserControls
{
    public class UserControlFactory : IUserControlFactory
    {
        public MonthCalendar GetMonthCalendar(IEventsCalendarViewModel viewModel)
        {
            return new MonthCalendar(viewModel, this);
        }

        public EventUserControl GetEventUserControl(IEventsCalendarViewModel viewModel, Event @event)
        {
            return new EventUserControl(viewModel, @event);
        }
    }
}
