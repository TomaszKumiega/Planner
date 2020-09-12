using ToDoList.Model;
using ToDoList.ViewModel.ViewModels;

namespace ToDoList.WPF.UserControls
{
    public interface IUserControlFactory
    {
        EventUserControl GetEventUserControl(IEventsCalendarViewModel viewModel, Event @event);
        MonthCalendar GetMonthCalendar(IEventsCalendarViewModel viewModel);
    }
}