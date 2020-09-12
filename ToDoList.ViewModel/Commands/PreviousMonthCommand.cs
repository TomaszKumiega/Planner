using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ToDoList.ViewModel.ViewModels;

namespace ToDoList.ViewModel.Commands
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
