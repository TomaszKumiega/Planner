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
using ToDoList.WPF.UserControls;

namespace ToDoList.WPF
{
    /// <summary>
    /// Interaction logic for MonthCalendar.xaml
    /// </summary>
    public partial class MonthCalendar : UserControl, IObserver
    {
        private readonly IUserControlFactory _controlFactory;
        private List<ItemsControl> _itemsControls;
        public MonthCalendar(IEventsCalendarViewModel viewModel, IUserControlFactory userControlFactory)
        {
            InitializeComponent();

            _itemsControls = new List<ItemsControl>();
            DataContext = viewModel;
            _controlFactory = userControlFactory;
            viewModel.AddObserver(this);

            InitializeStrings();
            InitializeGridLines();
            InitializeDayItemsControls();
            InitializeDayTextBlocks();
            LoadEvents();
        }

        private void InitializeGridLines()
        {
            for(int i=0; i<5; i++)
            {
                for(int k=0; k<7; k++)
                {
                    var rect = new Rectangle();
                    rect.Fill = Brushes.Transparent;
                    rect.Stroke = Brushes.WhiteSmoke;
                    rect.StrokeThickness = 0.5;

                    Grid.SetColumn(rect, k);
                    Grid.SetRow(rect, i+1);

                    RootGrid.Children.Add(rect);
                }
            }
            
        }

        private void InitializeDayItemsControls()
        {
            for(int i=0; i<5; i++)
            {
                for(int t=0;t<7;t++)
                {
                    var scrollBar = new ScrollViewer();
                    var itemControl = new ItemsControl();

                    Grid.SetColumn(scrollBar, t);
                    Grid.SetRow(scrollBar, i + 1);

                    scrollBar.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    scrollBar.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;

                    itemControl.Name = "Day" + ((i * 7) + (t + 1) - 1).ToString() + "ItemsControl";
                    scrollBar.Content = itemControl;
                    _itemsControls.Add(itemControl);
                    RootGrid.Children.Add(scrollBar);
                }
            }
        }

        private void LoadEvents()
        {
            var viewModel = (DataContext as IEventsCalendarViewModel);
            
            for(int i=0; i<35; i++)
            {
                var events = (DataContext as IEventsCalendarViewModel).Schedule.ElementAt(i).Value;

                foreach (var e in events)
                {
                    var eventsControl = _controlFactory.GetEventUserControl((DataContext as IEventsCalendarViewModel), e);

                    foreach (var itemsControl in _itemsControls)
                    {
                        if (itemsControl.Name == ("Day" + i.ToString() + "ItemsControl"))
                        {
                            itemsControl.Items.Add(eventsControl);
                        }
                    }
                }
            }
        }

        private void InitializeDayTextBlocks()
        {
            var viewModel = (DataContext as IEventsCalendarViewModel);
            
            for (int i = 0; i < 5; i++)
            {
                for(int t=0;t<7;t++)
                {
                    var textBlock = new TextBlock();
                    textBlock.FontSize = 14;
                    textBlock.Foreground = Brushes.WhiteSmoke;
                    textBlock.Text = viewModel.Schedule.ElementAt((i * 7) + (t + 1) - 1).Key.Day.ToString();
                    textBlock.Name = "Day" + ((i * 7) + (t + 1) - 1).ToString() + "TextBlock";

                    foreach (var itemsControl in _itemsControls)
                    {
                        if (itemsControl.Name == "Day" + ((i * 7) + (t + 1) - 1).ToString() + "ItemsControl")
                        {
                            itemsControl.Items.Add(textBlock);
                        }
                        
                    }
                }
            }
        }
       
        private void ClearItemsControls()
        {
            foreach (var itemsControl in _itemsControls)
            { 
                itemsControl.Items.Clear();    
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
            ClearItemsControls();
            InitializeDayTextBlocks();
            LoadEvents();
        }

    }
}
