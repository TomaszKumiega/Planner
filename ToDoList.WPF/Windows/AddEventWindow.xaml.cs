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
using System.Windows.Shapes;
using ToDoList.ViewModel.ViewModels;

namespace ToDoList.WPF.Windows
{
    /// <summary>
    /// Interaction logic for AddEventWindow.xaml
    /// </summary>
    public partial class AddEventWindow : Window
    {
        public AddEventWindow(IEventsCalendarViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Initialize();
        }


        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, EventArgs e)
        {
            this.Close();
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

        private void Initialize()
        {
            this.StartDatePicker.Value = DateTime.Now;
            this.EndDatePicker.Value = DateTime.Now;

            this.RepeatComboBox.Items.Add("Daily");
            this.RepeatComboBox.Items.Add("Weekly");
            this.RepeatComboBox.Items.Add("Monthly");
            this.RepeatComboBox.Items.Add("Yearly");

        }
    }
}
