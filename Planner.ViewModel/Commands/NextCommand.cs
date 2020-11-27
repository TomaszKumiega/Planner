using System;
using System.Windows.Input;
using Planner.ViewModel.ViewModels;

namespace Planner.ViewModel.Commands
{
    public class NextCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly IScheduleViewModel _viewModel;

        public NextCommand(IScheduleViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            _viewModel.Next();
            await _viewModel.LoadScheduleAsync();
            _viewModel.NotifyObservers();
        }
    }
}
