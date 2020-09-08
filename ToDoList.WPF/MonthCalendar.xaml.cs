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
using ToDoList.ViewModel.ObserverPattern;

namespace ToDoList.WPF
{
    /// <summary>
    /// Interaction logic for MonthCalendar.xaml
    /// </summary>
    public partial class MonthCalendar : UserControl, IObserver
    {
        public MonthCalendar()
        {
            InitializeComponent();
            InitializeStrings();
        }

        private void InitializeStrings()
        {
            MondayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.monday;
            TuesdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.tuesday;
            WednesdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.wednesday;
            ThursdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.thursday;
            FridayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.friday;
            SaturdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.saturday;
            SundayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.sunday;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

    }
}
