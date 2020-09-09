using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ToDoList.ViewModel.ViewModels;

namespace ToDoList.ViewModel.Commands
{
    public class NextMonthCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly IEventsCalendarViewModel _viewModel;

        public NextMonthCommand(IEventsCalendarViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.NextMonth();
            _viewModel.LoadScheduleAsync();
            _viewModel.NotifyObservers();
        }
    }
}
