using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoList.Model;
using ToDoList.ViewModel.ViewModels;

namespace ToDoList.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for EventUserControl.xaml
    /// </summary>
    public partial class EventUserControl : UserControl
    {
        private IEventsCalendarViewModel _viewModel;
        public EventUserControl(IEventsCalendarViewModel viewModel, Event @event)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = @event;
            SetBackground();
        }

        private void SetBackground()
        {
            //TODO
        }

        private async void CompleteEventButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.CompleteEventAsync(DataContext as Event);
        }

        private async void RemoveEventButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.RemoveEventAsync(DataContext as Event);
        }
    }
}
