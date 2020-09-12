using Planner.Model;
using Planner.ViewModel.ViewModels;

namespace Planner.WPF.UserControls
{
    public interface IUserControlFactory
    {
        EventUserControl GetEventUserControl(IEventsCalendarViewModel viewModel, Event @event);
        MonthCalendar GetMonthCalendar(IEventsCalendarViewModel viewModel);
    }
}