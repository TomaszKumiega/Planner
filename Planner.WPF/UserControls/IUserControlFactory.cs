using Planner.Model;
using Planner.ViewModel.ViewModels;

namespace Planner.WPF.UserControls
{
    public interface IUserControlFactory
    {
        EventUserControl GetEventUserControl(IScheduleViewModel viewModel, Event @event);
        MonthCalendar GetMonthCalendar(IScheduleViewModel viewModel);
    }
}