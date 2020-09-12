using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Model;
using ToDoList.Model.Repositories;
using ToDoList.ViewModel;
using ToDoList.WPF.Windows;
using ToDoList.ViewModel.ViewModels;
using ToDoList.WPF.UserControls;

namespace ToDoList.WPF.DIContainer
{
    public static class ContainerConfig
    {

        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<EventsCalendarViewModel>().As<IEventsCalendarViewModel>();
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            builder.RegisterType<ScheduleDbContext>().AsSelf();
            builder.RegisterType<UserControlFactory>().As<IUserControlFactory>();

            return builder.Build();
        }
    }
}
