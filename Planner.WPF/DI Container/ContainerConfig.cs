using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Planner.Model;
using Planner.Model.Repositories;
using Planner.ViewModel;
using Planner.WPF.Windows;
using Planner.ViewModel.ViewModels;
using Planner.WPF.UserControls;

namespace Planner.WPF.DIContainer
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
