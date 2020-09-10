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
using System.Text.RegularExpressions;

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
            SetTextBlocksForegroundsToDefault();

            // Name
            bool isNameValid = true;
            var name = NameTextBox.Text;
            var listOfInvalidCharacters = new List<char>();
            Regex regex = new Regex(@"^[A-Z0-9_-]+$", RegexOptions.IgnoreCase);

            if (String.IsNullOrWhiteSpace(name) || !regex.IsMatch(name))
            {
                NameTextBlock.Foreground = Brushes.MediumOrchid;
                isNameValid = false;
            }

            // Event Type
            bool isEventTypeValid = true;
            var eventSelectedIndex = EventTypeComboBox.SelectedIndex;
            
            if(eventSelectedIndex == -1)
            {
                EventTypeTextBlock.Foreground = Brushes.MediumOrchid;
                isEventTypeValid = false;
            
            }

            // Difficulty
            bool isDifficultyValid = true;
            var difficultySelectedIndex = DifficultyComboBox.SelectedIndex;

            if(difficultySelectedIndex == -1)
            {
                DifficultyTextBlock.Foreground = Brushes.MediumOrchid;
                isDifficultyValid = false;
            }

            // StartDate
            bool isStartDateValid = true;
            var startDate = StartDatePicker.Value;

            if(startDate.HasValue)
            {
                if(DateTime.Compare(startDate.Value, DateTime.Now) < 0)
                {
                    StartDateTextBlock.Foreground = Brushes.MediumOrchid;
                    isStartDateValid = false;
                }
            }
            else isStartDateValid = false;

            // EndDate and allDay
            bool isEndDateValid = true;
            var endDate = EndDatePicker.Value;
            var allDay = AllDayCheckBox.IsChecked;

            if(!allDay.Value && !endDate.HasValue)
            {
                EndDateTextBlock.Foreground = Brushes.MediumOrchid;
                isEndDateValid = false;
            }
            else if(!allDay.Value && endDate.HasValue)
            {
                if (DateTime.Compare(endDate.Value, DateTime.Now) < 0 || DateTime.Compare(endDate.Value, startDate.Value) < 0)
                {
                    EndDateTextBlock.Foreground = Brushes.MediumOrchid;
                    isEndDateValid = false;
                }
            }

            // Interval
            bool isIntervalValid = true;

            if(RepeatComboBox.SelectedIndex != -1 && RepeatEveryIntegerUpDown.Value < 1)
            {
                RepeatEveryTextBlock.Foreground = Brushes.MediumOrchid;
                isIntervalValid = false;
            }


            // WeekDays
            bool isWeekDaysValid = true;

            if(!MondayCheckBox.IsChecked.Value && !TuesdayCheckBox.IsChecked.Value && !WednesdayCheckBox.IsChecked.Value && !ThursdayCheckBox.IsChecked.Value &&
                !FridayCheckBox.IsChecked.Value && !SaturdayCheckBox.IsChecked.Value && !SundayCheckBox.IsChecked.Value && RepeatComboBox.SelectedIndex > 0)
            {
                OnTextBlock.Foreground = Brushes.MediumOrchid;
                isWeekDaysValid = false;
            }

            // Index
            bool isIndexValid = true;

            if(WeekComboBox.SelectedIndex == -1 && RepeatComboBox.SelectedIndex > 1)
            {
                EveryTextBlock.Foreground = Brushes.MediumOrchid;
                isIndexValid = false;
            }

            // Month
            bool isMonthValid = true;

            if(MonthComboBox.SelectedIndex == -1 && RepeatComboBox.SelectedIndex > 2)
            {
                OfTextBlock.Foreground = Brushes.MediumOrchid;
                isMonthValid = false;
            }

            // After
            bool isAfterValid = true;

            if(AfterRadioButton.IsChecked.Value && AfterIntegerUpDown.Value < 1)
            {
                EndTextBlock.Foreground = Brushes.MediumOrchid;
                isAfterValid = false;
            }


            return isNameValid && isEventTypeValid && isDifficultyValid && isStartDateValid && isEndDateValid && isIntervalValid && isWeekDaysValid
                && isIndexValid && isMonthValid && isAfterValid;
        }

        private void SetTextBlocksForegroundsToDefault()
        {
            NameTextBlock.Foreground = Brushes.WhiteSmoke;
            EventTypeTextBlock.Foreground = Brushes.WhiteSmoke;
            DifficultyTextBlock.Foreground = Brushes.WhiteSmoke;
            StartDateTextBlock.Foreground = Brushes.WhiteSmoke;
            EndDateTextBlock.Foreground = Brushes.WhiteSmoke;
            RepeatTextBlock.Foreground = Brushes.WhiteSmoke;
            OnTextBlock.Foreground = Brushes.WhiteSmoke;
            EveryTextBlock.Foreground = Brushes.WhiteSmoke;
            RepeatEveryTextBlock.Foreground = Brushes.WhiteSmoke;
            OfTextBlock.Foreground = Brushes.WhiteSmoke;
            EndTextBlock.Foreground = Brushes.WhiteSmoke;
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
