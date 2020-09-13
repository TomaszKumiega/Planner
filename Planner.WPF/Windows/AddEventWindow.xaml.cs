using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Planner.ViewModel.ViewModels;
using DayOfWeek = Microsoft.Graph.DayOfWeek;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Planner.WPF.Windows
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
            EventTypeComboBox.Items.Add(Planner.WPF.Properties.Resources.obligatory);
            EventTypeComboBox.Items.Add(Planner.WPF.Properties.Resources.voluntary);
            EventTypeComboBox.Items.Add(Planner.WPF.Properties.Resources.mustDo);

            DifficultyComboBox.Items.Add(Planner.WPF.Properties.Resources.easy);
            DifficultyComboBox.Items.Add(Planner.WPF.Properties.Resources.medium);
            DifficultyComboBox.Items.Add(Planner.WPF.Properties.Resources.hard);
            DifficultyComboBox.Items.Add(Planner.WPF.Properties.Resources.veryHard);
            DifficultyComboBox.Items.Add(Planner.WPF.Properties.Resources.Impossible);

            this.StartDatePicker.Value = DateTime.Now;
            this.EndDatePicker.Value = DateTime.Now;

            this.RepeatComboBox.Items.Add(Planner.WPF.Properties.Resources.daily);
            this.RepeatComboBox.Items.Add(Planner.WPF.Properties.Resources.weekly);
            this.RepeatComboBox.Items.Add(Planner.WPF.Properties.Resources.monthly);
            this.RepeatComboBox.Items.Add(Planner.WPF.Properties.Resources.yearly);

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
                        RecurrenceTextBlock.Text = Planner.WPF.Properties.Resources.weeks;
                    }
                    break;
                case 2:
                    {
                        DisableRecurrenceControls();
                        EnableDailyRecurrenceControls();
                        EnableDaysOfWeekCheckBoxes();
                        WeekComboBox.IsEnabled = true;
                        RecurrenceTextBlock.Text = Planner.WPF.Properties.Resources.months;
                    }
                    break;
                case 3:
                    {
                        DisableRecurrenceControls();
                        EnableDailyRecurrenceControls();
                        EnableDaysOfWeekCheckBoxes();
                        WeekComboBox.IsEnabled = true;
                        MonthComboBox.IsEnabled = true;
                        RecurrenceTextBlock.Text = Planner.WPF.Properties.Resources.years;
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
            RecurrenceTextBlock.Text = Planner.WPF.Properties.Resources.days;
        }

        private void EnableDailyRecurrenceControls()
        {
            RepeatEveryIntegerUpDown.IsEnabled = true;
            NeverRadioButton.IsEnabled = true;
            AfterRadioButton.IsEnabled = true;
            AfterIntegerUpDown.IsEnabled = true;
            RecurrenceTextBlock.Text = Planner.WPF.Properties.Resources.days;
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
            bool isNameValid = true;
            bool isAfterValid = true;
            bool isEventTypeValid = true;
            bool isDifficultyValid = true;
            bool isStartDateValid = true;
            bool isEndDateValid = true;
            bool isIntervalValid = true;
            bool isWeekDaysValid = true;
            bool isIndexValid = true;
            bool isMonthValid = true;

            Dispatcher.Invoke(() =>
            {
                SetTextBlocksForegroundsToDefault();

                // Name
                var name = NameTextBox.Text;
                Regex regex = new Regex(@"^[A-Z0-9_-]+$", RegexOptions.IgnoreCase);

                if (String.IsNullOrWhiteSpace(name) || !regex.IsMatch(name))
                {
                    NameTextBlock.Foreground = Brushes.MediumOrchid;
                    isNameValid = false;
                }

                // Event Type
                var eventSelectedIndex = EventTypeComboBox.SelectedIndex;

                if (eventSelectedIndex == -1)
                {
                    EventTypeTextBlock.Foreground = Brushes.MediumOrchid;
                    isEventTypeValid = false;

                }

                // Difficulty
                var difficultySelectedIndex = DifficultyComboBox.SelectedIndex;

                if (difficultySelectedIndex == -1)
                {
                    DifficultyTextBlock.Foreground = Brushes.MediumOrchid;
                    isDifficultyValid = false;
                }

                // StartDate
                var startDate = StartDatePicker.Value;

                if (startDate.HasValue)
                {
                    if (DateTime.Compare(startDate.Value, DateTime.Now) < 0)
                    {
                        StartDateTextBlock.Foreground = Brushes.MediumOrchid;
                        isStartDateValid = false;
                    }
                }
                else isStartDateValid = false;

                // EndDate and allDay
                var endDate = EndDatePicker.Value;
                var allDay = AllDayCheckBox.IsChecked;

                if (!allDay.Value && !endDate.HasValue)
                {
                    EndDateTextBlock.Foreground = Brushes.MediumOrchid;
                    isEndDateValid = false;
                }
                else if (!allDay.Value && endDate.HasValue)
                {
                    if (DateTime.Compare(endDate.Value, DateTime.Now) < 0 || DateTime.Compare(endDate.Value, startDate.Value) < 0)
                    {
                        EndDateTextBlock.Foreground = Brushes.MediumOrchid;
                        isEndDateValid = false;
                    }
                }

                // Interval
                if (RepeatComboBox.SelectedIndex != -1 && RepeatEveryIntegerUpDown.Value < 1)
                {
                    RepeatEveryTextBlock.Foreground = Brushes.MediumOrchid;
                    isIntervalValid = false;
                }


                // WeekDays
                if (!MondayCheckBox.IsChecked.Value && !TuesdayCheckBox.IsChecked.Value && !WednesdayCheckBox.IsChecked.Value && !ThursdayCheckBox.IsChecked.Value &&
                    !FridayCheckBox.IsChecked.Value && !SaturdayCheckBox.IsChecked.Value && !SundayCheckBox.IsChecked.Value && RepeatComboBox.SelectedIndex > 0)
                {
                    OnTextBlock.Foreground = Brushes.MediumOrchid;
                    isWeekDaysValid = false;
                }

                // Index
                if (WeekComboBox.SelectedIndex == -1 && RepeatComboBox.SelectedIndex > 1)
                {
                    EveryTextBlock.Foreground = Brushes.MediumOrchid;
                    isIndexValid = false;
                }

                // Month
                if (MonthComboBox.SelectedIndex == -1 && RepeatComboBox.SelectedIndex > 2)
                {
                    OfTextBlock.Foreground = Brushes.MediumOrchid;
                    isMonthValid = false;
                }

                // After
                if (AfterRadioButton.IsChecked.Value && AfterIntegerUpDown.Value < 1)
                {
                    EndTextBlock.Foreground = Brushes.MediumOrchid;
                    isAfterValid = false;
                }

            });

            if (!(isNameValid && isEventTypeValid && isDifficultyValid && isStartDateValid && isEndDateValid && isIntervalValid && isWeekDaysValid
                && isIndexValid && isMonthValid && isAfterValid))
            {
                Dispatcher.Invoke(() =>
                {
                    InputInvalidTextBlock.Visibility = Visibility.Visible;
                });
                return false;
            }
            else return true;
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

            InputInvalidTextBlock.Visibility = Visibility.Hidden;
        }

        private void AddEvent()
        {
            if (!ValidateInput()) return;

            Dispatcher.Invoke(() =>
            {
                LoadIcon.Visibility = Visibility.Visible;

                var name = NameTextBox.Text;
                var eventType = EventTypeComboBox.SelectedIndex;
                var difficulty = DifficultyComboBox.SelectedIndex;
                var startDateTime = StartDatePicker.Value;
                bool allDay = AllDayCheckBox.IsChecked.Value;
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

                (DataContext as IEventsCalendarViewModel).AddEventAsync(name, eventType, difficulty, startDateTime.Value, endDateTime, allDay, recurrenceType, interval.Value, listOfDays, index, month, occurrences);

                this.Close();
            });

        }

        private async void FinishButton_Click(object sender, RoutedEventArgs e)
        {  
            await Task.Run(() => AddEvent());
            LoadIcon.Visibility = Visibility.Hidden;
        }
    }
}
