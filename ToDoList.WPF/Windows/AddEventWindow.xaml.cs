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
using Microsoft.Graph;
using DayOfWeek = Microsoft.Graph.DayOfWeek;

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

        private void Initialize()
        {
            EventTypeComboBox.Items.Add(ToDoList.WPF.Properties.Resources.obligatory);
            EventTypeComboBox.Items.Add(ToDoList.WPF.Properties.Resources.voluntary);
            EventTypeComboBox.Items.Add(ToDoList.WPF.Properties.Resources.mustDo);

            DifficultyComboBox.Items.Add(ToDoList.WPF.Properties.Resources.easy);
            DifficultyComboBox.Items.Add(ToDoList.WPF.Properties.Resources.medium);
            DifficultyComboBox.Items.Add(ToDoList.WPF.Properties.Resources.hard);
            DifficultyComboBox.Items.Add(ToDoList.WPF.Properties.Resources.veryHard);
            DifficultyComboBox.Items.Add(ToDoList.WPF.Properties.Resources.Impossible);

            this.StartDatePicker.Value = DateTime.Now;
            this.EndDatePicker.Value = DateTime.Now;

            this.RepeatComboBox.Items.Add(ToDoList.WPF.Properties.Resources.daily);
            this.RepeatComboBox.Items.Add(ToDoList.WPF.Properties.Resources.weekly);
            this.RepeatComboBox.Items.Add(ToDoList.WPF.Properties.Resources.monthly);
            this.RepeatComboBox.Items.Add(ToDoList.WPF.Properties.Resources.yearly);

            this.NeverRadioButton.IsChecked = true;

            this.RepeatEveryIntegerUpDown.Value = 0;
            this.AfterIntegerUpDown.Value = 0;

            DisableRecurrenceControls();
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

        private void RepeatComboBox_SelectionChanged(object sender, EventArgs eventArgs)
        {
            switch (RepeatComboBox.SelectedIndex)
            {
                case -1:
                    {
                        DisableRecurrenceControls();
                    }
                    break;
                case 0:
                    {
                        DisableRecurrenceControls();
                        EnableDailyRecurrenceControls();
                    }
                    break;
                case 1:
                    {
                        DisableRecurrenceControls();
                        EnableDailyRecurrenceControls();
                        EnableDaysOfWeekCheckBoxes();
                        RecurrenceTextBlock.Text = ToDoList.WPF.Properties.Resources.weeks;
                    }
                    break;
                case 2:
                    {
                        DisableRecurrenceControls();
                        EnableDailyRecurrenceControls();
                        EnableDaysOfWeekCheckBoxes();
                        WeekComboBox.IsEnabled = true;
                        RecurrenceTextBlock.Text = ToDoList.WPF.Properties.Resources.months;
                    }
                    break;
                case 3:
                    {
                        DisableRecurrenceControls();
                        EnableDailyRecurrenceControls();
                        EnableDaysOfWeekCheckBoxes();
                        WeekComboBox.IsEnabled = true;
                        MonthComboBox.IsEnabled = true;
                        RecurrenceTextBlock.Text = ToDoList.WPF.Properties.Resources.years;
                    }
                    break;
            }
        }

        private void DisableRecurrenceControls()
        {
                       
            RepeatEveryIntegerUpDown.IsEnabled = false;
            MondayCheckBox.IsEnabled = false;
            TuesdayCheckBox.IsEnabled = false;
            WednesdayCheckBox.IsEnabled = false;
            ThursdayCheckBox.IsEnabled = false;
            FridayCheckBox.IsEnabled = false;
            SaturdayCheckBox.IsEnabled = false;
            SundayCheckBox.IsEnabled = false;
            WeekComboBox.IsEnabled = false;
            MonthComboBox.IsEnabled = false;
            NeverRadioButton.IsEnabled = false;
            AfterRadioButton.IsEnabled = false;
            AfterIntegerUpDown.IsEnabled = false;
            RecurrenceTextBlock.Text = ToDoList.WPF.Properties.Resources.days;
        }

        private void EnableDailyRecurrenceControls()
        {
            RepeatEveryIntegerUpDown.IsEnabled = true;
            NeverRadioButton.IsEnabled = true;
            AfterRadioButton.IsEnabled = true;
            AfterIntegerUpDown.IsEnabled = true;
            RecurrenceTextBlock.Text = ToDoList.WPF.Properties.Resources.days;
        }

        private void EnableDaysOfWeekCheckBoxes()
        {
            MondayCheckBox.IsEnabled = true;
            TuesdayCheckBox.IsEnabled = true;
            WednesdayCheckBox.IsEnabled = true;
            ThursdayCheckBox.IsEnabled = true;
            FridayCheckBox.IsEnabled = true;
            SaturdayCheckBox.IsEnabled = true;
            SundayCheckBox.IsEnabled = true;
        }

        private void AllDayCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if(AllDayCheckBox.IsChecked.Value)
            {
                EndDatePicker.IsEnabled = false;
            }
            else
            {
                EndDatePicker.IsEnabled = true;
            }
        }

        /// <summary>
        /// Validates input and updates UI to let user know about invalid fields.
        /// </summary>
        /// <returns>Is input valid</returns>
        private bool ValidateInput()
        {
            return false;
        }

        private async void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            var name = NameTextBox.Text;
            var eventType = EventTypeComboBox.SelectedIndex;
            var difficulty = DifficultyComboBox.SelectedIndex;
            var startDateTime = StartDatePicker.Value;
            bool allDay =  AllDayCheckBox.IsChecked.Value;
            DateTime? endDateTime;

            if (AllDayCheckBox.IsChecked.Value) endDateTime = null;
            else endDateTime = EndDatePicker.Value;

            var recurrenceType = RepeatComboBox.SelectedIndex;
            var interval = RepeatEveryIntegerUpDown.Value;
            var listOfDays = new List<DayOfWeek>();

            if (MondayCheckBox.IsChecked.Value) listOfDays.Add(DayOfWeek.Monday);
            if (TuesdayCheckBox.IsChecked.Value) listOfDays.Add(DayOfWeek.Tuesday);
            if (WednesdayCheckBox.IsChecked.Value) listOfDays.Add(DayOfWeek.Wednesday);
            if (ThursdayCheckBox.IsChecked.Value) listOfDays.Add(DayOfWeek.Thursday);
            if (FridayCheckBox.IsChecked.Value) listOfDays.Add(DayOfWeek.Friday);
            if (SaturdayCheckBox.IsChecked.Value) listOfDays.Add(DayOfWeek.Saturday);
            if (SundayCheckBox.IsChecked.Value) listOfDays.Add(DayOfWeek.Sunday);

            var index = WeekComboBox.SelectedIndex;
            var month = MonthComboBox.SelectedIndex;
            
            int? occurrences = null;

            if (AfterRadioButton.IsChecked.Value) occurrences = AfterIntegerUpDown.Value;

            await (DataContext as IEventsCalendarViewModel).AddEventAsync(name, eventType, difficulty, startDateTime.Value, endDateTime, allDay, recurrenceType, interval.Value, listOfDays, index, month, occurrences);

            this.Close();
        }
    }
}
