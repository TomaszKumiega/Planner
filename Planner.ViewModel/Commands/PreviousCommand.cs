using System;
using System.Windows.Input;
using Planner.ViewModel.ViewModels;

namespace Planner.ViewModel.Commands
{
    public class PreviousCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly IScheduleViewModel _viewModel;

        public PreviousCommand(IScheduleViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            _viewModel.Previous();
            await _viewModel.LoadScheduleAsync();
            _viewModel.NotifyObservers();
        }
    }
}
