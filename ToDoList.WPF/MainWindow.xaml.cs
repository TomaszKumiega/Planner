using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoList.ViewModel;
using ToDoList.ViewModel.ObserverPattern;
using ToDoList.WPF;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IObserver
    {
        private MonthCalendar MonthCalendar { get; }
        public MainWindow(IEventsCalendarViewModel viewModel, MonthCalendar monthCalendar)
        {
            InitializeComponent();
            viewModel.AddObserver(this);
            DataContext = viewModel;
            MonthCalendar = monthCalendar;
            Grid.SetColumn(MonthCalendar, 0);
            Grid.SetRow(MonthCalendar, 4);
            Grid.SetColumnSpan(MonthCalendar, 2);
            RootGrid.Children.Add(MonthCalendar);
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

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
