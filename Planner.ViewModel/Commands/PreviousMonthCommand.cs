using System;
using System.Windows.Input;
using Planner.ViewModel.ViewModels;

namespace Planner.ViewModel.Commands
{
    public class PreviousMonthCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly IEventsCalendarViewModel _viewModel;

        public PreviousMonthCommand(IEventsCalendarViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            _viewModel.PreviousMonth();
            await _viewModel.LoadScheduleAsync();
            _viewModel.NotifyObservers();
        }
    }
}
