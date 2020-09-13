using System.Windows;
using System.Windows.Controls;
using Planner.Model;
using Planner.ViewModel.ViewModels;

namespace Planner.WPF.UserControls
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
            var column = Grid.GetColumn((this.Parent as ItemsControl).Parent as ScrollViewer);
            var row = Grid.GetRow((this.Parent as ItemsControl).Parent as ScrollViewer) -1;

            var day = (row * 7) + (column + 1) - 1;

            await _viewModel.CompleteEventAsync(DataContext as Event, day);
        }

        private async void RemoveEventButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.RemoveEventAsync(DataContext as Event);
        }
    }
}
