using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Planner.ViewModel.ObserverPattern;
using Planner.ViewModel.ViewModels;
using Planner.WPF.UserControls;

namespace Planner.WPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IObserver
    {
        private MonthCalendar MonthCalendar { get; }
        public MainWindow(IScheduleViewModel viewModel, IUserControlFactory controlFactory)
        {
            InitializeComponent();

            viewModel.AddObserver(this);
            DataContext = viewModel;
            MonthCalendar = controlFactory.GetMonthCalendar(viewModel);

            Initialize();
        }

        /// <summary>
        /// Allows to drag window with a mouse with its left button down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectangleTitleBarBackground_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// Minimizes the window.
        /// </summary>
        private void Minimize(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Maximizes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maximize(object sender, EventArgs e)
        {
            WindowState = (WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal);
        }

        /// <summary>
        /// Shuts down the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void AddEvent(object sender, EventArgs e)
        {
            var addEventWindow = new AddEventWindow(DataContext as IScheduleViewModel);
            this.IsEnabled = false;
            addEventWindow.Show();

            addEventWindow.Closed += (s, e) =>  this.IsEnabled = true;
        }

        /// <summary>
        /// Sets text property of MonthTextBlock
        /// </summary>
        private void SetMonth()
        {
            switch((DataContext as IScheduleViewModel).CurrentlyDisplayedMonth.Month)
            {
                case 1: 
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.january;
                    break;
                case 2:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.february;
                    break;
                case 3:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.march;
                    break;
                case 4:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.april;
                    break;
                case 5:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.may;
                    break;
                case 6:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.june;
                    break;
                case 7:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.july;
                    break;
                case 8:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.august;
                    break;
                case 9:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.september;
                    break;
                case 10:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.october;
                    break;
                case 11:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.november;
                    break;
                case 12:
                    MonthTextBlock.Text = Planner.WPF.Properties.Resources.december;
                    break;
            }
        }

        private void Initialize()
        {
            // Add MonthCalendar to the grid
            Grid.SetColumn(MonthCalendar, 0);
            Grid.SetRow(MonthCalendar, 4);
            Grid.SetColumnSpan(MonthCalendar, 2);
            RootGrid.Children.Add(MonthCalendar);

            // Set strings
            SetMonth();
        }

        public void Update()
        {
            SetMonth();
        }
    }
}
