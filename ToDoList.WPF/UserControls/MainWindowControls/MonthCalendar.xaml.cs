using System;
using System.Collections.Generic;
using System.Linq;
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
using ToDoList.ViewModel.ViewModels;

namespace ToDoList.WPF
{
    /// <summary>
    /// Interaction logic for MonthCalendar.xaml
    /// </summary>
    public partial class MonthCalendar : UserControl, IObserver
    {
        public MonthCalendar(IEventsCalendarViewModel viewModel)
        {
            InitializeComponent();
            InitializeStrings();
            DataContext = viewModel;
            viewModel.AddObserver(this);
            InitializeDayTextBlocks();
        }

        private void LoadEvents()
        {
            var viewModel = (DataContext as IEventsCalendarViewModel);
            
            
        }

        private void InitializeDayTextBlocks()
        {
            var viewModel = (DataContext as IEventsCalendarViewModel);
            
            for (int i = 0; i < 5; i++)
            {
                var tb = new TextBlock();
                int t = 0;
                Grid.SetColumn(tb, 0);
                Grid.SetRow(tb, i+1);

                tb.FontSize = 18;
                tb.Foreground = Brushes.WhiteSmoke;
                tb.Text = viewModel.Schedule.ElementAt(i*7).Key.Day.ToString();
                tb.Name = "Day" + ((i * 7) + (t + 1) - 1).ToString() + "TextBlock";
                RootGrid.Children.Add(tb);

                for(;t<7;t++)
                {
                    var textBlock = new TextBlock();

                    Grid.SetColumn(textBlock, t);
                    Grid.SetRow(textBlock,i+1);

                    textBlock.FontSize = 18;
                    textBlock.Foreground = Brushes.WhiteSmoke;
                    textBlock.Text = viewModel.Schedule.ElementAt((i * 7) + (t + 1) - 1).Key.Day.ToString();
                    textBlock.Name = "Day" + ((i * 7) + (t + 1) - 1).ToString() + "TextBlock";
                    RootGrid.Children.Add(textBlock);
                }
            }
        }
       
        private void LoadDays()
        {
            foreach (var t in RootGrid.Children)
            {
                var tb = t as TextBlock;
                if (tb != null)
                {
                    if (tb.Name.StartsWith("Day"))
                    {
                        for (int i = 0; i < 35; i++)
                        {
                            if (tb.Name.StartsWith("Day" + i.ToString()))
                            {
                                tb.Text = (DataContext as IEventsCalendarViewModel).Schedule.ElementAt(i).Key.Day.ToString();
                            }
                        }
                    }
                }
            }
        }

        private void InitializeStrings()
        {
            MondayWeekDayTextBlock.Text = ToDoList.WPF.Properties.Resources.monday;
            TuesdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.Resources.tuesday;
            WednesdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.Resources.wednesday;
            ThursdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.Resources.thursday;
            FridayWeekDayTextBlock.Text = ToDoList.WPF.Properties.Resources.friday;
            SaturdayWeekDayTextBlock.Text = ToDoList.WPF.Properties.Resources.saturday;
            SundayWeekDayTextBlock.Text = ToDoList.WPF.Properties.Resources.sunday;
        }

        public void Update()
        {
            LoadEvents();
            LoadDays();
        }

    }
}
