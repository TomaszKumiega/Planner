using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Model;
using ToDoList.ViewModel.ViewModels;

namespace ToDoList.WPF.UserControls
{
    public class UserControlFactory : IUserControlFactory
    {
        public MonthCalendar GetMonthCalendar(IEventsCalendarViewModel viewModel)
        {
            return new MonthCalendar(viewModel);
        }

        public EventUserControl GetEventUserControl(IEventsCalendarViewModel viewModel, Event @event)
        {
            return new EventUserControl(viewModel, @event);
        }
    }
}
