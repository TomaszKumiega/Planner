using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Model;
using ToDoList.Model.Repositories;
using ToDoList.ViewModel;

namespace ToDoList.WPF
{
    public static class ContainerConfig
    {

        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<EventsCalendarViewModel>().As<IEventsCalendarViewModel>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<ScheduleDbContext>().AsSelf();
            builder.RegisterType<MonthCalendar>().AsSelf();

            return builder.Build();
        }
    }
}
