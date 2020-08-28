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

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeStrings();
        }

        private void InitializeStrings()
        {
            MondayWeekDayTextBlock.Text=ToDoList.WPF.Properties.strings.monday;
            TuesdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.tuesday;
            WednesdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.wednesday;
            ThursdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.thursday;
            FridayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.friday;
            SaturdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.saturday;
            SundayWeekDayTextBlock.Text = ToDoList.WPF.Properties.strings.sunday;
        }
    }
}
