using Planner.Model;
using Planner.ViewModel.ViewModels;

namespace Planner.WPF.UserControls
{
    public class UserControlFactory : IUserControlFactory
    {
        public MonthCalendar GetMonthCalendar(IScheduleViewModel viewModel)
        {
            return new MonthCalendar(viewModel, this);
        }

        public EventUserControl GetEventUserControl(IScheduleViewModel viewModel, Event @event)
        {
            return new EventUserControl(viewModel, @event);
        }
    }
}
